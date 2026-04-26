namespace Impresiones3D.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IClienteRepository Clientes { get; }
        IMaterialRepository Materiales { get; }
        IImpresoraRepository Impresoras { get; }
        IOrdenRepository Ordenes { get; }
        IConfiguracionRepository Configuraciones { get; }

        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}