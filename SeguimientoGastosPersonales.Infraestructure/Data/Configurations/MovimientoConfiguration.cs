using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Infrastructure.Data.Configurations
{
    public class MovimientoConfiguration : IEntityTypeConfiguration<Movimiento>
    {
        public void Configure(EntityTypeBuilder<Movimiento> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");
            builder.ToTable("movimientos");

            builder.HasIndex(e => e.UsuarioId, "usuario_id");
            builder.HasIndex(e => e.CategoriaId, "categoria_id");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            builder.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            builder.Property(e => e.Tipo).IsRequired().HasColumnType("enum('INGRESO','GASTO')").HasColumnName("tipo");
            builder.Property(e => e.Descripcion).HasMaxLength(255).HasColumnName("descripcion");
            builder.Property(e => e.Importe).HasPrecision(12, 2).HasColumnName("importe");
            builder.Property(e => e.Fecha).HasColumnType("date").HasColumnName("fecha");

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movimientos_ibfk_1");

            builder.HasOne(d => d.Categoria)
                .WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movimientos_ibfk_2");
        }
    }
}
