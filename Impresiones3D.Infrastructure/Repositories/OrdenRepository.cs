using Impresiones3D.Domain.Entities;
using Impresiones3D.Domain.Enums;
using Impresiones3D.Domain.Interfaces;
using Impresiones3D.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Impresiones3D.Infrastructure.Repositories
{
    public class OrdenRepository : BaseRepository<Orden>, IOrdenRepository
    {
        public OrdenRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Orden>> GetByClienteIdAsync(int clienteId)
        {
            return await _dbSet.Where(o => o.ClienteId == clienteId)
                .Include(o => o.Cliente)
                .Include(o => o.Material)
                .Include(o => o.Impresora)
                .ToListAsync();
        }

        public async Task<IEnumerable<Orden>> GetByEstadoAsync(EstadoOrden estado)
        {
            return await _dbSet.Where(o => o.Estado == estado)
                .Include(o => o.Cliente)
                .Include(o => o.Material)
                .Include(o => o.Impresora)
                .ToListAsync();
        }

        public async Task<IEnumerable<Orden>> GetByDateRangeAsync(DateTime desde, DateTime hasta)
        {
            return await _dbSet
                .Where(o => o.FechaSolicitud >= desde && o.FechaSolicitud <= hasta)
                .Include(o => o.Cliente)
                .Include(o => o.Material)
                .Include(o => o.Impresora)
                .ToListAsync();
        }

        public async Task<bool> ExisteOrdenConMaterialAsync(int materialId)
        {
            return await _dbSet.AnyAsync(o => o.MaterialId == materialId);
        }


        public async Task<IEnumerable<Orden>> BuscarAsync(
            int? clienteId, DateTime? desde, DateTime? hasta,
            EstadoOrden? estado, int? materialId)
        {
            var query = _dbSet.AsQueryable();
            if (clienteId.HasValue) query = query.Where(o => o.ClienteId == clienteId);
            if (estado.HasValue) query = query.Where(o => o.Estado == estado);
            if (materialId.HasValue) query = query.Where(o => o.MaterialId == materialId);
            if (desde.HasValue) query = query.Where(o => o.FechaSolicitud >= desde);
            if (hasta.HasValue) query = query.Where(o => o.FechaSolicitud <= hasta.Value.AddDays(1));

            return await query
                .Include(o => o.Cliente)
                .Include(o => o.Material)
                .Include(o => o.Impresora)
                .OrderByDescending(o => o.FechaSolicitud)
                .ToListAsync();
        }

        // ✅ NUEVOS métodos
        public async Task<IEnumerable<Orden>> GetAllWithIncludesAsync()
        {
            return await _dbSet
                .Include(o => o.Cliente)
                .Include(o => o.Material)
                .Include(o => o.Impresora)
                .ToListAsync();
        }

        public async Task<Orden?> GetByIdWithIncludesAsync(int id)
        {
            return await _dbSet
                .Include(o => o.Cliente)
                .Include(o => o.Material)
                .Include(o => o.Impresora)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}