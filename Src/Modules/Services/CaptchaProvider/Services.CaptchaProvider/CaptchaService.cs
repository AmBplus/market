using Framework.NumberHelper;
using Framework.ResultHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CaptchaProvider; 

public class CaptchaService : ICaptchaService
{
    private readonly ICaptchaDb captchaDb;
    private readonly TimeSpan _tokenTTL = TimeSpan.FromMinutes(5);

    public CaptchaImageGenerator CaptchaImageGenerator { get; }

    public CaptchaService(ICaptchaDb _captchaDb , CaptchaImageGenerator captchaImageGenerator)
    {
        captchaDb = _captchaDb;
        CaptchaImageGenerator = captchaImageGenerator;
    }

    public async Task<CaptchaResult> CreateCaptchaAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var token = Guid.NewGuid().ToString();
        var key = $"sms_captcha_{token}";
        
        var code = GenerateCaptchaCode();
        

        await captchaDb.SetAsync(key, code, _tokenTTL);

        var captchaImageBytes = await CaptchaImageGenerator.GenerateAsync(code, cancellationToken);

        return new
            (
                token: token,
                captchBase64Data: Convert.ToBase64String(captchaImageBytes)
            );
    }

    public async Task<CaptchaResult> ReCreateCaptchaAsync(string token, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var key = $"sms_captcha_{token}";

        if (await captchaDb.ExitsAsync(key) == false)
        {
            throw new ArgumentException("the provided token does not exists");
        }

        var code = GenerateCaptchaCode();

        await captchaDb.SetAsync(key, code, _tokenTTL);

        var captchaImageBytes = await CaptchaImageGenerator.GenerateAsync(code, cancellationToken);

        return new
            (
                token: token,
                captchBase64Data: Convert.ToBase64String(captchaImageBytes)
            );
    }
    public async Task<ResultOperation> ValidateCaptchaCodeAsync(string token, string code, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var key = $"sms_captcha_{token}";

        var validCode = await captchaDb.GetDeleteAsync(key);

        if (validCode.IsSuccess == false)
        {
            return ResultOperation.ToFailedResult("کد امنیتی وارد شده منقضی شده است.");
        }

        if (validCode.Data != code)
        {
            return ResultOperation.ToFailedResult("کد امنیتی وارد شده اشتباه می باشد.");
        }

        return ResultOperation.ToSuccessResult();
    }

    private static string GenerateCaptchaCode()
    {
        const string Letters = "012346789";
        const int CaptchaCodeLength = 5;

        var chars = Enumerable.Range(0, CaptchaCodeLength)
            .Select(_ => Letters[Random.Shared.Next(0, Letters.Length)])
            .ToArray();

        return new(chars);
    }
}
