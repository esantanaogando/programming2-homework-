using Impresiones3D.Domain.Entities;
using Impresiones3D.Domain.Interfaces;
using Impresiones3D.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Impresiones3D.Infrastructure.Repositories
{
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(AppDbContext context) : base(context) { }

        public async Task<Cliente?> GetByCorreoAsync(string correo)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Correo == correo);
        }

        public async Task<IEnumerable<Cliente>> GetClientesConOrdenesAsync()
        {
            return await _dbSet.Include(c => c.Ordenes).ToListAsync();
        }

        public async Task<Cliente?> GetByIdWithOrdenesAsync(int id)
        {
            return await _dbSet.Include(c => c.Ordenes).FirstOrDefaultAsync(c => c.Id == id);
        }


        // NUEVO: busca por nombre, correo O teléfono (insensible a mayúsculas)
        public async Task<IEnumerable<Cliente>> BuscarAsync(string termino)
        {
            var t = termino.ToLower();
            return await _dbSet
                .Where(c => c.Nombre.ToLower().Contains(t)
                         || c.Correo.ToLower().Contains(t)
                         || c.Telefono.ToLower().Contains(t))
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }
    }
}