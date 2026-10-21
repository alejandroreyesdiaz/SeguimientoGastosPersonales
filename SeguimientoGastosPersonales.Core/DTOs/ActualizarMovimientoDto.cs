using System;
using System.Collections.Generic;
using System.Text;

namespace SeguimientoGastosPersonales.Core.DTOs
{
    public class ActualizarMovimientoDto
    {
        public int UsuarioId { get; set; }
        public int CategoriaId { get; set; }
        public string Tipo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Importe { get; set; }
        public DateOnly Fecha { get; set; }
    }
}