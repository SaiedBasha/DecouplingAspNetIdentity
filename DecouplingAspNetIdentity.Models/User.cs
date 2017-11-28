using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DecouplingAspNetIdentity.Infrastructure;
using DecouplingAspNetIdentity.Models.Collections;

namespace DecouplingAspNetIdentity.Models
{
    public class User : DomainEntity<int>
    {
        public User()
        {
            Roles = new Roles();
            Claims = new Claims();
            Logins = new ExternalLogins();
        }

        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PasswordResetToken { get; set; }
        public string Email { get; set; }
        public int AccessFailedCount { get; set; }
        public Claims Claims { get; set; }
        public Roles Roles { get; set; }
        public ExternalLogins Logins { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public Guid ProviderId { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public bool External { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in Claims.Validate())
            {
                yield return result;
            }

            foreach (var result in Roles.Validate())
            {
                yield return result;
            }

            foreach (var result in Logins.Validate())
            {
                yield return result;
            }

            yield return null;
        }
    }
}