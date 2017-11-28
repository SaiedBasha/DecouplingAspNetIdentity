using DecouplingAspNetIdentity.Infrastructure;
using DecouplingAspNetIdentity.Models;

namespace DecouplingAspNetIdentity.Repositories
{
    public interface IUserRepository: IRepository<User, int>
    {
    }
}
