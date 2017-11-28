using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DecouplingAspNetIdentity.Models;

namespace DecouplingAspNetIdentity.Repositories.EF.Repositories
{
    public class ExternalLoginRepository : Repository<ExternalLogin, int>, IExternalLoginRepository
    {
        public ExternalLogin GetByProviderAndKey(string loginProvider, string providerKey)
        {
            return GetAll().Where(
                    e => e.LoginProvider.Equals(loginProvider, StringComparison.OrdinalIgnoreCase)
                         && e.ProviderKey.Equals(providerKey, StringComparison.OrdinalIgnoreCase))
                .OrderBy(i => i.Id).FirstOrDefault();
        }

        public Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string providerKey)
        {
            return Task.FromResult(GetByProviderAndKey(loginProvider, providerKey));
        }

        public Task<ExternalLogin> GetByProviderAndKeyAsync(CancellationToken cancellationToken, string loginProvider,
            string providerKey)
        {
            return Task.FromResult(GetByProviderAndKey(loginProvider, providerKey));
        }
    }
}