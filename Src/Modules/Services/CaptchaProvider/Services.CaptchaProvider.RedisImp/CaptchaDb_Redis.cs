using Framework.Resources;
using Framework.ResultHelper;
using Services.CaptchaProvider;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CaptchaProvider.CaptchaImplementation
{
    public class CaptchaDb_Redis : ICaptchaDb
    {
        // Redis Db
        IDatabase Context { get; set; }
        public CaptchaDb_Redis(IDatabase context)
        {
            Context = context;
        }
        public async Task<bool> ExitsAsync(string key)
        {
            return Context.KeyExists(key);
        }

        public async Task<ResultOperation<string>> GetDeleteAsync(string key)
        {
            var validCode = await Context.StringGetDeleteAsync(key);
            if (validCode.HasValue == false)
            {
                return ResultOperation<string>.ToFailedResult(ErrorMessages.CaptchaExpiredOrWrong);
            }
            return validCode.ToString().ToSuccessResult();
        }

        public async Task SetAsync(string key, string code, TimeSpan _tokenTTL)
        {
            await Context.StringSetAsync(key, code, _tokenTTL);
        }
    }
}
