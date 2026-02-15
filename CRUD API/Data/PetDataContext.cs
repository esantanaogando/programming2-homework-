using CRUD_API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUD_API.Data
{
    public class PetDataContext : DbContext
    {

        public PetDataContext(DbContextOptions<PetDataContext> options) : base(options)
        { }

        public DbSet<Pet> Pets { get; set; }


    }
}
