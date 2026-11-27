using System;
using System.Collections.Generic;
using System.Text;
using SeguimientoGastosPersonales.Core.CustomEntities;

namespace SeguimientoGastosPersonales.Services.Interfaces
{
    public interface IReportesService
    {
        Task<IEnumerable<GastoPorCategoria>> GetResumenPorCategoriaAsync(int usuarioId);
    }
}
