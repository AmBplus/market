using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.CachingProvider;

    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _database;
        private readonly string _keyPrefix; // پیشوند کلید
        private TimeSpan DefaultExpiration {  get;  }   

        public RedisCacheService(IDatabase database, ICacheConfig cacheConfig)
        {
            _database = database;
            _keyPrefix = cacheConfig.KeyPrefix;
            DefaultExpiration = TimeSpan.FromMinutes(cacheConfig.DefaultExpirationInMin);
        }

        private string ApplyPrefix(string key) => $"{_keyPrefix}:{key}";

        public async Task<bool> ExistsAsync(string key)
        {
            key = ApplyPrefix(key);
            return await _database.KeyExistsAsync(key);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            key = ApplyPrefix(key);
            var value = await _database.StringGetAsync(key);
            if (!value.HasValue)
                return default;

#pragma warning disable CS8604 // Possible null reference argument.
        return JsonSerializer.Deserialize<T>((string)value);
#pragma warning restore CS8604 // Possible null reference argument.
    }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            key = ApplyPrefix(key);
            var json = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, json, expiration ?? DefaultExpiration);
        }

        public async Task RemoveAsync(string key)
        {
            key = ApplyPrefix(key);
            await _database.KeyDeleteAsync(key);
        }
    }


