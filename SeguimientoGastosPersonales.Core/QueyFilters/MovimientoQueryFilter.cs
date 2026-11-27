using System;
using System.Collections.Generic;
using System.Text;

namespace SeguimientoGastosPersonales.Core.QueryFilters
{
    public class MovimientoQueryFilter
    {
        public int? UsuarioId { get; set; }
        public int? CategoriaId { get; set; }
        public string? Tipo { get; set; }
        public DateOnly? Fecha { get; set; }
        public string? Descripcion { get; set; }
    }
}