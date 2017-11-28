using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DecouplingAspNetIdentity.Infrastructure;

namespace DecouplingAspNetIdentity.Models
{
    public class Claim : DomainEntity<int>
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}
