using AdobeConnectSdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdobeConnect.Services
{
    public static class AdobeConnectServiceInjection
    {
        public static IServiceCollection AdobeConnectionInjection(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddTransient<IAdobeConnectService, AdobeConnectService>();
            return services;
        }
    }
}
