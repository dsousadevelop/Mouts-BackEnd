using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Handlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.CartItems;

public class CreateCartItemHandlerTests
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IMapper _mapper;
    private readonly CreateCartItemHandler _handler;

    public CreateCartItemHandlerTests()
    {
        _cartItemRepository = Substitute.For<ICartItemRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateCartItemHandler(_cartItemRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid cart item data When creating Then returns success")]
    public async Task Handle_ValidRequest_ReturnsCartItemDto()
    {
        // Given
        var cartItemDto = new CartItemDto { ProductId = 1, Quantity = 2, UnitPrice = 10 };
        var command = new CreateCartItemCommand(cartItemDto);
        var cartItem = new CartItem(0, 1, 2, 0, 20, 20);
        var createdCartItem = new CartItem(0, 1, 2, 0, 20, 20) { Id = 1 };
        var resultDto = new CartItemDto { Id = 1, ProductId = 1, Quantity = 2 };

        _mapper.Map<CartItem>(cartItemDto).Returns(cartItem);
        _cartItemRepository.CreateAsync(cartItem, Arg.Any<CancellationToken>()).Returns(createdCartItem);
        _mapper.Map<CartItemDto>(createdCartItem).Returns(resultDto);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(1);
    }
}
