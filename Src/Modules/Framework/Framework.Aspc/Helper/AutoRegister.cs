using Framework.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Aspc.Helper;

public static class AutoRegisterExtensions
{
    /// <summary>
    /// Scans the given assemblies and registers all classes marked with <see cref="AutoRegisterAttribute"/> into the DI container.
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    /// <param name="assemblies">Assemblies to scan. Defaults to the calling assembly if none provided.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddAutoRegisteredServices(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        // Fall back to the calling assembly if no assemblies are specified
        if (assemblies.Length == 0)
            assemblies = [Assembly.GetCallingAssembly()];

        var attributeType = typeof(AutoRegisterAttribute);

        // Find all concrete classes decorated with AutoRegisterAttribute
        var types = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                !t.IsGenericTypeDefinition &&         // Skip open generic types
                t.IsDefined(attributeType, inherit: false)); // Faster than GetCustomAttribute; no instance allocation

        foreach (var type in types)
        {
            // Read the lifetime defined in the attribute
            var attr = (AutoRegisterAttribute)type
                .GetCustomAttribute(attributeType)!;

            // Resolve the service type: looks for a matching interface by convention (e.g. OrderService -> IOrderService)
            // Falls back to the class itself if no matching interface is found
            var serviceType = Array.Find(
                type.GetInterfaces(),
                i => i.Name == $"I{type.Name}") ?? type;

            // Register the service with the resolved type and lifetime
            services.Add(new ServiceDescriptor(serviceType, type, attr.Lifetime));
        }

        return services;
    }

}