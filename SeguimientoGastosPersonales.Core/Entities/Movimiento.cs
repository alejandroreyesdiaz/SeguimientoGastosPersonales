using System;
using System.Collections.Generic;
using System.Text;

namespace SeguimientoGastosPersonales.Core.Entities;

public partial class Movimiento
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int CategoriaId { get; set; }
    public string Tipo { get; set; } = null!;
    public string? Descripcion { get; set; }
    public decimal Importe { get; set; }
    public DateOnly Fecha { get; set; }
    public virtual Categoria Categoria { get; set; } = null!;
    public virtual Usuario Usuario { get; set; } = null!;
}