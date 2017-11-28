using System.Threading;
using System.Threading.Tasks;
using DecouplingAspNetIdentity.Infrastructure;
using DecouplingAspNetIdentity.Models;
using DecouplingAspNetIdentity.Repositories;

namespace DecouplingAspNetIdentity.Business.Services
{
    public class ExternalLoginService : BusinessService<ExternalLogin, int>, IExternalLoginService
    {
        public ExternalLoginService(IUnitOfWorkFactory unitOfWorkFactory, IExternalLoginRepository repository)
            : base(unitOfWorkFactory, repository)
        {
            Repository = repository;
        }

        public new IExternalLoginRepository Repository { get; }

        public ExternalLogin GetByProviderAndKey(string loginProvider, string providerKey)
        {
            return Repository.GetByProviderAndKey(loginProvider, providerKey);
        }

        public Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string providerKey)
        {
            return Repository.GetByProviderAndKeyAsync(loginProvider, providerKey);
        }

        public Task<ExternalLogin> GetByProviderAndKeyAsync(CancellationToken cancellationToken, string loginProvider, string providerKey)
        {
            return Repository.GetByProviderAndKeyAsync(cancellationToken, loginProvider, providerKey);
        }
    }
}
