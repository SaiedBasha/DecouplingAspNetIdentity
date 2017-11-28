using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Threading;
using System.Threading.Tasks;
using DecouplingAspNetIdentity.Models;
using DecouplingAspNetIdentity.Repositories.EF.Configuration;

namespace DecouplingAspNetIdentity.Repositories.EF
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name=DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
#if DEBUG
            Database.Log = Console.Write;
            Configuration.LazyLoadingEnabled = true;
#endif
            Database.SetInitializer(new NullDatabaseInitializer<ApplicationDbContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ExternalLogin> Logins { get; set; }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException entityException)
            {
                var errors = entityException.EntityValidationErrors;
                throw;
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return base.SaveChangesAsync();
            }
            catch (DbEntityValidationException entityException)
            {
                var errors = entityException.EntityValidationErrors;
                throw;
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DbEntityValidationException entityException)
            {
                var errors = entityException.EntityValidationErrors;
                throw;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new ExternalLoginConfiguration());
            modelBuilder.Configurations.Add(new ClaimConfiguration());
        }
    }
}