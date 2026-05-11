using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Bank.Sepehr.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bank.Sepehr
{
    public static class ServiceInjection
    {
        public static IServiceCollection InjectSepehrServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddTransient<ISepehrServices, SepehrServices>();

            return services;
        }
    }
}
