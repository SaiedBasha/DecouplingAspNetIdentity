using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DecouplingAspNetIdentity.Infrastructure;
using DecouplingAspNetIdentity.Models;
using DecouplingAspNetIdentity.Repositories;

namespace DecouplingAspNetIdentity.Business.Services
{
    public class UserService : BusinessService<User, int>, IUserService
    {
        public UserService(IUnitOfWorkFactory unitOfWorkFactory, IUserRepository repository)
            : base(unitOfWorkFactory, repository)
        {
            Repository = repository;
        }

        public new IUserRepository Repository { get; }

        public User GetUserWithRelatedDataById(int id)
        {
            return Repository.GetById(id, true, u => u.Roles, u => u.Claims, u => u.Logins);
        }

        public User FindByUserName(string userName)
        {
            return Repository.GetAll(true, user => user.Roles)
                .FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }

        public Task<User> FindByUserNameAsync(string userName)
        {
            return Task.Factory.StartNew(() => FindByUserName(userName));

        }

        public Task<User> FindByUserNameAsync(CancellationToken cancellationToken, string userName)
        {
            return Task.Factory.StartNew(() => FindByUserName(userName), cancellationToken);
        }
    }
}