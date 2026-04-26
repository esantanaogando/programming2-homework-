using Impresiones3D.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Impresiones3D.Persistence.Configurations
{
    public class ImpresoraConfiguration : IEntityTypeConfiguration<Impresora>
    {
        public void Configure(EntityTypeBuilder<Impresora> builder)
        {
            builder.ToTable("Impresoras");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.Modelo)
                .HasMaxLength(100);

            builder.Property(i => i.Tecnologia)
                .HasConversion<int>();

            builder.Property(i => i.Estado)
                .HasConversion<int>();

            builder.Property(i => i.VelocidadMaxima);

            builder.Property(i => i.VolumenX).HasPrecision(10, 2);
            builder.Property(i => i.VolumenY).HasPrecision(10, 2);
            builder.Property(i => i.VolumenZ).HasPrecision(10, 2);

            // NUEVO: horas acumuladas de uso
            builder.Property(i => i.HorasTotalesUso)
                .HasPrecision(10, 2)
                .HasDefaultValue(0);

            builder.HasMany(i => i.Ordenes)
                .WithOne(o => o.Impresora)
                .HasForeignKey(o => o.ImpresoraId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}