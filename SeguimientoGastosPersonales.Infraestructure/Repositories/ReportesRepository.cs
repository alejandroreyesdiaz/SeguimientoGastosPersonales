using System;
using System.Collections.Generic;
using System.Text;
using SeguimientoGastosPersonales.Core.CustomEntities;
using SeguimientoGastosPersonales.Core.Interfaces;
using SeguimientoGastosPersonales.Infrastructure.Queries;

namespace SeguimientoGastosPersonales.Infrastructure.Repositories
{
    public class ReportesRepository : IReportesRepository
    {
        private readonly IDapperContext _dapper;

        public ReportesRepository(IDapperContext dapper)
        {
            _dapper = dapper;
        }

        public async Task<IEnumerable<GastoPorCategoria>> GetResumenPorCategoria(int usuarioId)
        {
            return await _dapper.QueryAsync<GastoPorCategoria>(
                ReportesQueries.ResumenPorCategoriaMySql,
                new { UsuarioId = usuarioId });
        }
    }
}