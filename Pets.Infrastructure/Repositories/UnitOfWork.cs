using Pets.Persistence;

namespace Pets.Infrastructure.Repositories
{
    public class UnitOfWork
    {
        private readonly PetDataContext _Pets;

        public UnitOfWork(PetDataContext petContext)
        {
            _Pets = petContext;
        }


        public void BeginTransaction()
        {
            _Pets.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _Pets.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _Pets.Database.RollbackTransaction();
        }

        public void Complete()
        {
            _Pets.SaveChanges();
        }
    }
}
