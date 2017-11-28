using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using DecouplingAspNetIdentity.Models;

namespace DecouplingAspNetIdentity.Repositories.EF.Configuration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PasswordHash).HasMaxLength(256);
            Property(x => x.SecurityStamp).HasMaxLength(256);
            Property(x => x.PasswordResetToken).HasMaxLength(256);
            Property(x => x.UserName).IsRequired().HasMaxLength(25)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute()
                    {
                        IsUnique = true
                    }));
            Property(x => x.Email).IsRequired().HasMaxLength(256)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute()
                    {
                        IsUnique = true
                    }));
            Property(x => x.FirstName).IsRequired().HasMaxLength(20);
            Property(x => x.SurName).IsRequired().HasMaxLength(20);
            Property(x => x.PhoneNumber).IsRequired().HasMaxLength(20);

            HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .Map(x =>
                {
                    x.ToTable("UserRole");
                    x.MapLeftKey("UserId");
                    x.MapRightKey("RoleId");
                });

            HasMany(x => x.Claims)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId);

            HasMany(x => x.Logins)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}