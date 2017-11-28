using DecouplingAspNetIdentity.Infrastructure;

namespace DecouplingAspNetIdentity.Repositories.EF
{
    public class EfUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        {
            return Create(false);
        }

        public IUnitOfWork Create(bool forceNew)
        {
            return new EfUnitOfWork(forceNew);
        }
    }
}
