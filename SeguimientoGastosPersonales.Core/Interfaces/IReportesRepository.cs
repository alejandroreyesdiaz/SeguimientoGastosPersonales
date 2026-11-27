using SeguimientoGastosPersonales.Core.CustomEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeguimientoGastosPersonales.Core.Interfaces
{
    public interface IReportesRepository
    {
        Task<IEnumerable<GastoPorCategoria>> GetResumenPorCategoria(int usuarioId);
    }
}