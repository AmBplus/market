using Dapper;
using Microsoft.Data.SqlClient;
using Services.CachingProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Services.Orm
{
    public class DapperHelperOption
    {
        public string ConnectionString { get; set; }
        public bool IsCacheEnable { get; set; } = true;
    }
    public class DapperHelper : IDapperHelper, IDisposable
    {



        public DapperHelper(DapperHelperOption dapperHelperOption, ICacheService cacheService)
        {
            if (string.IsNullOrWhiteSpace(dapperHelperOption.ConnectionString))
            {
                throw new ArgumentNullException(nameof(dapperHelperOption.ConnectionString));
            }

            DapperHelperOption = dapperHelperOption;
            _ConntectionString = dapperHelperOption.ConnectionString;

            CacheService = cacheService;
        }

        private DapperHelperOption DapperHelperOption { get; }
        private string _ConntectionString { get; set; }
        private bool _disposed;

        private IDbConnection CreateConnection => new SqlConnection(connectionString: _ConntectionString);

        private ICacheService CacheService { get; }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, Dictionary<string, object>? param = null, long cacheSeconds = -1, string cacheKey = "")
        {
            //param = SetParams(param);
            var isEnableCache = cacheSeconds != -1 && DapperHelperOption.IsCacheEnable;
            if (isEnableCache)
            {
                cacheKey = GetCacheKey(cacheKey, sql, param);
                var cacheValue = await CacheService.GetAsync<IEnumerable<T>>(cacheKey);
                if (cacheValue != null)
                {
                    return cacheValue;
                }
            }

            using var connection = CreateConnection;

            var value = await connection.QueryAsync<T>(sql, param);
            if (value != null && isEnableCache)
            {
                await CacheService.SetAsync(cacheKey, value, TimeSpan.FromSeconds(cacheSeconds));
            }
            return value;
        }


        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, Dictionary<string, object>? param = null, long cacheSeconds = -1, string cacheKey = "")
        {
            //param = SetParams(param);
            var isEnableCache = cacheSeconds != -1 && DapperHelperOption.IsCacheEnable;
            if (isEnableCache)
            {
                cacheKey = GetCacheKey(cacheKey, sql, param);
                var cacheValue = await CacheService.GetAsync<T>(cacheKey);
                if (cacheValue != null)
                {
                    return cacheValue;
                }
            }
            using var connection = CreateConnection;
            var value = await connection.QueryFirstOrDefaultAsync<T>(sql, param);
            if (value != null && isEnableCache)
            {
                await CacheService.SetAsync(cacheKey, value, TimeSpan.FromSeconds(cacheSeconds));
            }
            return value;
        }
        private string GetCacheKey(string cacheKey, string sql, Dictionary<string, object>? parameters)
        {
            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                return cacheKey;
            }

            var paramString = new StringBuilder();
            if (parameters != null && parameters.Any())
            {
                bool first = true;
                foreach (var param in parameters) // مرتب‌سازی کلیدها
                {
                    if (!first)
                    {
                        paramString.Append(',');
                    }
                    first = false;

                    var value = param.Value;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string valueString = value switch
                    {
                        null => "NULL",
                        string str => str,
                        _ => value.ToString() // برای سایر نوع‌ها
                    };
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    paramString.Append(param.Key).Append('=').Append(valueString);
                }
            }

            return $"-{sql}:{paramString}";
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    CreateConnection.Dispose();
            _disposed = true;
        }
    }
}
