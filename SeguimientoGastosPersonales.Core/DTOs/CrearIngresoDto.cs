using System;
using System.Collections.Generic;
using System.Text;

namespace SeguimientoGastosPersonales.Core.DTOs
{
    public class CrearIngresoDto
    {
        public int UsuarioId { get; set; }
        public int CategoriaId { get; set; }
        public string? Descripcion { get; set; }
        public decimal Importe { get; set; }
        public DateOnly Fecha { get; set; }
    }
}
