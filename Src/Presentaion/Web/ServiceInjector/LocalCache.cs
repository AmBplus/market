using Microsoft.Extensions.Caching.Memory;

namespace Services.CachingProvider
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly string _keyPrefix; // پیشوند کلید
        private TimeSpan DefaultExpiration { get; }

        public InMemoryCacheService(IMemoryCache cache, ICacheConfig cacheConfig)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _keyPrefix = cacheConfig?.KeyPrefix ?? throw new ArgumentNullException(nameof(cacheConfig));
            DefaultExpiration = TimeSpan.FromMinutes(cacheConfig.DefaultExpirationInMin);
            if (DefaultExpiration <= TimeSpan.Zero)
            {
                throw new ArgumentException("Default expiration must be positive.", nameof(cacheConfig.DefaultExpirationInMin));
            }
        }

        private string ApplyPrefix(string key) => $"{_keyPrefix}:{key}";

        public async Task<bool> ExistsAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return await Task.FromResult(false);
            }

            string fullKey = ApplyPrefix(key);
            return await Task.FromResult(_cache.TryGetValue(fullKey, out _));
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return await Task.FromResult(default(T));
            }

            string fullKey = ApplyPrefix(key);
            if (_cache.TryGetValue(fullKey, out T value))
            {
                return await Task.FromResult(value);
            }

            return await Task.FromResult(default(T));
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));
            }

            string fullKey = ApplyPrefix(key);
            var cacheEntryOptions = new MemoryCacheEntryOptions();

            TimeSpan effectiveExpiration = expiration ?? DefaultExpiration;
            if (effectiveExpiration > TimeSpan.Zero)
            {
                cacheEntryOptions.AbsoluteExpirationRelativeToNow = effectiveExpiration;
            }
            // Note: If expiration is zero or negative, the item won't expire (no expiration set),
            // but you might want to add validation or handle it differently based on requirements.

            _cache.Set(fullKey, value, cacheEntryOptions);
            await Task.CompletedTask;
        }

        public async Task RemoveAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                await Task.CompletedTask; // No-op for invalid key
                return;
            }

            string fullKey = ApplyPrefix(key);
            _cache.Remove(fullKey);
            await Task.CompletedTask;
        }
    }
}