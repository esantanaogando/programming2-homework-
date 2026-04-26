using Impresiones3D.Domain.Entities;
using Impresiones3D.Domain.Enums;

namespace Impresiones3D.Domain.Interfaces
{
    public interface IImpresoraRepository : IBaseRepository<Impresora>
    {
        Task<IEnumerable<Impresora>> GetByEstadoAsync(EstadoImpresora estado);
        Task<IEnumerable<Impresora>> GetDisponiblesPorTecnologiaAsync(TecnologiaImpresora tecnologia);
    }
}