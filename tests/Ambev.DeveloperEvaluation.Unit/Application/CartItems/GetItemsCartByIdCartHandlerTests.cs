using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Queries;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Handlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.CartItems;

public class GetItemsCartByIdCartHandlerTests
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IMapper _mapper;
    private readonly GetItemsCartByIdCartHandler _handler;

    public GetItemsCartByIdCartHandlerTests()
    {
        _cartItemRepository = Substitute.For<ICartItemRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetItemsCartByIdCartHandler(_cartItemRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid cart id When listing items Then returns item list")]
    public async Task Handle_ValidRequest_ReturnsCartItemList()
    {
        // Given
        var cartId = 1;
        var query = new GetItemsCartByIdCartQuery(cartId);
        var items = new List<CartItem>
        {
            new CartItem(cartId, 1, 1, 0, 10, 10) { Id = 1 },
            new CartItem(cartId, 2, 1, 0, 10, 10) { Id = 2 }
        };
        var itemDtos = new List<CartItemDto>
        {
            new() { Id = 1, CartId = cartId },
            new() { Id = 2, CartId = cartId }
        };

        _cartItemRepository.GetListAllAsync(cartId, Arg.Any<CancellationToken>()).Returns(items);
        _mapper.Map<List<CartItemDto>>(items).Returns(itemDtos);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Should().HaveCount(2);
        result.AsT0.Should().BeEquivalentTo(itemDtos);
    }
}

