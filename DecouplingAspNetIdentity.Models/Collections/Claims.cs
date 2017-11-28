using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DecouplingAspNetIdentity.Models.Collections
{

    /// <summary>
    /// 
    /// </summary>
    public class Claims : CollectionBase<Claim>
    {
        /// <summary>
        /// 
        /// </summary>
        public Claims()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialList"></param>
        public Claims(IList<Claim> initialList) : base(initialList)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialList"></param>
        public Claims(CollectionBase<Claim> initialList) : base(initialList)
        {
        }

        /// <summary>
        /// Validate 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate()
        {
            var errors = new List<ValidationResult>();
            foreach (var claim in this)
            {
                errors.AddRange(claim.Validate());
            }

            return errors;
        }
    }
}
