using SeguimientoGastosPersonales.Core.Entities;
using SeguimientoGastosPersonales.Core.QueryFilters;
using SeguimientoGastosPersonales.Core.DTOs;

namespace SeguimientoGastosPersonales.Services.Interfaces
{
    public interface IMovimientoService
    {
        Task<IEnumerable<Movimiento>> GetAllMovimientosAsync(MovimientoQueryFilter? filters = null);

        Task<Movimiento?> GetMovimientoByIdAsync(int id);

        Task RegistrarGastoAsync(Movimiento movimiento);

        Task RegistrarIngresoAsync(Movimiento movimiento);

        Task<bool> ActualizarMovimientoAsync(
            int id, Movimiento movimiento);

        Task<bool> EliminarMovimientoAsync(int id);

        Task<BalanceDto> ConsultarBalanceAsync(int? usuarioId = null);
    }
}