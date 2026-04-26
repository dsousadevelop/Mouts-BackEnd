using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class CartCreatedEvent
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }

        public CartCreatedEvent() { }

        public CartCreatedEvent(int cartId, int userId, string userEmail, decimal totalAmount)
        {
            CartId = cartId;
            UserId = userId;
            UserEmail = userEmail;
            TotalAmount = totalAmount;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
