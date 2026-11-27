using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeguimientoGastosPersonales.Core.Entities;
using SeguimientoGastosPersonales.Core.Enums;

namespace SeguimientoGastosPersonales.Infraestructure.Data.Configurations
{
    public class SecurityConfiguration : IEntityTypeConfiguration<Security>
    {
        public void Configure(EntityTypeBuilder<Security> builder)
        {
            builder.ToTable("Seguridad");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.User)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Role)
                .HasMaxLength(20)
                .IsRequired()
                .HasConversion(
                    x => x.ToString(),
                    x => (RoleType)Enum.Parse(typeof(RoleType), x)
                );
        }
    }
}