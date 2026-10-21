using System;
using System.Collections.Generic;
using System.Text;

namespace SeguimientoGastosPersonales.Core.Entities;

public partial class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Tipo { get; set; } = null!;
    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
}