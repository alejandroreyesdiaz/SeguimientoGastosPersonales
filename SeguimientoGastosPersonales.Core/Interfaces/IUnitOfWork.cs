
using SeguimientoGastosPersonales.Core.Entities;
using System.Data;

namespace SeguimientoGastosPersonales.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Categoria> CategoriaRepository { get; }
        IBaseRepository<Usuario> UsuarioRepository { get; }
        IBaseRepository<Movimiento> MovimientoRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();

        // Dapper
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        IDbConnection? GetDbConnection();
        IDbTransaction? GetDbTransaction();
    }
}
