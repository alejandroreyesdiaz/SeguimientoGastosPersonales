
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using SeguimientoGastosPersonales.Core.Interfaces;
using System.Data;

namespace SeguimientoGastosPersonales.Infrastructure.Repositories
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _config;
        private readonly string _mySqlConn;

        public DbConnectionFactory(IConfiguration config)
        {
            _config = config;

            _mySqlConn =
                _config.GetConnectionString("DefaultConnection")
                ?? string.Empty;
        }

        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(_mySqlConn);
        }
    }
}
