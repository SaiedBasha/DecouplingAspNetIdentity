using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Owin;
using Microsoft.AspNet.Identity;
using AutoMapper;
using Autofac;
using Autofac.Integration.Mvc;
using DecouplingAspNetIdentity.Business.Services;
using DecouplingAspNetIdentity.Infrastructure;
using DecouplingAspNetIdentity.Repositories.EF;
using DecouplingAspNetIdentity.Repositories.EF.Repositories;
using DecouplingAspNetIdentity.Web.Models.Identity;

namespace DecouplingAspNetIdentity.Web
{
    /// <summary>
    /// Register types into the Autofac Inversion of Control (IOC) container. Autofac makes it easy to register common 
    /// MVC types like the <see cref="UrlHelper"/> using the <see cref="AutofacWebTypesModule"/>. Feel free to change 
    /// this to another IoC container of your choice but ensure that common MVC types like <see cref="UrlHelper"/> are 
    /// registered. See http://autofac.readthedocs.org/en/latest/integration/aspnet.html.
    /// </summary>
    public partial class Startup
    {
        public static void ConfigureContainer(IAppBuilder app)
        {
            var container = CreateContainer();
            app.UseAutofacMiddleware(container);

            // Register MVC Types 
            app.UseAutofacMvc();
        }

        private static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();

            RegisterServices(builder);
            RegisterMvc(builder, assembly);

            var container = builder.Build();

            SetMvcDependencyResolver(container);

            return container;
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            //builder.RegisterType<ApplicationDbContext>().InstancePerRequest();
            builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>)).InstancePerRequest();
            builder.RegisterGeneric(typeof(BusinessService<,>)).As(typeof(IBusinessService<,>)).InstancePerRequest();
            builder.RegisterType(typeof(EfUnitOfWork)).As(typeof(IUnitOfWork)).InstancePerRequest();
            builder.RegisterType<EfUnitOfWorkFactory>().As<IUnitOfWorkFactory>().InstancePerRequest();

            //register repositories
            builder.RegisterAssemblyTypes(typeof(Repository<,>).Assembly).AsImplementedInterfaces().InstancePerRequest();

            //register business services
            builder.RegisterAssemblyTypes(typeof(BusinessService<,>).Assembly).AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterType<UserStore>().As<IUserStore<ApplicationUser, int>>().InstancePerRequest();
            builder.RegisterType<RoleStore>().InstancePerRequest();
            builder.RegisterType<UserManager<ApplicationUser, int>>().InstancePerRequest();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
                .As<Profile>();

            builder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>()).As<IConfigurationProvider>();

            builder.Register(c => c.Resolve<MapperConfiguration>()
                    .CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }

        private static void RegisterMvc(ContainerBuilder builder, Assembly assembly)
        {
            // Register Common MVC Types
            builder.RegisterModule<AutofacWebTypesModule>();

            // Register MVC Filters
            builder.RegisterFilterProvider();

            // Register MVC Controllers
            builder.RegisterControllers(assembly);
        }

        /// <summary>
        /// Sets the ASP.NET MVC dependency resolver.
        /// </summary>
        /// <param name="container">The container.</param>
        private static void SetMvcDependencyResolver(IContainer container)
        {
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}