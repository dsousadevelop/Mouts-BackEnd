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

public class UpdateCartItemHandlerTests
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IMapper _mapper;
    private readonly UpdateCartItemHandler _handler;

    public UpdateCartItemHandlerTests()
    {
        _cartItemRepository = Substitute.For<ICartItemRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdateCartItemHandler(_cartItemRepository, _mapper);
    }

    [Fact(DisplayName = "Dado dados válidos, ao atualizar o item do carrinho, retorna sucesso")]
    public async Task Handle_ValidRequest_ReturnsCartItemDto()
    {
        // Given
        const int cartItemId = 1;
        var cartItemDto = new CartItemDto { Id = cartItemId, ProductId = 1, Quantity = 3, UnitPrice = 10 };
        var command = new UpdateCartItemCommand(cartItemDto);
        var existingItem = new CartItem(0, 1, 1, 0, 10, 10) { Id = cartItemId };

        _cartItemRepository.GetByIdAsync(cartItemId, Arg.Any<CancellationToken>()).Returns(existingItem);
        _cartItemRepository.UpdateAsync(existingItem, Arg.Any<CancellationToken>()).Returns(existingItem);
        _mapper.Map<CartItemDto>(existingItem).Returns(cartItemDto);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(cartItemId);
    }

    [Fact(DisplayName = "Dado nenhum ID, ao atualizar o item do carrinho, retorna erro de validação")]
    public async Task Handle_NoId_ReturnsValidationError()
    {
        // Given
        var cartItemDto = new CartItemDto { Id = null };
        var command = new UpdateCartItemCommand(cartItemDto);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT2.Should().BeTrue();
        result.AsT2.Detail.Should().Contain("Id is required");
    }
}
