using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DecouplingAspNetIdentity.Models.Collections
{
    public class Roles : CollectionBase<Role>
    {
        public Roles()
        {
        }

        public Roles(IList<Role> initialList) : base(initialList)
        {
        }

        public Roles(CollectionBase<Role> initialList) : base(initialList)
        {
        }

        public IEnumerable<ValidationResult> Validate()
        {
            var errors = new List<ValidationResult>();
            foreach (var role in this)
            {
                errors.AddRange(role.Validate());
            }

            return errors;
        }
    }
}
