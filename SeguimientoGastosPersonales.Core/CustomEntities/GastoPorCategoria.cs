using System;
using System.Collections.Generic;
using System.Text;

namespace SeguimientoGastosPersonales.Core.CustomEntities
{
    public class GastoPorCategoria
    {
        public string Categoria { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int CantidadMovimientos { get; set; }
        public decimal TotalMonto { get; set; }
    }
}
