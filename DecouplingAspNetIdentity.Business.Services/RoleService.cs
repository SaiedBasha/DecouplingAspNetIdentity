using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DecouplingAspNetIdentity.Infrastructure;
using DecouplingAspNetIdentity.Models;
using DecouplingAspNetIdentity.Repositories;

namespace DecouplingAspNetIdentity.Business.Services
{
    public class RoleService : BusinessService<Role, int>, IRoleService
    {
        public RoleService(IUnitOfWorkFactory unitOfWorkFactory, IRoleRepository repository)
            : base(unitOfWorkFactory, repository)
        {
            Repository = repository;
        }

        public new IRoleRepository Repository { get; set; }

        public Role FindByName(string roleName)
        {
            return Repository.GetAll()
                .FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            return Task.Factory.StartNew(() => FindByName(roleName));

        }

        public Task<Role> FindByNameAsync(CancellationToken cancellationToken, string roleName)
        {
            return Task.Factory.StartNew(() => FindByName(roleName), cancellationToken);
        }
    }
}