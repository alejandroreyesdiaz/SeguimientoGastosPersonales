
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SeguimientoGastosPersonales.Core.DTOs;
using SeguimientoGastosPersonales.Services.Interfaces;

namespace SeguimientoGastosPersonales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IValidator<CrearUsuarioDto> _crearValidator;
        private readonly IValidator<UsuarioDto> _actualizarValidator;

        public UsuarioController(
            IUsuarioService usuarioService,
            IValidator<CrearUsuarioDto> crearValidator,
            IValidator<UsuarioDto> actualizarValidator)
        {
            _usuarioService = usuarioService;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<IActionResult> GetAllUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.GetAllUsuariosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al obtener los usuarios.",
                    error = ex.Message
                });
            }
        }

        // GET: api/Usuario/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioByIdAsync(id);

                if (usuario == null)
                {
                    return NotFound(new
                    {
                        message = "No se encontró el usuario."
                    });
                }

                return Ok(usuario);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al obtener el usuario.",
                    error = ex.Message
                });
            }
        }

        // POST: api/Usuario
        [HttpPost]
        public async Task<IActionResult> CrearUsuario(
            [FromBody] CrearUsuarioDto usuarioDto)
        {
            var validationResult =
                await _crearValidator.ValidateAsync(usuarioDto);

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
                var usuario =
                    await _usuarioService.CrearUsuarioAsync(usuarioDto);

                return CreatedAtAction(
                    nameof(GetUsuarioById),
                    new { id = usuario.Id },
                    usuario);
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
                    message = "Error al crear el usuario.",
                    error = ex.Message
                });
            }
        }

        // PUT: api/Usuario/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarUsuario(
            int id,
            [FromBody] UsuarioDto usuarioDto)
        {
            if (id != usuarioDto.Id)
            {
                return BadRequest(new
                {
                    message = "El ID del usuario no coincide con el de la ruta."
                });
            }

            var validationResult =
                await _actualizarValidator.ValidateAsync(usuarioDto);

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
                    await _usuarioService.ActualizarUsuarioAsync(
                        id, usuarioDto);

                if (!actualizado)
                {
                    return NotFound(new
                    {
                        message = "No se encontró el usuario."
                    });
                }

                return Ok(new
                {
                    message = "Usuario actualizado correctamente."
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
                    message = "Error al actualizar el usuario.",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarUsuario(int id)
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
                    await _usuarioService.EliminarUsuarioAsync(id);

                if (!eliminado)
                {
                    return NotFound(new
                    {
                        message = "No se encontró el usuario."
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
                    message = "Error al eliminar el usuario.",
                    error = ex.Message
                });
            }
        }
    }
}
