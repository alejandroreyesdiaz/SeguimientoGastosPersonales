using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Services.Interfaces
{
    public interface IMovimientoService
    {
        Task<IEnumerable<Movimiento>> GetAllMovimientosAsync();

        Task<Movimiento?> GetMovimientoByIdAsync(int id);

        Task RegistrarGastoAsync(Movimiento movimiento);

        Task RegistrarIngresoAsync(Movimiento movimiento);

        Task<bool> ActualizarMovimientoAsync(
            int id, Movimiento movimiento);

        Task<bool> EliminarMovimientoAsync(int id);
    }
}