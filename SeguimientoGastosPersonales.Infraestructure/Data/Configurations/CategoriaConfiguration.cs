using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Infrastructure.Data.Configurations
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");
            builder.ToTable("categorias");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Nombre).IsRequired().HasMaxLength(100).HasColumnName("nombre");
            builder.Property(e => e.Tipo).IsRequired().HasColumnType("enum('INGRESO','GASTO')").HasColumnName("tipo");
        }
    }
}