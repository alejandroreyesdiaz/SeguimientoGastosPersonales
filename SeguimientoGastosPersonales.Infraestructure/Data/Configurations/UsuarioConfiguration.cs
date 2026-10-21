using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");
            builder.ToTable("usuarios");
            builder.HasIndex(e => e.Email, "email").IsUnique();

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Nombre).IsRequired().HasMaxLength(100).HasColumnName("nombre");
            builder.Property(e => e.Email).IsRequired().HasMaxLength(150).HasColumnName("email");
            builder.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255).HasColumnName("password_hash");
        }
    }
}
