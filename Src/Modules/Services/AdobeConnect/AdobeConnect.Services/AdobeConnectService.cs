using AdobeConnectSDK;
using AdobeConnectSDK.Common;
using AdobeConnectSDK.Model;
using AdobeConnectSDK.Extensions;
using Framework.ResultHelper;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdobeConnectSdk;

public class AdobeConnectService : IAdobeConnectService
{
    private AdobeConnectXmlAPI? _api;
    private readonly ILogger<AdobeConnectService> _logger;

    public AdobeConnectService(ILogger<AdobeConnectService> logger)
    {
        _logger = logger;
    }

    public ResultOperation Connect(string apiUrl, string username, string password)
    {
        var communicationProvider = new HttpCommunicationProvider();

        var settings = new SdkSettings
        {
            ServiceURL = apiUrl,
            NetUser = username,
            NetPassword = password
        };

        _api = new AdobeConnectXmlAPI(communicationProvider, settings);

        var loginResult = _api.Login();

        if (loginResult.Code == StatusCodes.OK)
        {
            return ResultOperation.ToSuccessResult();
        }

       _logger.LogError(loginResult.Code.ToString() + "/n" + loginResult.Exception);
  
        return ResultOperation.ToFailedResult(loginResult.Code.ToString());
    }

    public ResultOperation<AdobeMeetingDetail> GetMeetingDetail(string scoId)
    {
        EnsureConnected(nameof(GetMeetingDetail));

        var getMeetingResult = _api!.GetMeetingDetail(scoId);

        if (getMeetingResult.Code == StatusCodes.OK)
        {
            var data = new AdobeMeetingDetail
            {
                AccountId = getMeetingResult.Result.AccountId,
                DateBegin = getMeetingResult.Result.DateBegin,
                DateClosed = getMeetingResult.Result.DateClosed,
                DateCreated = getMeetingResult.Result.DateCreated,
                DateEnd = getMeetingResult.Result.DateEnd,
                DateModified = getMeetingResult.Result.DateModified,
                Duration = getMeetingResult.Result.Duration,
                FolderId = getMeetingResult.Result.FolderId,
                FullUrl = getMeetingResult.Result.FullUrl,
                Language = getMeetingResult.Result.Language,
                Name = getMeetingResult.Result.Name,
                PassingScore = getMeetingResult.Result.PassingScore,
                ScoId = getMeetingResult.Result.ScoId,
                SectionCount = getMeetingResult.Result.SectionCount,
                UrlPath = getMeetingResult.Result.UrlPath
            };

            return data.ToSuccessResult();
        }

        const string AdobeConnectFailedLogTemplate = "[adobe-connect] Failed to connect | exception message : {exceptionMessage} | inner exception : {innerExceptionMessage}";

        _logger.LogError(AdobeConnectFailedLogTemplate, getMeetingResult.Exception, getMeetingResult.InnerException?.Message);


        return ResultOperation<AdobeMeetingDetail>.ToFailedResult(AdobeConnectFailedLogTemplate);
    }

    private void EnsureConnected(string operation)
    {
        if (_api is null)
        {
            throw new InvalidOperationException($"for this operation '{operation}' need connect first");
        }
    }
    public ResultOperation<AdobeMeetingDetail> GetMeetingDetail(string apiUrl, string username, string password, string scoId)
    {
       var ConnectedResult = Connect(apiUrl, username, password);
        if (!ConnectedResult.IsSuccess)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return ResultOperation<AdobeMeetingDetail>.ToFailedResult(ConnectedResult.Message);
#pragma warning restore CS8604 // Possible null reference argument.
        }
        return GetMeetingDetail(scoId);
    }
}
