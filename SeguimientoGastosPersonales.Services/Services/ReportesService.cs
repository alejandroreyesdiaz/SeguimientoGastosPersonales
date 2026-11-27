using System;
using System.Collections.Generic;
using System.Text;
using SeguimientoGastosPersonales.Core.CustomEntities;
using SeguimientoGastosPersonales.Core.Interfaces;
using SeguimientoGastosPersonales.Services.Interfaces;

namespace SeguimientoGastosPersonales.Services.Services
{
    public class ReportesService : IReportesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GastoPorCategoria>> GetResumenPorCategoriaAsync(int usuarioId)
        {
            return await _unitOfWork.ReportesRepository.GetResumenPorCategoria(usuarioId);
        }
    }
}