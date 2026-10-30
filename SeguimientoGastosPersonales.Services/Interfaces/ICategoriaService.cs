using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> GetAllCategoriasAsync();
        Task<Categoria?> GetCategoriaByIdAsync(int id);
        Task<Categoria> CrearCategoriaAsync(Categoria categoria);
        Task<bool> ActualizarCategoriaAsync(int id, Categoria categoria);
        Task<bool> EliminarCategoriaAsync(int id);
    }
}
