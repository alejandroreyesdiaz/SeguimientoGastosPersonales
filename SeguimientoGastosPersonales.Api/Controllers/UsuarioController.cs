
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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private readonly CrearUsuarioDtoValidator _crearValidator;
        private readonly ActualizarUsuarioDtoValidator _actualizarValidator;

        public UsuarioController(
            IMapper mapper,
            IUsuarioService usuarioService,
            CrearUsuarioDtoValidator crearValidator,
            ActualizarUsuarioDtoValidator actualizarValidator)
        {
            _mapper = mapper;
            _usuarioService = usuarioService;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<IActionResult> GetAllUsuarios()
        {
            var usuarios = await _usuarioService.GetAllUsuariosAsync();

            var usuarioDto = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);

            var response = new ApiResponse<IEnumerable<UsuarioDto>>(usuarioDto);

            return Ok(response);
        }

        //get
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            var usuario = await _usuarioService.GetUsuarioByIdAsync(id);

            if (usuario == null)
                return NotFound(new
                {
                    message = "No se encontró el usuario."
                });

            var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

            var response = new ApiResponse<UsuarioDto>(usuarioDto);

            return Ok(response);
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
                var usuario = _mapper.Map<Usuario>(usuarioDto);

                // Se asigna la contraseña para que el servicio genere su hash.
                usuario.PasswordHash = usuarioDto.Password;

                await _usuarioService.CrearUsuarioAsync(usuario);

                var resultDto = _mapper.Map<UsuarioDto>(usuario);

                return CreatedAtAction(
                    nameof(GetUsuarioById),
                    new { id = usuario.Id },
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
                var usuario = _mapper.Map<Usuario>(usuarioDto);

                var actualizado =
                    await _usuarioService.ActualizarUsuarioAsync(id, usuario);

                if (!actualizado)
                    return NotFound(new
                    {
                        message = "No se encontró el usuario."
                    });

                var resultDto = _mapper.Map<UsuarioDto>(usuario);

                return Ok(new ApiResponse<UsuarioDto>(resultDto));
            }
            catch (BusinessException)
            {
                throw;
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
                return BadRequest(new
                {
                    message = "El ID debe ser mayor que cero."
                });

            var usuario = await _usuarioService.GetUsuarioByIdAsync(id);

            if (usuario == null)
                return NotFound(new
                {
                    message = "No se encontró el usuario."
                });

            try
            {
                await _usuarioService.EliminarUsuarioAsync(id);

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
                    message = "Error al eliminar el usuario.",
                    error = ex.Message
                });
            }
        }
    }
}
