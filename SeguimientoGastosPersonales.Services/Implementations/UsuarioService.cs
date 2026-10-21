
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SeguimientoGastosPersonales.Core.DTOs;
using SeguimientoGastosPersonales.Core.Entities;
using SeguimientoGastosPersonales.Core.Interfaces;
using SeguimientoGastosPersonales.Services.Interfaces;
using System.Net.Mail;

namespace SeguimientoGastosPersonales.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            IPasswordHasher<Usuario> passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync()
        {
            var usuarios =
                await _usuarioRepository.GetAllUsuariosAsync();

            return _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
        }

        public async Task<UsuarioDto?> GetUsuarioByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del usuario debe ser mayor que cero.");

            var usuario =
                await _usuarioRepository.GetUsuariosByIdAsync(id);

            if (usuario == null)
                return null;

            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<UsuarioDto> CrearUsuarioAsync(
            CrearUsuarioDto dto)
        {
            ValidarNombre(dto.Nombre);
            ValidarEmail(dto.Email);

            if (string.IsNullOrWhiteSpace(dto.Password) ||
                dto.Password.Length < 8)
            {
                throw new ArgumentException(
                    "La contraseña debe tener al menos 8 caracteres.");
            }

            var usuarios =
                await _usuarioRepository.GetAllUsuariosAsync();

            if (usuarios.Any(u =>
                string.Equals(
                    u.Email,
                    dto.Email.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    "Ya existe un usuario con ese correo electrónico.");
            }

            var usuario = new Usuario
            {
                Nombre = dto.Nombre.Trim(),
                Email = dto.Email.Trim()
            };

            usuario.PasswordHash =
                _passwordHasher.HashPassword(usuario, dto.Password);

            await _usuarioRepository.InsertUsuario(usuario);

            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<bool> ActualizarUsuarioAsync(
            int id, UsuarioDto dto)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del usuario debe ser mayor que cero.");

            if (dto.Id != id)
                throw new ArgumentException(
                    "El ID del DTO no coincide con el ID indicado.");

            ValidarNombre(dto.Nombre);
            ValidarEmail(dto.Email);

            var usuario =
                await _usuarioRepository.GetUsuariosByIdAsync(id);

            if (usuario == null)
                return false;

            var usuarios =
                await _usuarioRepository.GetAllUsuariosAsync();

            if (usuarios.Any(u =>
                u.Id != id &&
                string.Equals(
                    u.Email,
                    dto.Email.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    "Ya existe otro usuario con ese correo electrónico.");
            }

            usuario.Nombre = dto.Nombre.Trim();
            usuario.Email = dto.Email.Trim();

            await _usuarioRepository.UpdateUsuario(usuario);

            return true;
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del usuario debe ser mayor que cero.");

            var usuario =
                await _usuarioRepository.GetUsuariosByIdAsync(id);

            if (usuario == null)
                return false;

            await _usuarioRepository.DeleteUsuario(usuario);

            return true;
        }

        private static void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException(
                    "El nombre es obligatorio.");

            if (nombre.Trim().Length > 100)
                throw new ArgumentException(
                    "El nombre no puede superar los 100 caracteres.");
        }

        private static void ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(
                    "El correo electrónico es obligatorio.");

            email = email.Trim();

            if (email.Length > 150 ||
                !MailAddress.TryCreate(email, out var direccion) ||
                direccion.Address != email)
            {
                throw new ArgumentException(
                    "El correo electrónico no es válido.");
            }
        }
    }
}


