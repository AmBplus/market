using Microsoft.AspNetCore.Http;
using IranKish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IranKish
{
     public interface IIranKishIPGServices 
    {
        Task<string> GetPayToken(IPGDataModel iPGData,HttpContext httpContext);
        Task<string> GetPayToken(IranKishGetPayTokenRequest iPGData);
    }
}
