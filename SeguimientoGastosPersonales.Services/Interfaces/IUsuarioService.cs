using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllUsuariosAsync();
        Task<Usuario?> GetUsuarioByIdAsync(int id);
        Task<Usuario> CrearUsuarioAsync(Usuario usuario);
        Task<bool> ActualizarUsuarioAsync(int id, Usuario usuario);
        Task<bool> EliminarUsuarioAsync(int id);
    }
}