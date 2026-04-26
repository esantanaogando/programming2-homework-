using Impresiones3D.Domain.Entities;

namespace Impresiones3D.Domain.Interfaces
{
    public interface IClienteRepository : IBaseRepository<Cliente>
    {
        Task<Cliente?> GetByCorreoAsync(string correo);
        Task<IEnumerable<Cliente>> GetClientesConOrdenesAsync();
        Task<IEnumerable<Cliente>> BuscarAsync(string termino);
        Task<Cliente?> GetByIdWithOrdenesAsync(int id);
    }
}