using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Services.Orm
{
    public interface IDapperHelper
    {
        public Task<IEnumerable<T>> QueryAsync<T>(string sql, Dictionary<string, object>? param = null, long cacheDuration = -1, string cacheKey = "");
        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, Dictionary<string, object>? param = null, long cacheDuration = -1, string cacheKey = "");
    }
}
