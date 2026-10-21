using System;
using System.Collections.Generic;
using System.Text;
using SeguimientoGastosPersonales.Core.DTOs;

namespace SeguimientoGastosPersonales.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync();
        Task<UsuarioDto?> GetUsuarioByIdAsync(int id);
        Task<UsuarioDto> CrearUsuarioAsync(CrearUsuarioDto dto);
        Task<bool> ActualizarUsuarioAsync(int id, UsuarioDto dto);
        Task<bool> EliminarUsuarioAsync(int id);
    }
}
