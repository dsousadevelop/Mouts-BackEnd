using Ambev.DeveloperEvaluation.Application.Features.Cart.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Cart.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Handlers;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
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
    private readonly IUserRepository _userRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IMapper _mapper;
    private readonly CreateCartHandler _handler;

    public CreateCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _eventPublisher = Substitute.For<IEventPublisher>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateCartHandler(_cartRepository, _productRepository, _userRepository, _eventPublisher, _mapper);
    }

    [Fact(DisplayName = "Dado dados de carrinho válidos, ao criar, retorna sucesso")]
    public async Task Handle_ValidRequest_ReturnsCartDto()
    {
        // Given
        const int userId = 1;
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

    [Fact(DisplayName = "Dado um produto inexistente, ao criar o carrinho, retorna erro de validação")]
    public async Task Handle_NonExistentProduct_ReturnsValidationError()
    {
        // Given
        const int userId = 1;
        const int productId = 1;
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

    [Fact(DisplayName = "Dado dados de carrinho válidos com itens, ao criar, retorna sucesso e calcula os itens")]
    public async Task Handle_ValidRequestWithItems_ReturnsCartDto()
    {
        // Given
        const int userId = 1;
        const int productId = 101;
        const decimal price = 50.0m;
        const int quantity = 2;

        var cartItemDto = new CartItemDto { ProductId = productId, Quantity = quantity };
        var cartDto = new CartDto
        {
            UserId = userId,
            CartItems = new List<CartItemDto> { cartItemDto }
        };
        var command = new CreateCartCommand(cartDto);

        var product = new Product("Test Product", price, "Description", 1, "image.png", 4.5m, 10, new Category(), DateTime.UtcNow, null) { Id = productId };
        var cartItem = new CartItem(0, productId, quantity, price, 0, price * quantity);
        var cart = new Ambev.DeveloperEvaluation.Domain.Entities.Cart(userId, DateTime.UtcNow);
        cart.CartItems.Add(cartItem);

        var createdCart = new Ambev.DeveloperEvaluation.Domain.Entities.Cart(userId, DateTime.UtcNow);
        createdCart.Id = 1;
        createdCart.CartItems.Add(cartItem);

        var resultDto = new CartDto { Id = 1, UserId = userId };

        _mapper.Map<Ambev.DeveloperEvaluation.Domain.Entities.Cart>(cartDto).Returns(cart);
        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
        _cartRepository.CreateAsync(cart, Arg.Any<CancellationToken>()).Returns(createdCart);
        _mapper.Map<CartDto>(createdCart).Returns(resultDto);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
    }
}
