using Framework.Resources;
using Framework.ResultHelper;
using Framework.Settings.AppSettings;

namespace Services.SmsProvider;
public interface IOtpProvider
{
    public Task<ResultOperation> Send(string phoneNumber, CancellationToken cancellation = default);
    public Task<bool> IsCanSendOtp(string phoneNumber ,CancellationToken cancellation = default);
    public Task<ResultOperation<string>> TryValidateOtp(string phoneNumber, string code, CancellationToken cancellation = default);

}
public class OtpProvider : IOtpProvider
{
    public OtpProvider(IOtpDatabaseProvider otpDatabaseProvider, OtpSettings otpSettings, ISmsService smsService)
    {
        OtpDatabaseProvider = otpDatabaseProvider;
        OtpSettings = otpSettings;
        SmsService = smsService;
    }

    public IOtpDatabaseProvider OtpDatabaseProvider { get; }
    public OtpSettings OtpSettings { get; }
    public ISmsService SmsService { get; }

    public async Task<bool> IsCanSendOtp(string phoneNumber, CancellationToken cancellation = default)
    {
        var result = await OtpDatabaseProvider.Get(phoneNumber, cancellation);
        if (!result.IsSuccess)
        {
            return true;
        }
        return false;
    }

    public async Task<ResultOperation> Send(string phoneNumber, CancellationToken cancellation = default)
    {
        // Generate Code 
        var code = GetCode();
        // Save Code in OtpDb
        await OtpDatabaseProvider.Save(phoneNumber, code, OtpSettings.OtpCodeExpiryAfter, cancellation);
        // Send Sms
        var message = GetOtpMessage(code);
        var result =  await SmsService.SendAsync(phoneNumber, message, cancellation);
        return result;
    }
    public async Task<ResultOperation<string>> TryValidateOtp(string phoneNumber, string code, CancellationToken cancellation = default)
    {
        // Get Code From OtpDb
        // Check 
        var errorMsg = ErrorMessages.WrongCodeOrExpireMsg;
        var result = await OtpDatabaseProvider.Get(phoneNumber, cancellation);
        if (!result.IsSuccess)
        {
            return ResultOperation<string>.ToFailedResult(errorMsg);
        }
        else
        {
            try
            {
                var codeNumber = Convert.ToInt32(code);
                var codeInDb = Convert.ToInt32(result.Data);
                if (codeNumber == codeInDb)
                {
                    return code.ToSuccessResult();
                }
            }
            catch (Exception)
            {
                return code.ToFailedResult(errorMsg);
            }
            return code.ToFailedResult(errorMsg);

        }
    }

private string GetCode()
{
        //Get 5 Digit Number
    return System.Security.Cryptography.RandomNumberGenerator.GetInt32(10_000, 99_999).ToString();
}
private string GetOtpMessage(string code)
{
    return string.Format(Framework.Resources.Messages.OtpMessage, code);
}
}
public interface IOtpDatabaseProvider
{
    public Task Save(string phoneNumber, string code, TimeSpan expireTime, CancellationToken cancellation = default);
    public Task<ResultOperation<string>> Get(string phoneNumber, CancellationToken cancellation = default);
}
