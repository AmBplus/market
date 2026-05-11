using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Services.Bank.Sepehr.Models;
using Microsoft.AspNetCore.Http;
namespace Services.Bank.Sepehr.Services;


public interface ISepehrServices
{
    Task<SepehrTokenResponse> GetTokenAsync(SepehrTokenRequest request);
    string GetPayloadSingleTashim(string iban, string amount, string nationalCode);
    Task<SepehrAdviceResult> AdviceAsync(string digitalReceipt, string terminalId); // متد برای تایید پرداخت
}


public class SepehrServices : ISepehrServices
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SepehrServices> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SepehrServices(HttpClient httpClient, ILogger<SepehrServices> logger, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetPayloadSingleTashim(string iban, string amount, string nationalCode)
    {
        // ایجاد شیء برای ساختار JSON
        var payload = new
        {
            SList = new[]
            {
                new { Amount = amount, Iban = iban }
            },
            id = "0010",
            nationalCode = nationalCode
        };

        // سریال‌سازی شیء به رشته JSON
        var jsonPayload = JsonSerializer.Serialize(payload);

        // بازگشت رشته JSON به عنوان نتیجه
        return jsonPayload;
    }

    public async Task<SepehrTokenResponse> GetTokenAsync(SepehrTokenRequest request)
    {
        var jsonRequest = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://sepehr.shaparak.ir:8081/V1/PeymentApi/GetToken", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<SepehrTokenResponse>(responseContent);
            if (tokenResponse.Status == 0)
            {
                tokenResponse.IsSuccessStatusCode = true;
                _logger.LogInformation("Response: {@TokenResponse}", tokenResponse);
            }
            else
            {
                _logger.LogInformation("Response: {@TokenResponse}", tokenResponse);
            }
            return tokenResponse;
        }
        else
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Error: {StatusCode}, {ErrorMessage}", response.StatusCode, responseContent);
            return new SepehrTokenResponse
            {
                Status = (int)response.StatusCode,
                ErrorMessage = responseContent,
                IsSuccessStatusCode = false
            };
        }
    }
    public async Task<SepehrAdviceResult> AdviceAsync(string digitalReceipt, string terminalId)
    {
        var request = new
        {
            digitalreceipt = digitalReceipt,
            Tid = terminalId
        };

        var jsonRequest = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://sepehr.shaparak.ir:8081/V1/PeymentApi/Advice", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var adviceResponse = JsonSerializer.Deserialize<AdviceResponse>(responseContent);
            _logger.LogInformation("Advice Response: {@AdviceResponse}", adviceResponse);
            var statusmsg = adviceResponse?.Status.ToLower() ;

            if(!string.IsNullOrWhiteSpace(statusmsg) )
            {
                var status = statusmsg  == "ok" || statusmsg == "duplicate" ? 0 : -1;
                var IsSuccessStatusCode = statusmsg  == "ok" || statusmsg == "duplicate" ? true : false;
                return new SepehrAdviceResult
                {
                    Status = status,
                    Message = adviceResponse?.Message,
                    ReturnId = adviceResponse?.ReturnId,
                    IsSuccessStatusCode = IsSuccessStatusCode,
                };
            }
            else
            {
                _logger.LogError("Error: {StatusCode}, {ErrorMessage}", response.StatusCode, responseContent);
                return new SepehrAdviceResult
                {
                    Status = -1,
                    Message = responseContent,
                    IsSuccessStatusCode = false
                };
            }
         
        }
        else
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Error: {StatusCode}, {ErrorMessage}", response.StatusCode, responseContent);
            return new SepehrAdviceResult
            {
                Status = -1,
                Message = responseContent,
                IsSuccessStatusCode = false
            };
        }
    }

}


