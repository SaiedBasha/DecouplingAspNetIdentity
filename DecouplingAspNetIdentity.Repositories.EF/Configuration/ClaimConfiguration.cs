using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DecouplingAspNetIdentity.Models;

namespace DecouplingAspNetIdentity.Repositories.EF.Configuration
{
    public class ClaimConfiguration : EntityTypeConfiguration<Claim>
    {
        public ClaimConfiguration()
        {
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ClaimType).IsMaxLength();
            Property(x => x.ClaimValue).IsMaxLength();
            HasRequired(x => x.User).WithMany(x => x.Claims).HasForeignKey(x => x.UserId);
        }
    }
}
