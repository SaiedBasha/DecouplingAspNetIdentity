using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using DecouplingAspNetIdentity.Models;

namespace DecouplingAspNetIdentity.Repositories.EF.Configuration
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasMaxLength(256).HasColumnAnnotation(IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute()
                {
                    IsUnique = true
                }));
            HasMany(x => x.Users)
                .WithMany(x => x.Roles)
                .Map(x =>
                {
                    x.ToTable("UserRole");
                    x.MapLeftKey("RoleId");
                    x.MapRightKey("UserId");
                });
        }
    }
}