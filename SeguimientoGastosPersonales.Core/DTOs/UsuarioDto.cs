using System;
using System.Collections.Generic;
using System.Text;

namespace SeguimientoGastosPersonales.Core.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}