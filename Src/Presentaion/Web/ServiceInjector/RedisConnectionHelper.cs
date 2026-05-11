using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class RedisConnectionHelper
{
    public RedisConnectionHelper(IConfiguration configuration)
    {
        string? password = configuration.GetValue<string>("RedisConnection:Password");
        int? port = configuration.GetValue<int?>("RedisConnection:Port");
        string? ip = configuration.GetValue<string?>("RedisConnection:IP");

        ArgumentNullException.ThrowIfNull(ip);
        ArgumentNullException.ThrowIfNull(port);


        ConfigurationOptions options = ConfigurationOptions.Parse($"{ip}:{port}");
        //options.ClientName = providerName;
        options.Password = password;
        options.AbortOnConnectFail = false;


        lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(options, Console.Out);
        });
    }

    private Lazy<ConnectionMultiplexer> lazyConnection;
    public ConnectionMultiplexer Connection
    {
        get
        {
            return lazyConnection.Value;
        }
    }
}
