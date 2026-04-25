using Ambev.DeveloperEvaluation.Application.Features.Cart.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Cart.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Handlers;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Cart;

public class CreateCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly CreateCartHandler _handler;

    public CreateCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateCartHandler(_cartRepository, _productRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid cart data When creating cart Then returns success")]
    public async Task Handle_ValidRequest_ReturnsCartDto()
    {
        // Given
        var userId = 1;
        var cartDto = new CartDto { UserId = userId };
        var command = new CreateCartCommand(cartDto);
        var cart = new Ambev.DeveloperEvaluation.Domain.Entities.Cart(userId, DateTime.UtcNow);
        var createdCart = new Ambev.DeveloperEvaluation.Domain.Entities.Cart(userId, DateTime.UtcNow);
        createdCart.Id = 1;
        var resultDto = new CartDto { Id = 1, UserId = userId };

        _mapper.Map<Ambev.DeveloperEvaluation.Domain.Entities.Cart>(cartDto).Returns(cart);
        _cartRepository.CreateAsync(cart, Arg.Any<CancellationToken>()).Returns(createdCart);
        _mapper.Map<CartDto>(createdCart).Returns(resultDto);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(1);
    }

    [Fact(DisplayName = "Given nonexistent product When creating cart Then returns validation error")]
    public async Task Handle_NonExistentProduct_ReturnsValidationError()
    {
        // Given
        var userId = 1;
        var productId = 1;
        var cartDto = new CartDto 
        { 
            UserId = userId,
            CartItems = new List<CartItemDto> { new() { ProductId = productId, Quantity = 1 } } 
        };
        var command = new CreateCartCommand(cartDto);
        
        var cartItem = new CartItem(0, productId, 1, 0, 10, 10);
        var cart = new Ambev.DeveloperEvaluation.Domain.Entities.Cart(userId, DateTime.UtcNow);
        cart.CartItems.Add(cartItem);

        _mapper.Map<Ambev.DeveloperEvaluation.Domain.Entities.Cart>(cartDto).Returns(cart);
        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns((Product?)null);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT1.Should().BeTrue();
        result.AsT1.Detail.Should().Contain($"product {productId} not exists");
    }
}

