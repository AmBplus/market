using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetMng.Service
{
    public interface IUserAssets
    {
        public Task<string> GetUserProfileUrl();
        public Task<string> GetUserProfileUrl(string avatar);
    }

}
