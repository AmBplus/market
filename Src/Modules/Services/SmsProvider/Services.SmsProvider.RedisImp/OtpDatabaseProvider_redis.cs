using Framework.ResultHelper;
using Framework.Settings.AppSettings;
using StackExchange.Redis;

namespace Services.SmsProvider;

public class OtpDatabaseProvider_redis : IOtpDatabaseProvider
{
    IDatabase database;

    public OtpDatabaseProvider_redis(IDatabase database , RedisBaseKeyPathSetting redisBaseKeyPathSetting)
    {
        this.database = database;
        RedisBaseKeyPathSetting = redisBaseKeyPathSetting;
    }

    public RedisBaseKeyPathSetting RedisBaseKeyPathSetting { get; }

    public async Task<ResultOperation<string>> Get(string phoneNumber ,CancellationToken cancellationToken = default)
    {
        var key = GetKey(phoneNumber);
        var result = await database.StringGetAsync(key); 
        if(result.HasValue)
        {
            return result.ToString().ToSuccessResult();
        }
        else
        {
            return ResultOperation<string>.ToFailedResult();
        }
    }

    public async Task Save(string phoneNumber, string code ,TimeSpan expire, CancellationToken cancellationToken = default)
    {
       var key = GetKey(phoneNumber);
        var result = await database.StringGetAsync(key);
        if (!result.HasValue) {
            await database.StringSetAsync(key, code, expire);
        }
    }
    private string GetKey(string phoneNumber) {
        return $"{RedisBaseKeyPathSetting}{phoneNumber}";
    }
}