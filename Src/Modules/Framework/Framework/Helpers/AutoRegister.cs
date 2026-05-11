using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helpers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AutoRegisterAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; }

        public AutoRegisterAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            Lifetime = lifetime;
        }
    }

}
