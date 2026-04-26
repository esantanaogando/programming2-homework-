using Impresiones3D.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Impresiones3D.Persistence.Configurations
{
    public class OrdenConfiguration : IEntityTypeConfiguration<Orden>
    {
        public void Configure(EntityTypeBuilder<Orden> builder)
        {
            builder.ToTable("Ordenes");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.DescripcionModelo)
                .HasMaxLength(500);

            builder.Property(o => o.CantidadPiezas);

            builder.Property(o => o.AlturaCapa)
                .HasPrecision(18, 2);

            builder.Property(o => o.Relleno);

            builder.Property(o => o.TiempoEstimadoHoras)
                .HasPrecision(18, 2);

            builder.Property(o => o.CostoEstimado)
                .HasPrecision(18, 2);

            builder.Property(o => o.Estado)
                .HasConversion<int>();

            builder.Property(o => o.FechaSolicitud)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(o => o.MotivoCancelacion)
                .HasMaxLength(250);

            // Índices para búsquedas frecuentes
            builder.HasIndex(o => o.Estado);
            builder.HasIndex(o => o.FechaSolicitud);
            builder.HasIndex(o => o.ClienteId);
        }
    }
}