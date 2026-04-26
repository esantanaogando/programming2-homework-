using Impresiones3D.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Impresiones3D.Persistence.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Correo)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(c => c.Correo)
                .IsUnique();

            builder.Property(c => c.Telefono)
                .HasMaxLength(20);

            builder.Property(c => c.Direccion)
                .HasMaxLength(200);

            builder.Property(c => c.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            // Relación: un cliente tiene muchas órdenes
            builder.HasMany(c => c.Ordenes)
                .WithOne(o => o.Cliente)
                .HasForeignKey(o => o.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}