using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CartItemTests
{
    [Theory]
    [InlineData(1, 100, 100, 0)] // 1 item, no discount
    [InlineData(3, 100, 300, 0)] // 3 items, no discount
    [InlineData(4, 100, 360, 10)] // 4 items, 10% discount
    [InlineData(9, 100, 810, 10)] // 9 items, 10% discount
    [InlineData(10, 100, 800, 20)] // 10 items, 20% discount
    [InlineData(20, 100, 1600, 20)] // 20 items, 20% discount
    public void CalculateDiscount_ShouldApplyCorrectDiscount(int quantity, decimal price, decimal expectedTotal, decimal expectedDiscountPercentage)
    {
        // Arrange
        var cartItem = new CartItem(1, 1, quantity, 0, 0, 0);

        // Act
        cartItem.CalculateDiscount(price);

        // Assert
        cartItem.Discount.Should().Be(price * quantity * (expectedDiscountPercentage / 100));
        cartItem.Total.Should().Be(expectedTotal);
    }

    [Fact]
    public void CalculateDiscount_ShouldThrowException_WhenQuantityExceeds20()
    {
        // Arrange
        var cartItem = new CartItem(1, 1, 21, 0, 0, 0);

        // Act
        var act = () => cartItem.CalculateDiscount(100);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Cannot sell more than 20 items of the same product.");
    }
}
