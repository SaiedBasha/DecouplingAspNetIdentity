using Microsoft.AspNet.Identity;

namespace DecouplingAspNetIdentity.Web.Models.Identity
{
    public class ApplicationRole : IRole<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}