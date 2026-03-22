using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pets.Domain.Entities;
namespace Pets.Persistence.EntitiesConfigurations
{
    internal class PetConfigurations : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
          
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(150).IsRequired();
            builder.Property(p => p.Specie).HasMaxLength(300).IsRequired();
            builder.Property(p => p.HumanAge).IsRequired();
        }
    }
}
