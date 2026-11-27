using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeguimientoGastosPersonales.Api.Response;
using SeguimientoGastosPersonales.Core.CustomEntities;
using SeguimientoGastosPersonales.Services.Interfaces;

namespace SeguimientoGastosPersonales.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IReportesService _reportesService;

        public ReportesController(IReportesService reportesService)
        {
            _reportesService = reportesService;
        }

        // GET: api/Reportes/resumen-por-categoria/1
        [HttpGet("resumen-por-categoria/{usuarioId:int}")]
        public async Task<IActionResult> GetResumenPorCategoria(int usuarioId)
        {
            var resumen = await _reportesService.GetResumenPorCategoriaAsync(usuarioId);
            var response = new ApiResponse<IEnumerable<GastoPorCategoria>>(resumen);
            return Ok(response);
        }
    }
}