using Impresiones3D.Domain.Entities;
using Impresiones3D.Domain.Interfaces;
using Impresiones3D.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Impresiones3D.Infrastructure.Repositories
{
    public class ConfiguracionRepository : BaseRepository<Configuracion>, IConfiguracionRepository
    {
        public ConfiguracionRepository(AppDbContext context) : base(context) { }

        public async Task<Configuracion?> GetByClaveAsync(string clave)
            => await _dbSet.FirstOrDefaultAsync(c => c.Clave == clave);
    }
}