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
    public class CartItem : BaseEntity
    {
        public CartItem() { }

        public CartItem(int cartId, int productId, decimal quantity, decimal? discount, decimal subTotal, decimal total)
        {
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            Discount = discount;
            SubTotal = subTotal;
            Total = total;
            if(Id is null)
            {
                CreatedAtDate();
            }
        }

        /// <summary>
        /// Entity of CartItem
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        public int CartId { get; private set; }
        public int ProductId { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal? Discount { get; private set; }
        public decimal SubTotal { get; private set; }
        public decimal Total { get; private set; }

        public virtual Cart? Cart { get; set; }
        public virtual Product? Product { get; set; }

        public ValidationResultDetail Validate()
        {
            var validator = new CartItemValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
