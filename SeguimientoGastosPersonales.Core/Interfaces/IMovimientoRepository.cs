using System;
using System.Collections.Generic;
using System.Text;
using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Core.Interfaces
{
    public interface IMovimientoRepository
    {
        Task<IEnumerable<Movimiento>> GetAllMovimientosAsync();
        Task<Movimiento> GetMovimientosByIdAsync(int id);
        Task InsertMovimiento(Movimiento movimiento);
        Task UpdateMovimiento(Movimiento movimiento);
        Task DeleteMovimiento(Movimiento movimiento);
    }
}