using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CartTests
{
    [Fact(DisplayName = "CalculateTotalAmount deve somar o total de todos os itens")]
    public void CalculateTotalAmount_ShouldSumItemsTotal()
    {
        // Arrange
        var cart = new Cart(1, DateTime.UtcNow);

        var item1 = new CartItem(1, 1, 5, 0, 0, 0); // 10% discount if quantity >= 4
        item1.CalculateDiscount(100); // 500 - 50 = 450

        var item2 = new CartItem(1, 2, 10, 0, 0, 0); // 20% discount if quantity >= 10
        item2.CalculateDiscount(100); // 1000 - 200 = 800

        cart.CartItems.Add(item1);
        cart.CartItems.Add(item2);

        // Act
        cart.CalculateTotalAmount();

        // Assert
        cart.TotalAmount.Should().Be(1250); // 450 + 800
    }
}
