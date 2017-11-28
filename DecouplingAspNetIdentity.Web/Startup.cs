using System.Web.Mvc;
using DecouplingAspNetIdentity.Business;
using DecouplingAspNetIdentity.Infrastructure;
using DecouplingAspNetIdentity.Repositories;
using DecouplingAspNetIdentity.Web.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DecouplingAspNetIdentity.Web.Startup))]
namespace DecouplingAspNetIdentity.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureContainer(app);
            ConfigureAuth(app);
        }
    }
}
