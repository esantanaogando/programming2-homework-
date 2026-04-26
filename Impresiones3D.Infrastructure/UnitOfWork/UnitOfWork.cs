using Impresiones3D.Domain.Interfaces;
using Impresiones3D.Infrastructure.Repositories;
using Impresiones3D.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Impresiones3D.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public IClienteRepository Clientes { get; private set; }
        public IMaterialRepository Materiales { get; private set; }
        public IImpresoraRepository Impresoras { get; private set; }
        public IOrdenRepository Ordenes { get; private set; }
        public IConfiguracionRepository Configuraciones { get; private set; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Clientes = new ClienteRepository(_context);
            Materiales = new MaterialRepository(_context);
            Impresoras = new ImpresoraRepository(_context);
            Ordenes = new OrdenRepository(_context);
            Configuraciones = new ConfiguracionRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}