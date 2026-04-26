using Impresiones3D.Domain.Entities;
using Impresiones3D.Domain.Enums;

namespace Impresiones3D.Domain.Interfaces
{
    public interface IOrdenRepository : IBaseRepository<Orden>
    {
        Task<IEnumerable<Orden>> GetByClienteIdAsync(int clienteId);
        Task<IEnumerable<Orden>> GetByEstadoAsync(EstadoOrden estado);
        Task<IEnumerable<Orden>> GetByDateRangeAsync(DateTime desde, DateTime hasta);
        Task<IEnumerable<Orden>> BuscarAsync(int? clienteId, DateTime? desde, DateTime? hasta, EstadoOrden? estado, int? materialId);
        Task<bool> ExisteOrdenConMaterialAsync(int materialId);

        // NUEVOS métodos con includes
        Task<IEnumerable<Orden>> GetAllWithIncludesAsync();
        Task<Orden?> GetByIdWithIncludesAsync(int id);
    }
}