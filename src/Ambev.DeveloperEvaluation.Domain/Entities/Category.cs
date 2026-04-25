using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Category: BaseEntity
    {
        public Category() { }
        /// <summary>
        /// Entity of Category
        /// </summary>
        /// <param name="description"></param>
        public Category(int? id,string description)
        {
            Id = id;
            Description = description;
        }

        public string Description { get; private set; }
        
        public void UpdateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty", nameof(description));
                
            Description = description;
        }

        public virtual ICollection<Product> Products { get; private set; }

    }
}
