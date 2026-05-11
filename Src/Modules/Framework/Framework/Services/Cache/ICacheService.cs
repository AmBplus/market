using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CachingProvider;

public interface ICacheService
{
    Task<bool> ExistsAsync(string key);
    Task<T> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
}

public interface ICacheConfig
{
    string KeyPrefix { get; }
    int DefaultExpirationInMin { get; }
}