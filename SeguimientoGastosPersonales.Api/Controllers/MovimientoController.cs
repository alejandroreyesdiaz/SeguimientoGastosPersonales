
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SeguimientoGastosPersonales.Api.Response;
using SeguimientoGastosPersonales.Core.DTOs;
using SeguimientoGastosPersonales.Core.Entities;
using SeguimientoGastosPersonales.Core.Exceptions;
using SeguimientoGastosPersonales.Services.Interfaces;
using SeguimientoGastosPersonales.Services.Validators;

namespace SeguimientoGastosPersonales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoController : ControllerBase
    {
        private readonly IMovimientoService _movimientoService;
        private readonly IMapper _mapper;
        private readonly CrearGastoDtoValidator _gastoValidator;
        private readonly CrearIngresoDtoValidator _ingresoValidator;
        private readonly ActualizarMovimientoDtoValidator _actualizarValidator;

        public MovimientoController(
            IMapper mapper,
            IMovimientoService movimientoService,
            CrearGastoDtoValidator gastoValidator,
            CrearIngresoDtoValidator ingresoValidator,
            ActualizarMovimientoDtoValidator actualizarValidator)
        {
            _movimientoService = movimientoService;
            _mapper = mapper;
            _gastoValidator = gastoValidator;
            _ingresoValidator = ingresoValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Dto-AutoMapper

        // GET: api/Movimiento
        [HttpGet]
        public async Task<IActionResult> GetAllMovimientos()
        {
            var movimientos =
                await _movimientoService.GetAllMovimientosAsync();

            var movimientoDto =
                _mapper.Map<IEnumerable<MovimientoDto>>(movimientos);

            var response =
                new ApiResponse<IEnumerable<MovimientoDto>>(movimientoDto);

            return Ok(response);
        }

        // GET: api/Movimiento/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMovimientoById(int id)
        {
            var movimiento =
                await _movimientoService.GetMovimientoByIdAsync(id);

            if (movimiento == null)
                return NotFound(new
                {
                    message = "Movimiento no encontrado."
                });

            var movimientoDto =
                _mapper.Map<MovimientoDto>(movimiento);

            var response =
                new ApiResponse<MovimientoDto>(movimientoDto);

            return Ok(response);
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
                throw new BusinessException(
                    message: "Error de validación",
                    details: validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    }));
            }

            try
            {
                var movimiento = _mapper.Map<Movimiento>(gastoDto);

                await _movimientoService.RegistrarGastoAsync(movimiento);

                var resultDto =
                    _mapper.Map<MovimientoDto>(movimiento);

                return CreatedAtAction(
                    nameof(GetMovimientoById),
                    new { id = movimiento.Id },
                    resultDto);
            }
            catch (BusinessException)
            {
                throw;
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
                throw new BusinessException(
                    message: "Error de validación",
                    details: validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    }));
            }

            try
            {
                var movimiento = _mapper.Map<Movimiento>(ingresoDto);

                await _movimientoService.RegistrarIngresoAsync(movimiento);

                var resultDto =
                    _mapper.Map<MovimientoDto>(movimiento);

                return CreatedAtAction(
                    nameof(GetMovimientoById),
                    new { id = movimiento.Id },
                    resultDto);
            }
            catch (BusinessException)
            {
                throw;
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
            if (id != movimientoDto.Id)
                return BadRequest(new
                {
                    message = "El ID de la ruta no coincide con el ID del movimiento."
                });

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

            var movimiento =
                await _movimientoService.GetMovimientoByIdAsync(id);

            if (movimiento == null)
                return NotFound(new
                {
                    message = "Movimiento no encontrado."
                });

            try
            {
                _mapper.Map(movimientoDto, movimiento);

                var actualizado =
                    await _movimientoService.ActualizarMovimientoAsync(
                        id, movimiento);

                if (!actualizado)
                    return NotFound(new
                    {
                        message = "Movimiento no encontrado."
                    });

                var resultDto =
                    _mapper.Map<MovimientoDto>(movimiento);

                return Ok(resultDto);
            }
            catch (BusinessException)
            {
                throw;
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
                return BadRequest(new
                {
                    message = "El ID debe ser mayor que cero."
                });

            var movimiento =
                await _movimientoService.GetMovimientoByIdAsync(id);

            if (movimiento == null)
                return NotFound(new
                {
                    message = "Movimiento no encontrado."
                });

            try
            {
                await _movimientoService.EliminarMovimientoAsync(id);

                return NoContent();
            }
            catch (BusinessException)
            {
                throw;
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

        #endregion
    }
}
