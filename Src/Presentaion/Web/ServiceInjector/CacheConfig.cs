
using Microsoft.AspNetCore.Http;



namespace Services.CachingProvider
{
    public class CacheConfig : ICacheConfig
    {
        public string KeyPrefix { get; private set; }
        public int DefaultExpirationInMin { get; private set; }

        public CacheConfig(IHttpContextAccessor httpContextAccessor)
        {
            DefaultExpirationInMin = 5; // مقدار ثابت پیش‌فرض

            var host = httpContextAccessor.HttpContext?.Request.Host.Host;

            if (!string.IsNullOrEmpty(host))
            {
                // حذف پسوند‌های دامنه
                var domain = host
                    .Replace("https://", string.Empty)
                    .Replace("http://", string.Empty)
                    .Replace(".com", string.Empty)
                    .Replace(".ir", string.Empty)
                    .Replace(".net", string.Empty)
                    .Replace(".org", string.Empty);

                KeyPrefix = domain;
            }
            else
            {
                KeyPrefix = "DefaultApp"; // پیشوند پیش‌فرض
            }
        }
    }
}
