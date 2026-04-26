using Impresiones3D.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Impresiones3D.Persistence.Configurations
{
    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.ToTable("Materiales");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.Color)
                .HasMaxLength(30);

            builder.Property(m => m.PrecioPorGramo)
                .HasPrecision(18, 2);

            builder.Property(m => m.Stock)
                .HasPrecision(18, 2);

            builder.Property(m => m.UnidadMedida)
                .HasMaxLength(20)
                .HasDefaultValue("gramos");

            // Relación: un material aparece en muchas órdenes
            builder.HasMany(m => m.Ordenes)
                .WithOne(o => o.Material)
                .HasForeignKey(o => o.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}