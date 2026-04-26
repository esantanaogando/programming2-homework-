using Impresiones3D.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Impresiones3D.Persistence.Configurations
{
    public class ConfiguracionConfiguration : IEntityTypeConfiguration<Configuracion>
    {
        public void Configure(EntityTypeBuilder<Configuracion> builder)
        {
            builder.ToTable("Configuraciones");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Clave).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Valor).HasMaxLength(500).IsRequired();
            builder.HasIndex(c => c.Clave).IsUnique();
        }
    }
}