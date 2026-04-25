using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public int UserId { get; private set; }
        public DateTime Date { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; }
        public virtual User? User{ get; private set; }
        public virtual ICollection<CartItem> CartItems { get; private set; } = new List<CartItem>();

        public Cart() { }

        /// <summary>
        /// Entity of Cart
        /// </summary>
        public Cart(int userId, DateTime date)
        {
            UserId = userId;
            Date = date;
            CartItems = new List<CartItem>();
        }

        public void DateSaveCart()
        {
            Date = DateTime.UtcNow;
        }

        public void Cancel()
        {
            IsCancelled = true;
        }

        public void CalculateTotalAmount()
        {
            TotalAmount = CartItems.Sum(i => i.Total);
        }

        public ValidationResultDetail Validate()
        {
            var validator = new CartValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
