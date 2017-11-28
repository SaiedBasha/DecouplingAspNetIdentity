using System.Threading;
using System.Threading.Tasks;
using DecouplingAspNetIdentity.Infrastructure;
using DecouplingAspNetIdentity.Models;

namespace DecouplingAspNetIdentity.Business
{
    public interface IUserService : IBusinessService<User, int>
    {
        User GetUserWithRelatedDataById(int id);
        User FindByUserName(string userName);
        Task<User> FindByUserNameAsync(string userName);
        Task<User> FindByUserNameAsync(CancellationToken cancellationToken, string userName);
    }
}