using AutoMapper;
using DecouplingAspNetIdentity.Models;

namespace DecouplingAspNetIdentity.Web.Models.Identity
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<User, ApplicationUser>()
                .ReverseMap();
            CreateMap<Role, ApplicationRole>()
                .ReverseMap();
        }
    }
}