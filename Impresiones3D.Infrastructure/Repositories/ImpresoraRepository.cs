using Impresiones3D.Domain.Entities;
using Impresiones3D.Domain.Enums;
using Impresiones3D.Domain.Interfaces;
using Impresiones3D.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Impresiones3D.Infrastructure.Repositories
{
    public class ImpresoraRepository : BaseRepository<Impresora>, IImpresoraRepository
    {
        public ImpresoraRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Impresora>> GetByEstadoAsync(EstadoImpresora estado)
        {
            return await _dbSet.Where(i => i.Estado == estado).ToListAsync();
        }

        public async Task<IEnumerable<Impresora>> GetDisponiblesPorTecnologiaAsync(TecnologiaImpresora tecnologia)
        {
            return await _dbSet.Where(i => i.Estado == EstadoImpresora.Disponible && i.Tecnologia == tecnologia).ToListAsync();
        }
    }
}