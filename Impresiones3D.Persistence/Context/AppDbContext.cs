using Impresiones3D.Domain.Entities;
using Impresiones3D.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Impresiones3D.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Material> Materiales { get; set; }
        public DbSet<Impresora> Impresoras { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<Configuracion> Configuraciones { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplicar configuraciones separadas
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialConfiguration());
            modelBuilder.ApplyConfiguration(new ImpresoraConfiguration());
            modelBuilder.ApplyConfiguration(new OrdenConfiguration());
            modelBuilder.ApplyConfiguration(new ConfiguracionConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}