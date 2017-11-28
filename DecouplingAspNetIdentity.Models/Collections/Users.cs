using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DecouplingAspNetIdentity.Models.Collections
{
    public class Users : CollectionBase<User>
    {
        public Users()
        {
        }

        public Users(IList<User> initialList) : base(initialList)
        {
        }

        public Users(CollectionBase<User> initialList) : base(initialList)
        {
        }

        public IEnumerable<ValidationResult> Validate()
        {
            var errors = new List<ValidationResult>();
            foreach (var user in this)
            {
                errors.AddRange(user.Validate());
            }

            return errors;
        }
    }
}
