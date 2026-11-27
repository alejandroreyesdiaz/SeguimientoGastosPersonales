
using System.Data;

namespace SeguimientoGastosPersonales.Core.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
