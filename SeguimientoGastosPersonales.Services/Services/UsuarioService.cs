
using Microsoft.AspNetCore.Identity;
using SeguimientoGastosPersonales.Core.Entities;
using SeguimientoGastosPersonales.Core.Exceptions;
using SeguimientoGastosPersonales.Core.Interfaces;
using SeguimientoGastosPersonales.Services.Interfaces;
using System.Net.Mail;

namespace SeguimientoGastosPersonales.Services.Services
{
    public class UsuarioService : IUsuarioService
    {
        public readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuarioService(
            IUnitOfWork unitOfWork,
            IPasswordHasher<Usuario> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
        {
            return await _unitOfWork.UsuarioRepository.GetAll();
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            if (id <= 0)
                throw new BusinessException(
                    "El ID del usuario debe ser mayor que cero.");

            return await _unitOfWork.UsuarioRepository.GetById(id);
        }

        public async Task<Usuario> CrearUsuarioAsync(Usuario usuario)
        {
            ValidarNombre(usuario.Nombre);
            ValidarEmail(usuario.Email);

            if (string.IsNullOrWhiteSpace(usuario.PasswordHash) ||
                usuario.PasswordHash.Length < 8)
            {
                throw new BusinessException(
                    "La contraseña debe tener al menos 8 caracteres.");
            }

            var password = usuario.PasswordHash;

            var usuarios = await _unitOfWork.UsuarioRepository.GetAll();

            if (usuarios.Any(u =>
                string.Equals(
                    u.Email,
                    usuario.Email.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new BusinessException(
                    "Ya existe un usuario con ese correo electrónico.");
            }

            usuario.Nombre = usuario.Nombre.Trim();
            usuario.Email = usuario.Email.Trim();

            usuario.PasswordHash =
                _passwordHasher.HashPassword(usuario, password);

            await _unitOfWork.UsuarioRepository.Add(usuario);
            await _unitOfWork.SaveChangesAsync();

            return usuario;
        }

        public async Task<bool> ActualizarUsuarioAsync(
            int id, Usuario usuario)
        {
            if (id <= 0)
                throw new BusinessException(
                    "El ID del usuario debe ser mayor que cero.");

            if (usuario.Id != id)
                throw new BusinessException(
                    "El ID del usuario no coincide con el ID indicado.");

            ValidarNombre(usuario.Nombre);
            ValidarEmail(usuario.Email);

            var usuarioExistente =
                await _unitOfWork.UsuarioRepository.GetById(id);

            if (usuarioExistente == null)
                return false;

            var usuarios = await _unitOfWork.UsuarioRepository.GetAll();

            if (usuarios.Any(u =>
                u.Id != id &&
                string.Equals(
                    u.Email,
                    usuario.Email.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new BusinessException(
                    "Ya existe otro usuario con ese correo electrónico.");
            }

            usuarioExistente.Nombre = usuario.Nombre.Trim();
            usuarioExistente.Email = usuario.Email.Trim();

            await _unitOfWork.UsuarioRepository.Update(usuarioExistente);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            if (id <= 0)
                throw new BusinessException(
                    "El ID del usuario debe ser mayor que cero.");

            var usuario = await _unitOfWork.UsuarioRepository.GetById(id);

            if (usuario == null)
                return false;

            var movimientos =
                await _unitOfWork.MovimientoRepository.GetAll();

            if (movimientos.Any(m => m.UsuarioId == id))
                throw new BusinessException(
                    "No se puede eliminar el usuario porque tiene movimientos registrados.",
                    System.Net.HttpStatusCode.Conflict);

            await _unitOfWork.UsuarioRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new BusinessException(
                    "El nombre es obligatorio.");

            if (nombre.Trim().Length > 100)
                throw new BusinessException(
                    "El nombre no puede superar los 100 caracteres.");
        }

        private static void ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new BusinessException(
                    "El correo electrónico es obligatorio.");

            email = email.Trim();

            if (email.Length > 150 ||
                !MailAddress.TryCreate(email, out var direccion) ||
                direccion.Address != email)
            {
                throw new BusinessException(
                    "El correo electrónico no es válido.");
            }
        }
    }
}
