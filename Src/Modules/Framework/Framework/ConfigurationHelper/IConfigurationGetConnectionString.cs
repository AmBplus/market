using Framework.ConstHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ConfigurationHelper
{
    public static class ConfigurationGetConnectionString
    {
        public static string GetDbConnection(this IConfiguration configuration  )
        {
            var connectionDb = configuration.GetConnectionString(SqlDbConnectionNameConst.ConnectionName) 
                ?? throw new NullReferenceException("Sql Connection Is Null , رشته ارتباطی با دیتابیس نال است");
            return connectionDb;
        }
    }
}
