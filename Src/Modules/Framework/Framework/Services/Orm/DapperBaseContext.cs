using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;




namespace Framework.Services.Orm
{
    public class DapperBaseContext(string conntectionString) : IDapperContext
    {
        private bool _disposed;
        public string ConnectionString { get; } = conntectionString;

        public void Dispose()
        { 
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    CreateConnection.Dispose();
            _disposed = true;
        }
#pragma warning disable CS01169
        public IDbConnection CreateConnection =>  new SqlConnection(connectionString: ConnectionString);
#pragma warning restore CS01169

    }
}
