using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DecouplingAspNetIdentity.Models;

namespace DecouplingAspNetIdentity.Repositories.EF.Configuration
{
    public class ExternalLoginConfiguration : EntityTypeConfiguration<ExternalLogin>
    {
        public ExternalLoginConfiguration()
        {
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.LoginProvider).HasMaxLength(128);
            Property(x => x.ProviderKey).HasMaxLength(128);
            HasRequired(x => x.User)
                .WithMany(x => x.Logins)
                .HasForeignKey(x => x.UserId);
        }
    }
}