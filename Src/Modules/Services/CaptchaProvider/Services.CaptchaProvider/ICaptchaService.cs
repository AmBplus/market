using Framework.ResultHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CaptchaProvider
{

    public interface ICaptchaService
    {
        public Task<ResultOperation> ValidateCaptchaCodeAsync(string token, string code, CancellationToken cancellationToken);
        public Task<CaptchaResult> ReCreateCaptchaAsync(string token, CancellationToken cancellationToken);
        public Task<CaptchaResult> CreateCaptchaAsync(CancellationToken cancellationToken);
    }
    public record CaptchaResult
    {
        public CaptchaResult(string token, string captchBase64Data)
        {
            Token = token;
            CaptchBase64Data = captchBase64Data;
        }

        public string Token { get; init; }
        public string CaptchBase64Data { get; init; }
    }

}
