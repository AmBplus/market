using AppCore.Data;
using Framework.Settings.AppSettings;
using Microsoft.EntityFrameworkCore;
using Web.Feature.Client.Common.Service;

namespace Feature.Client.Common; 

public static class ServicsInjection
{
    public static void RegisterCommonServices(this IServiceCollection services, IConfiguration configuration)
    {
        {
            services.AddScoped<ICommonServices, CommonServices>();
       
            services.AddSingleton<ApplicationSettings>();
        }
    }
}
