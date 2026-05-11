using Mapster;
using System;
using System.Linq;
using System.Reflection;

namespace Framework.Helpers
{
    // MappingConfigLoader.cs
    public static class MappingConfigLoader
    {
        private static TypeAdapterConfig? _cached;

        // MappingConfigLoader.cs
        public static void Load(params Assembly[] assemblies)
        {
            var config = TypeAdapterConfig.GlobalSettings;

            foreach (var assembly in assemblies)
            {
                var methods = assembly.GetTypes()
                    .Where(t => t.IsDefined(typeof(HasMappingsAttribute), false))
                    .Select(t => t.GetMethod("RegisterMappings", BindingFlags.Static | BindingFlags.Public, [typeof(TypeAdapterConfig)]))
                    .Where(m => m is not null);

                foreach (var method in methods)
                    method!.Invoke(null, [config]);
            }
        }
        public static TypeAdapterConfig GetConfig() =>
            _cached ?? throw new InvalidOperationException("MappingConfigLoader has not been initialized. Call Load() first.");
    }

}
