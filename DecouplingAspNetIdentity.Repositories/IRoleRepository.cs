using DecouplingAspNetIdentity.Infrastructure;
using DecouplingAspNetIdentity.Models;

namespace DecouplingAspNetIdentity.Repositories
{
    public interface IRoleRepository : IRepository<Role, int>
    {
    }
}
