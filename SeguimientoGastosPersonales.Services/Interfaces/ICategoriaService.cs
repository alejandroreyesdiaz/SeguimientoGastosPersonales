
using SeguimientoGastosPersonales.Core.DTOs;

namespace SeguimientoGastosPersonales.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDto>> GetAllCategoriasAsync();

        Task<CategoriaDto?> GetCategoriaByIdAsync(int id);

        Task<CategoriaDto> CrearCategoriaAsync(
            CrearCategoriaDto dto);

        Task<bool> ActualizarCategoriaAsync(
            int id, ActualizarCategoriaDto dto);

        Task<bool> EliminarCategoriaAsync(int id);


        private static void ValidarCategoria(
            string nombre, string tipo)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException(
                    "El nombre de la categoría es obligatorio.");

            if (nombre.Trim().Length > 100)
                throw new ArgumentException(
                    "El nombre no puede superar los 100 caracteres.");

            if (tipo != "INGRESO" && tipo != "GASTO")
                throw new ArgumentException(
                    "El tipo debe ser INGRESO o GASTO.");
        }

    }
}
