using Impresiones3D.Domain.Entities;

namespace Impresiones3D.Domain.Interfaces
{
    public interface IConfiguracionRepository : IBaseRepository<Configuracion>
    {
        Task<Configuracion?> GetByClaveAsync(string clave);
    }
}