﻿using System.Threading;
using System.Threading.Tasks;
using DecouplingAspNetIdentity.Infrastructure;
using DecouplingAspNetIdentity.Models;

namespace DecouplingAspNetIdentity.Business
{
    public interface IExternalLoginService : IBusinessService<ExternalLogin, int>
    {
        ExternalLogin GetByProviderAndKey(string loginProvider, string providerKey);
        Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string providerKey);
        Task<ExternalLogin> GetByProviderAndKeyAsync(CancellationToken cancellationToken, string loginProvider, string providerKey);
    } 
}
