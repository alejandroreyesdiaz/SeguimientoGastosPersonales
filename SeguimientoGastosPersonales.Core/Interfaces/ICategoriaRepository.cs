using System;
using System.Collections.Generic;
using System.Text;
using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Core.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> GetAllCategoriasAsync();
        Task<Categoria> GetCategoriasByIdAsync(int id);
        Task InsertCategoria(Categoria categoria);
        Task UpdateCategoria(Categoria categoria);
        Task DeleteCategoria(Categoria categoria);
    }
}