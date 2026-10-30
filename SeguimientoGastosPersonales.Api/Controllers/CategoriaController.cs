
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
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IMapper _mapper;
        private readonly CrearCategoriaDtoValidator _crearValidator;
        private readonly ActualizarCategoriaDtoValidator _actualizarValidator;

        public CategoriaController(
            IMapper mapper,
            ICategoriaService categoriaService,
            CrearCategoriaDtoValidator crearValidator,
            ActualizarCategoriaDtoValidator actualizarValidator)
        {
            _mapper = mapper;
            _categoriaService = categoriaService;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

     
        [HttpGet]
        public async Task<IActionResult> GetAllCategorias()
        {
            var categorias = await _categoriaService.GetAllCategoriasAsync();

            var categoriaDto =
                _mapper.Map<IEnumerable<CategoriaDto>>(categorias);

            var response =
                new ApiResponse<IEnumerable<CategoriaDto>>(categoriaDto);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoriaById(int id)
        {
            var categoria = await _categoriaService.GetCategoriaByIdAsync(id);

            if (categoria == null)
                return NotFound(new
                {
                    message = "No se encontró la categoría."
                });

            var categoriaDto = _mapper.Map<CategoriaDto>(categoria);

            var response = new ApiResponse<CategoriaDto>(categoriaDto);

            return Ok(response);
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
                var categoria = _mapper.Map<Categoria>(categoriaDto);

                await _categoriaService.CrearCategoriaAsync(categoria);

                var resultDto = _mapper.Map<CategoriaDto>(categoria);

                return CreatedAtAction(
                    nameof(GetCategoriaById),
                    new { id = categoria.Id },
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
                    message = "Error al crear la categoría.",
                    error = ex.Message
                });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarCategoria(
            int id,
            [FromBody] ActualizarCategoriaDto categoriaDto)
        {
            if (id != categoriaDto.Id)
            {
                return BadRequest(new
                {
                    message = "El ID de la categoría no coincide con el de la ruta."
                });
            }

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
                var categoria = _mapper.Map<Categoria>(categoriaDto);

                var actualizado =
                    await _categoriaService.ActualizarCategoriaAsync(id, categoria);

                if (!actualizado)
                    return NotFound(new
                    {
                        message = "No se encontró la categoría."
                    });

                var resultDto = _mapper.Map<CategoriaDto>(categoria);

                return Ok(new ApiResponse<CategoriaDto>(resultDto));
            }
            catch (BusinessException)
            {
                throw;
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
                return BadRequest(new
                {
                    message = "El ID debe ser mayor que cero."
                });

            var categoria =
                await _categoriaService.GetCategoriaByIdAsync(id);

            if (categoria == null)
                return NotFound(new
                {
                    message = "No se encontró la categoría."
                });

            try
            {
                await _categoriaService.EliminarCategoriaAsync(id);

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
                    message = "Error al eliminar la categoría.",
                    error = ex.Message
                });
            }
        }
    }
}
