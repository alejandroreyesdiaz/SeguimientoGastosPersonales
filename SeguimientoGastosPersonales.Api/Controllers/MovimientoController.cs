
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SeguimientoGastosPersonales.Core.DTOs;
using SeguimientoGastosPersonales.Services.Interfaces;

namespace SeguimientoGastosPersonales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientoController : ControllerBase
    {
        private readonly IMovimientoService _movimientoService;
        private readonly IValidator<CrearGastoDto> _gastoValidator;
        private readonly IValidator<CrearIngresoDto> _ingresoValidator;
        private readonly IValidator<ActualizarMovimientoDto> _actualizarValidator;

        public MovimientoController(
            IMovimientoService movimientoService,
            IValidator<CrearGastoDto> gastoValidator,
            IValidator<CrearIngresoDto> ingresoValidator,
            IValidator<ActualizarMovimientoDto> actualizarValidator)
        {
            _movimientoService = movimientoService;
            _gastoValidator = gastoValidator;
            _ingresoValidator = ingresoValidator;
            _actualizarValidator = actualizarValidator;
        }

        // GET: api/Movimiento
        [HttpGet]
        public async Task<IActionResult> GetAllMovimientos()
        {
            try
            {
                var movimientos =
                    await _movimientoService.GetAllMovimientosAsync();

                return Ok(movimientos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al obtener los movimientos.",
                    error = ex.Message
                });
            }
        }

        // GET: api/Movimiento/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMovimientoById(int id)
        {
            try
            {
                var movimiento =
                    await _movimientoService.GetMovimientoByIdAsync(id);

                if (movimiento == null)
                {
                    return NotFound(new
                    {
                        message = "No se encontró el movimiento."
                    });
                }

                return Ok(movimiento);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al obtener el movimiento.",
                    error = ex.Message
                });
            }
        }

        // POST: api/Movimiento/gastos
        [HttpPost("gastos")]
        public async Task<IActionResult> RegistrarGasto(
            [FromBody] CrearGastoDto gastoDto)
        {
            var validationResult =
                await _gastoValidator.ValidateAsync(gastoDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            try
            {
                var movimiento =
                    await _movimientoService.RegistrarGastoAsync(gastoDto);

                return CreatedAtAction(
                    nameof(GetMovimientoById),
                    new { id = movimiento.Id },
                    movimiento);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al registrar el gasto.",
                    error = ex.Message
                });
            }
        }

        // POST: api/Movimiento/ingresos
        [HttpPost("ingresos")]
        public async Task<IActionResult> RegistrarIngreso(
            [FromBody] CrearIngresoDto ingresoDto)
        {
            var validationResult =
                await _ingresoValidator.ValidateAsync(ingresoDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            try
            {
                var movimiento =
                    await _movimientoService.RegistrarIngresoAsync(ingresoDto);

                return CreatedAtAction(
                    nameof(GetMovimientoById),
                    new { id = movimiento.Id },
                    movimiento);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al registrar el ingreso.",
                    error = ex.Message
                });
            }
        }

        // PUT: api/Movimiento/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarMovimiento(
            int id,
            [FromBody] ActualizarMovimientoDto movimientoDto)
        {
            var validationResult =
                await _actualizarValidator.ValidateAsync(movimientoDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            try
            {
                var actualizado =
                    await _movimientoService.ActualizarMovimientoAsync(
                        id, movimientoDto);

                if (!actualizado)
                {
                    return NotFound(new
                    {
                        message = "No se encontró el movimiento."
                    });
                }

                return Ok(new
                {
                    message = "Movimiento actualizado correctamente."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al actualizar el movimiento.",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/Movimiento/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarMovimiento(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "El ID debe ser mayor que cero."
                });
            }

            try
            {
                var eliminado =
                    await _movimientoService.EliminarMovimientoAsync(id);

                if (!eliminado)
                {
                    return NotFound(new
                    {
                        message = "No se encontró el movimiento."
                    });
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al eliminar el movimiento.",
                    error = ex.Message
                });
            }
        }
    }
}
