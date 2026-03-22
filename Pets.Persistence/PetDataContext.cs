using Microsoft.EntityFrameworkCore;
using Pets.Domain.Entities;
using Pets.Persistence.EntitiesConfigurations;

namespace Pets.Persistence
{
    public class PetDataContext : DbContext
    {

        public PetDataContext(DbContextOptions<PetDataContext> options) : base(options)
        { }

        public DbSet<Pet> Pets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetConfigurations).Assembly);
        }
    }
}
