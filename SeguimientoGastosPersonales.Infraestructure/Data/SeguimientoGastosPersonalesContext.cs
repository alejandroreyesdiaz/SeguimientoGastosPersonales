using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Infrastructure.Data
{
    public partial class SeguimientoGastosPersonalesContext : DbContext
    {
        public SeguimientoGastosPersonalesContext(
            DbContextOptions<SeguimientoGastosPersonalesContext> options) : base(options) { }

        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<Movimiento> Movimientos { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}