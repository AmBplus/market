using ECommerce.AppCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.AppCore;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterAppCoreServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ECommerceDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }
}
