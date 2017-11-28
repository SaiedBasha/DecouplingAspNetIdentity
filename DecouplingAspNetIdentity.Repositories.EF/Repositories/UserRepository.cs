using DecouplingAspNetIdentity.Models;

namespace DecouplingAspNetIdentity.Repositories.EF.Repositories
{
    public class UserRepository : Repository<User, int>, IUserRepository
    {
    }
}