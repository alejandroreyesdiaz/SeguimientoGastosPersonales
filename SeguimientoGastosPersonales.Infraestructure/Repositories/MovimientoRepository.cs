using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SeguimientoGastosPersonales.Core.Entities;
using SeguimientoGastosPersonales.Core.Interfaces;
using SeguimientoGastosPersonales.Infrastructure.Data;

namespace SeguimientoGastosPersonales.Infrastructure.Repositories
{
    public class MovimientoRepository : IMovimientoRepository
    {
        private readonly SeguimientoGastosPersonalesContext _context;

        public MovimientoRepository(SeguimientoGastosPersonalesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movimiento>> GetAllMovimientosAsync()
        {
            return await _context.Movimientos.ToListAsync();
        }

        public async Task<Movimiento> GetMovimientosByIdAsync(int id)
        {
            return await _context.Movimientos.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task InsertMovimiento(Movimiento movimiento)
        {
            _context.Movimientos.Add(movimiento);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovimiento(Movimiento movimiento)
        {
            _context.Movimientos.Update(movimiento);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMovimiento(Movimiento movimiento)
        {
            _context.Movimientos.Remove(movimiento);
            await _context.SaveChangesAsync();
        }
    }
}
