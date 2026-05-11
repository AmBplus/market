using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Framework.Settings.AppSettings;
using Framework.ResultHelper;
using Framework.Resources;

namespace Services.SmsProvider
{
    public interface ISmsService
    {
        Task<ResultOperation> SendAsync(string phoneNumber, string message, CancellationToken cancellation);

    }
    public class SmsService : ISmsService
    {
        private readonly SmsProviderSettings _providerSettings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<SmsService> _logger;

        public SmsService(SmsProviderSettings providerSettings, HttpClient httpClient, ILogger<SmsService> logger)
        {
            _providerSettings = providerSettings;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ResultOperation> SendAsync(string phoneNumber, string message, CancellationToken cancellation)
        {
              const string SendSingleAction = "MySendSms";
              var requestData = new Dictionary<string, object>
               {
                   { "messageBody" , message },
                   { "senderNumber" , _providerSettings.DefaultSenderNumber },
                   { "recipientNumber" , phoneNumber }
               };

            var isSendSms =  await TrySendRequestToProvider(SendSingleAction, cancellation, requestData);
            if(isSendSms)
            {
                return ResultOperation.ToSuccessResult();
            }
            else
            {
                return ResultOperation.ToFailedResult(ErrorMessages.FailedSendMessages);
            }
        }
        private async Task<bool> TrySendRequestToProvider(string action, CancellationToken cancellation, Dictionary<string, object>? requestData = null)
        {
            try
            {
                await SendRequestToProvider(action, cancellation, requestData);

                return true;
            }
            catch (Exception ex)
            {
                const string LogTemplate = "unhandled exception when calling meshkat sms provider api --- action : {action} --- requestData : {requestData}";

                var requestDataFormated = requestData is not null ? string.Join(Environment.NewLine, requestData!) : "-";
                _logger.LogError(ex, LogTemplate, action, requestDataFormated);

                return false;
            }
        }
        private async Task SendRequestToProvider(string action, CancellationToken cancellation, Dictionary<string, object>? requestData = null)
        {
            const string BaseUrl = "http://app.mshdiau.ac.ir/m/sms/other/sms_magfa.asmx";
            using var request = new HttpRequestMessage(new HttpMethod("POST"), BaseUrl);

            var requestBody = string.Empty;

            if (requestData != null)
            {
                var sb = new StringBuilder();

                foreach (var item in requestData)
                {
                    sb.AppendLine($"<{item.Key}>{item.Value}</{item.Key}>");
                }
                requestBody = sb.ToString();
            }
            var contentAsXml = $"""
                            <?xml version="1.0" encoding="utf-8"?>
                            <soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
                              <soap12:Body>
                                <{action} xmlns="IaumSmsCenter">
                                    <myUsername>{_providerSettings.Username}</myUsername>
                                    <myPassword>{_providerSettings.Password}</myPassword>
                                    {requestBody}
                                </{action}>
                              </soap12:Body>
                            </soap12:Envelope>
                            """;

            request.Content = new StringContent(contentAsXml, Encoding.UTF8, "text/xml");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/soap+xml; charset=utf-8");

            var response = await _httpClient.SendAsync(request, cancellation);

            response.EnsureSuccessStatusCode();
        }
    }
}
