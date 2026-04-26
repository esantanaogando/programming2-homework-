using Impresiones3D.Domain.Entities;
using Impresiones3D.Domain.Interfaces;
using Impresiones3D.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Impresiones3D.Infrastructure.Repositories
{
    public class MaterialRepository : BaseRepository<Material>, IMaterialRepository
    {
        public MaterialRepository(AppDbContext context) : base(context) { }

        public async Task<Material?> GetByNombreAsync(string nombre)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Nombre == nombre);
        }

        public async Task<IEnumerable<Material>> GetMaterialesConStockBajoAsync(decimal limite)
        {
            return await _dbSet.Where(m => m.Stock <= limite).ToListAsync();
        }
    }
}