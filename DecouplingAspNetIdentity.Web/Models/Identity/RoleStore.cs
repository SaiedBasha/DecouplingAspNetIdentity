using System;
using System.Linq;
using System.Threading.Tasks;
using DecouplingAspNetIdentity.Business;
using DecouplingAspNetIdentity.Models;
using Microsoft.AspNet.Identity;

namespace DecouplingAspNetIdentity.Web.Models.Identity
{
    public class RoleStore : IRoleStore<ApplicationRole, int>, IQueryableRoleStore<ApplicationRole, int>, IDisposable
    {
        private readonly IRoleService _roleService;

        public RoleStore(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #region IRoleStore<IdentityRole, int> Members
        public Task CreateAsync(ApplicationRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var r = GetRole(role);

            _roleService.Save(r);
            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(ApplicationRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var r = GetRole(role);

            _roleService.Delete(r);
            return Task.FromResult<object>(null);
        }

        public Task<ApplicationRole> FindByIdAsync(int roleId)
        {
            var role = _roleService.GetById(roleId);
            return Task.FromResult<ApplicationRole>(GetIdentityRole(role));
        }

        public Task<ApplicationRole> FindByNameAsync(string roleName)
        {
            var role = _roleService.FindByName(roleName);
            return Task.FromResult<ApplicationRole>(GetIdentityRole(role));
        }

        public Task UpdateAsync(ApplicationRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            var r = GetRole(role);
            _roleService.Save(r);
            return Task.FromResult<object>(null);
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        }

        #endregion

        #region IQueryableRoleStore<IdentityRole, int> Members
        public IQueryable<ApplicationRole> Roles
        {
            get
            {
                return _roleService
                    .GetAll()
                    .Select(x => GetIdentityRole(x))
                    .AsQueryable();
            }
        }
        #endregion

        #region Private Methods
        private Role GetRole(ApplicationRole identityRole)
        {
            if (identityRole == null)
                return null;
            return new Role
            {
                Id = identityRole.Id,
                Name = identityRole.Name
            };
        }

        private ApplicationRole GetIdentityRole(Role role)
        {
            if (role == null)
                return null;
            return new ApplicationRole
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        #endregion
    }
}