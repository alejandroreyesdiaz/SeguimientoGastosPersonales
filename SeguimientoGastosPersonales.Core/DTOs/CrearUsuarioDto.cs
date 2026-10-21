using System;
using System.Collections.Generic;
using System.Text;

namespace SeguimientoGastosPersonales.Core.DTOs
{
    public class CrearUsuarioDto
    {
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
