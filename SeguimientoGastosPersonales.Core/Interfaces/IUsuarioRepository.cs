using System;
using System.Collections.Generic;
using System.Text;
using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Core.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllUsuariosAsync();
        Task<Usuario> GetUsuariosByIdAsync(int id);
        Task InsertUsuario(Usuario usuario);
        Task UpdateUsuario(Usuario usuario);
        Task DeleteUsuario(Usuario usuario);
    }
}
