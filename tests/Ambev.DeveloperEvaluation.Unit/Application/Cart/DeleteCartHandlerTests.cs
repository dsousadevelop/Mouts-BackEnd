using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Handlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Cart;

public class DeleteCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly DeleteCartHandler _handler;

    public DeleteCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _handler = new DeleteCartHandler(_cartRepository);
    }

    [Fact(DisplayName = "Given valid id When deleting cart Then returns true")]
    public async Task Handle_ValidRequest_ReturnsTrue()
    {
        // Given
        const int cartId = 1;
        var command = new DeleteCartCommand(cartId);
        var cart = new Ambev.DeveloperEvaluation.Domain.Entities.Cart(1, DateTime.UtcNow);
        cart.Id = cartId;

        _cartRepository.GetByIdAsync(cartId, Arg.Any<CancellationToken>()).Returns(cart);
        _cartRepository.DeleteAsync(cartId, Arg.Any<CancellationToken>()).Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Should().BeTrue();
        await _cartRepository.Received(1).DeleteAsync(cartId, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given nonexistent id When deleting cart Then returns not found error")]
    public async Task Handle_NonExistentCart_ReturnsNotFoundError()
    {
        // Given
        const int cartId = 1;
        var command = new DeleteCartCommand(cartId);

        _cartRepository.GetByIdAsync(cartId, Arg.Any<CancellationToken>()).Returns((Ambev.DeveloperEvaluation.Domain.Entities.Cart?)null);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT1.Should().BeTrue();
        result.AsT1.Detail.Should().Contain(cartId.ToString());
    }
}
