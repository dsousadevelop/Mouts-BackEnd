using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Handlers;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.CartItems;

public class DeleteCartItemHandlerTests
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly DeleteCartItemHandler _handler;

    public DeleteCartItemHandlerTests()
    {
        _cartItemRepository = Substitute.For<ICartItemRepository>();
        _handler = new DeleteCartItemHandler(_cartItemRepository);
    }

    [Fact(DisplayName = "Given valid id When deleting cart item Then returns true")]
    public async Task Handle_ValidRequest_ReturnsTrue()
    {
        // Given
        var cartItemId = 1;
        var command = new DeleteCartItemCommand(cartItemId);

        _cartItemRepository.DeleteAsync(cartItemId, Arg.Any<CancellationToken>()).Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Should().BeTrue();
        await _cartItemRepository.Received(1).DeleteAsync(cartItemId, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given nonexistent id When deleting cart item Then returns not found error")]
    public async Task Handle_NonExistentCartItem_ReturnsNotFoundError()
    {
        // Given
        var cartItemId = 1;
        var command = new DeleteCartItemCommand(cartItemId);

        _cartItemRepository.DeleteAsync(cartItemId, Arg.Any<CancellationToken>()).Returns(false);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT1.Should().BeTrue();
        result.AsT1.Detail.Should().Contain("not found");
    }
}

