using System;
using System.Collections.Generic;
using System.Text;
using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Core.Interfaces
{
    public interface ISecurityRepository : IBaseRepository<Security>
    {
        Task<Security?> GetLoginByCredentials(UserLogin login);
    }
}
