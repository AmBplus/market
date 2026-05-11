using IranKish.Interface;
using IranKish.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranKish
{
    public static class IranKishServiceInjection
    {
        public static IServiceCollection IranKishServiceInjector(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIranKishIPGServices, IranKishIPGRepository>();
            services.AddScoped<IIRanKishVerification, IRanKishVerification>();
            services.AddSingleton<IranKishSettings>();
            services.AddScoped<WebHelper>();
            return services;
        }
    }
}
