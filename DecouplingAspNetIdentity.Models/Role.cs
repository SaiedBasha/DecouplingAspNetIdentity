using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DecouplingAspNetIdentity.Infrastructure;
using DecouplingAspNetIdentity.Models.Collections;

namespace DecouplingAspNetIdentity.Models
{
    public class Role : DomainEntity<int>
    {
        public string Name { get; set; }
        public Users Users { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in Users.Validate())
            {
                yield return result;
            }
            yield return null;
        }
    }
}