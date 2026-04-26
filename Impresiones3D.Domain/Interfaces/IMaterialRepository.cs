using Impresiones3D.Domain.Entities;

namespace Impresiones3D.Domain.Interfaces
{
    public interface IMaterialRepository : IBaseRepository<Material>
    {
        Task<Material?> GetByNombreAsync(string nombre);
        Task<IEnumerable<Material>> GetMaterialesConStockBajoAsync(decimal limite);
    }
}