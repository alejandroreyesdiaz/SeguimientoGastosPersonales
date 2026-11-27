using System;
using System.Collections.Generic;
using System.Text;
using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Services.Interfaces
{
    public interface ISecurityService
    {
        Task<Security?> GetLoginByCredentials(UserLogin userLogin);
        Task RegisterUser(Security security);
    }
}