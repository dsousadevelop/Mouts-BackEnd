using Ambev.DeveloperEvaluation.Application.Features.Cart.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Cart.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Handlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Cart;

public class UpdateCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly UpdateCartHandler _handler;

    public UpdateCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdateCartHandler(_cartRepository, _productRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid cart data When updating cart Then returns success")]
    public async Task Handle_ValidRequest_ReturnsCartDto()
    {
        // Given
        var cartId = 1;
        var userId = 1;
        var cartDto = new CartDto { Id = cartId, UserId = userId };
        var command = new UpdateCartCommand(cartDto);
        var existingCart = new Ambev.DeveloperEvaluation.Domain.Entities.Cart(userId, DateTime.UtcNow);
        existingCart.Id = cartId;

        _cartRepository.GetByIdAsync(cartId, Arg.Any<CancellationToken>()).Returns(existingCart);
        _cartRepository.UpdateAsync(existingCart, Arg.Any<CancellationToken>()).Returns(existingCart);
        _mapper.Map<CartDto>(existingCart).Returns(cartDto);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(cartId);
        await _cartRepository.Received(1).UpdateAsync(existingCart, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given nonexistent id When updating cart Then returns not found error")]
    public async Task Handle_NonExistentCart_ReturnsNotFoundError()
    {
        // Given
        var cartId = 1;
        var cartDto = new CartDto { Id = cartId };
        var command = new UpdateCartCommand(cartDto);

        _cartRepository.GetByIdAsync(cartId, Arg.Any<CancellationToken>()).Returns((Ambev.DeveloperEvaluation.Domain.Entities.Cart?)null);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT1.Should().BeTrue();
        result.AsT1.Detail.Should().Contain(cartId.ToString());
    }
}

