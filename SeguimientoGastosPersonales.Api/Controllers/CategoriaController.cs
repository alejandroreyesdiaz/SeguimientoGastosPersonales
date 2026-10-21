
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SeguimientoGastosPersonales.Core.DTOs;
using SeguimientoGastosPersonales.Services.Interfaces;

namespace SeguimientoGastosPersonales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IValidator<CrearCategoriaDto> _crearValidator;
        private readonly IValidator<ActualizarCategoriaDto> _actualizarValidator;

        public CategoriaController(
            ICategoriaService categoriaService,
            IValidator<CrearCategoriaDto> crearValidator,
            IValidator<ActualizarCategoriaDto> actualizarValidator)
        {
            _categoriaService = categoriaService;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        // GET: api/Categoria
        [HttpGet]
        public async Task<IActionResult> GetAllCategorias()
        {
            try
            {
                var categorias =
                    await _categoriaService.GetAllCategoriasAsync();

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al obtener las categorías.",
                    error = ex.Message
                });
            }
        }

        // GET: api/Categoria/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoriaById(int id)
        {
            try
            {
                var categoria =
                    await _categoriaService.GetCategoriaByIdAsync(id);

                if (categoria == null)
                {
                    return NotFound(new
                    {
                        message = "No se encontró la categoría."
                    });
                }

                return Ok(categoria);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al obtener la categoría.",
                    error = ex.Message
                });
            }
        }

        // POST: api/Categoria
        [HttpPost]
        public async Task<IActionResult> CrearCategoria(
            [FromBody] CrearCategoriaDto categoriaDto)
        {
            var validationResult =
                await _crearValidator.ValidateAsync(categoriaDto);

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
                var categoria =
                    await _categoriaService.CrearCategoriaAsync(categoriaDto);

                return CreatedAtAction(
                    nameof(GetCategoriaById),
                    new { id = categoria.Id },
                    categoria);
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
                    message = "Error al crear la categoría.",
                    error = ex.Message
                });
            }
        }

        // PUT: api/Categoria/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarCategoria(
            int id,
            [FromBody] ActualizarCategoriaDto categoriaDto)
        {
            var validationResult =
                await _actualizarValidator.ValidateAsync(categoriaDto);

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
                    await _categoriaService.ActualizarCategoriaAsync(
                        id, categoriaDto);

                if (!actualizado)
                {
                    return NotFound(new
                    {
                        message = "No se encontró la categoría."
                    });
                }

                return Ok(new
                {
                    message = "Categoría actualizada correctamente."
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
                    message = "Error al actualizar la categoría.",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/Categoria/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarCategoria(int id)
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
                    await _categoriaService.EliminarCategoriaAsync(id);

                if (!eliminado)
                {
                    return NotFound(new
                    {
                        message = "No se encontró la categoría."
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
                    message = "Error al eliminar la categoría.",
                    error = ex.Message
                });
            }
        }
    }
}
