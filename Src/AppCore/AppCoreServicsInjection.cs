using AppCore.Data;
using Framework.ConfigurationHelper;
using Framework.Settings.AppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore
{
    public static class AppCoreServicsInjection
    {
        public static void RegisterAppCoreServics(this IServiceCollection services, IConfiguration configuration)
        {
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(configuration.GetDbConnection()));

            }
        }
    }
}
