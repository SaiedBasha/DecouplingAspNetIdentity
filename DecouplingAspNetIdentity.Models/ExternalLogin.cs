using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DecouplingAspNetIdentity.Infrastructure;

namespace DecouplingAspNetIdentity.Models
{
    public class ExternalLogin : DomainEntity<int>
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}