using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Cart.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Queries;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Handlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Cart;

public class GetCartByIdHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly GetCartByIdHandler _handler;

    public GetCartByIdHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetCartByIdHandler(_cartRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid id When cart exists Then returns cart details")]
    public async Task Handle_ExistingCart_ReturnsCartDto()
    {
        // Given
        const int cartId = 1;
        var query = new GetCartByIdQuery(cartId);
        var cart = new Ambev.DeveloperEvaluation.Domain.Entities.Cart(1, DateTime.UtcNow);
        cart.Id = cartId;
        var cartDto = new CartDto { Id = cartId };

        _cartRepository.GetByIdAsync(cartId, Arg.Any<CancellationToken>()).Returns(cart);
        _mapper.Map<CartDto>(cart).Returns(cartDto);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(cartId);
    }

    [Fact(DisplayName = "Given nonexistent id When cart does not exist Then returns not found error")]
    public async Task Handle_NonExistentCart_ReturnsNotFoundError()
    {
        // Given
        const int cartId = 1;
        var query = new GetCartByIdQuery(cartId);

        _cartRepository.GetByIdAsync(cartId, Arg.Any<CancellationToken>()).Returns((Ambev.DeveloperEvaluation.Domain.Entities.Cart?)null);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.IsT1.Should().BeTrue();
        result.AsT1.Detail.Should().Contain(cartId.ToString());
    }
}
