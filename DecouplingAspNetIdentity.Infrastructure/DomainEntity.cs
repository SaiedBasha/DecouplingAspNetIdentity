using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecouplingAspNetIdentity.Infrastructure
{
    public abstract class DomainEntity<TId> : IValidatableObject where TId :
    struct, IComparable, IFormattable, IConvertible, IComparable<TId>, IEquatable<TId>
    {
        public TId Id { get; set; }

        [NotMapped]
        public EntityObjectState ObjectState { get; set; }

        public bool IsTransient()
        {
            return Id.Equals(default(int));
        }

        public override bool Equals(object obj)
        {
            if (obj is DomainEntity<TId> == false)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var item = (DomainEntity<TId>) obj;

            if (item.IsTransient() || IsTransient())
            {
                return false;
            }

            return item.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                return Id.GetHashCode() ^ 31;
                // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)
            }

            return base.GetHashCode();
        }

        public static bool operator ==(DomainEntity<TId> left, DomainEntity<TId> right)
        {
            return Equals(left, null) ? Equals(right, null) : left.Equals(right);
        }

        public static bool operator !=(DomainEntity<TId> left, DomainEntity<TId> right)
        {
            return !(left == right);
        }

        public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);

        public IEnumerable<ValidationResult> Validate()
        {
            var validationErrors = new List<ValidationResult>();
            var ctx = new ValidationContext(this, null, null);
            Validator.TryValidateObject(this, ctx, validationErrors, true);
            return validationErrors;
        }
    }
}