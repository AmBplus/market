using Framework.Services.Orm;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.ServiceInjector
{
    public static class _FrameworkServiceInjector
    {
        public static IServiceCollection FrameworkServiceInjector(this IServiceCollection services , IConfiguration configuration)
        {
            var connectionDb = configuration.GetConnectionString("SqlServerDatabase") ?? throw new NullReferenceException("کانکشن استرینگ دیتابیس خالیست : Database Connection Is Null");
            services.AddScoped<IDapperContext, DapperBaseContext>(pro => new DapperBaseContext(connectionDb));
            return services;
        }
    }
}
