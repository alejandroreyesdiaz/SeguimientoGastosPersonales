
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SeguimientoGastosPersonales.Core.Entities;
using SeguimientoGastosPersonales.Core.Interfaces;
using SeguimientoGastosPersonales.Infrastructure.Data;
using System.Data;

namespace SeguimientoGastosPersonales.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SeguimientoGastosPersonalesContext _context;

        private readonly IBaseRepository<Categoria>? _categoriaRepository;
        private readonly IBaseRepository<Usuario>? _usuarioRepository;
        private readonly IBaseRepository<Movimiento>? _movimientoRepository;

        // Dapper
        private readonly IDapperContext _dapper;
        private IDbContextTransaction? _efTransaction;

        public UnitOfWork(
            SeguimientoGastosPersonalesContext context,
            IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }

        public IBaseRepository<Categoria> CategoriaRepository =>
            _categoriaRepository ??
            new BaseRepository<Categoria>(_context);

        public IBaseRepository<Usuario> UsuarioRepository =>
            _usuarioRepository ??
            new BaseRepository<Usuario>(_context);

        public IBaseRepository<Movimiento> MovimientoRepository =>
            _movimientoRepository ??
            new BaseRepository<Movimiento>(_context);

        public void Dispose()
        {
            if (_context != null)
            {
                _efTransaction?.Dispose();
                _context.Dispose();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #region Dapper

        public async Task BeginTransactionAsync()
        {
            if (_efTransaction == null)
            {
                _efTransaction =
                    await _context.Database.BeginTransactionAsync();

                var conn = _context.Database.GetDbConnection();
                var tx = _efTransaction.GetDbTransaction();

                _dapper.SetAmbientConnection(conn, tx);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

                if (_efTransaction != null)
                {
                    await _efTransaction.CommitAsync();
                    _efTransaction.Dispose();
                    _efTransaction = null;
                }
            }
            finally
            {
                _dapper.ClearAmbientConnection();
            }
        }

        public async Task RollbackAsync()
        {
            if (_efTransaction != null)
            {
                await _efTransaction.RollbackAsync();
                _efTransaction.Dispose();
                _efTransaction = null;
            }

            _dapper.ClearAmbientConnection();
        }

        public IDbConnection? GetDbConnection()
        {
            return _context.Database.GetDbConnection();
        }

        public IDbTransaction? GetDbTransaction()
        {
            return _efTransaction?.GetDbTransaction();
        }

        #endregion
    }
}
