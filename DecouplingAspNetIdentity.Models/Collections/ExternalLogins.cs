using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DecouplingAspNetIdentity.Models.Collections
{
    public class ExternalLogins : CollectionBase<ExternalLogin>
    {
        public ExternalLogins()
        {
        }

        public ExternalLogins(IList<ExternalLogin> initialList) : base(initialList)
        {
        }

        public ExternalLogins(CollectionBase<ExternalLogin> initialList) : base(initialList)
        {
        }

        public IEnumerable<ValidationResult> Validate()
        {
            var errors = new List<ValidationResult>();
            foreach (var externalLogin in this)
            {
                errors.AddRange(externalLogin.Validate());
            }

            return errors;
        }
    }
}
