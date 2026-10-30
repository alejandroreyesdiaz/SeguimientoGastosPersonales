using System;
using System.Collections.Generic;
using System.Text;
using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Core.Interfaces
{
    public interface IBaseRepository<T>
        where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(int id);
    }
}