using SeguimientoGastosPersonales.Core.DTOs;

namespace SeguimientoGastosPersonales.Services.Interfaces
{
    public interface IMovimientoService
    {
        Task<IEnumerable<MovimientoDto>> GetAllMovimientosAsync();

        Task<MovimientoDto?> GetMovimientoByIdAsync(int id);

        Task<MovimientoDto> RegistrarGastoAsync(
            CrearGastoDto dto);

        Task<MovimientoDto> RegistrarIngresoAsync(
            CrearIngresoDto dto);

        Task<bool> ActualizarMovimientoAsync(
            int id, ActualizarMovimientoDto dto);

        Task<bool> EliminarMovimientoAsync(int id);
    }
}