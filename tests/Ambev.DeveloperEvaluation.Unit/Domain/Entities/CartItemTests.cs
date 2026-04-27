using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CartItemTests
{
    [Theory(DisplayName = "CalculateDiscount deve aplicar o desconto correto baseado na quantidade")]
    [InlineData(3, 100, 300, 0)]
    [InlineData(4, 100, 360, 10)]
    [InlineData(9, 100, 810, 10)]
    [InlineData(10, 100, 800, 20)]
    [InlineData(20, 100, 1600, 20)]
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

    [Fact(DisplayName = "CalculateDiscount deve lançar exceção quando a quantidade excede 20")]
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
