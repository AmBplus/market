#region usings
using Framework.Aspc.Helper;
using Framework.ConfigurationHelper;
using Framework.Infrastructure.ServiceInjector;
using Framework.Services.Orm;
using Framework.Settings.AppSettings;
using Infrastructure.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ServiceHost.Helper;
using ServiceHost.Services;
using Services.CachingProvider;
using System.Globalization;

#endregion usings

namespace ServiceHost.ServiceInjector
{
    public static class _ServicesInjectors
    {
        public static IServiceCollection ServiceHostInjector(this IServiceCollection services, IConfiguration configuration , IHostEnvironment hostEnvironment)
        {
            services.FrameworkServiceInjector(configuration);
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            #region اضافه کردن کلاس تنظیمات برنامه به دپدنسی اینجکشن

            ;
            services.Configure<ApplicationSettings>(configuration.GetSection(ApplicationSettings.KeyName).Bind);

            //services.AddSingleton<ApplicationSettings>
            //    (sp => sp.GetRequiredService<IOptions<ApplicationSettings>>().Value
            //    ?? throw new NullReferenceException("کلاس تنظیمات خالیست | the setting class is null"));
            #endregion
            // تنظیمات CacheConfig و تعیین پیشوند
            services.AddSingleton<ICacheConfig>(sp =>
            {
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                return new CacheConfig(httpContextAccessor);
            });
            services.AddHttpContextAccessor();
            #region Redis
            services.AddSingleton(sp =>
            {
                RedisBaseKeyPathSetting settingsInstance = new();
                configuration.GetRequiredSection(nameof(RedisBaseKeyPathSetting)).Bind(settingsInstance);

                return settingsInstance;
            });
            services.AddSingleton<RedisConnectionHelper>();
            services.AddSingleton(sp =>
            {
                var connectionHelper = sp.GetRequiredService<RedisConnectionHelper>();

                return connectionHelper.Connection.GetDatabase();
            });
            #endregion Redis
            services.AddSingleton(sp =>
            {
                DapperHelperOption settingsInstance = new();
                settingsInstance.ConnectionString = configuration.GetDbConnection();
                settingsInstance.IsCacheEnable = true;
                return settingsInstance;
            });
            services.AddScoped<IDatatableResponseHelper, DatatableResponseHelper>();
            services.AddScoped<IExcelHelper, ExcelHelper>();
            services.AddSingleton<IDapperHelper, DapperHelper>();
            //services.AddSingleton<ICacheService, RedisCacheService>();
            services.AddSingleton<ICacheService, InMemoryCacheService>();
    
            return services;
        }

        public static IServiceCollection InjectLocalization(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
            new CultureInfo("en"),
            new CultureInfo("fa-IR"),
            new CultureInfo("ar")
        };

                options.DefaultRequestCulture = new RequestCulture("fa-IR");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            return services;
        }


    }
}



