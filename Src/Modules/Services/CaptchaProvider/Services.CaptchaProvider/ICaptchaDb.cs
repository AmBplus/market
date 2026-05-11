using Framework.ResultHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CaptchaProvider
{
    public interface ICaptchaDb
    {
        Task SetAsync(string key, string code, TimeSpan tokenTTL);
        Task<bool> ExitsAsync(string key);
        Task<ResultOperation<string>> GetDeleteAsync(string key);
    }
}
