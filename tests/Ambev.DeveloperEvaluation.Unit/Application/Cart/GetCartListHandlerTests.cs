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

public class GetCartListHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly GetCartListHandler _handler;

    public GetCartListHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetCartListHandler(_cartRepository, _mapper);
    }

    [Fact(DisplayName = "When listing carts Then returns all carts")]
    public async Task Handle_ValidRequest_ReturnsCartList()
    {
        // Given
        var query = new GetCartListQuery();
        var carts = new List<Ambev.DeveloperEvaluation.Domain.Entities.Cart>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        };
        var cartDtos = new List<CartDto>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        };

        _cartRepository.GetListAllAsync(Arg.Any<CancellationToken>()).Returns(carts);
        _mapper.Map<List<CartDto>>(carts).Returns(cartDtos);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Should().HaveCount(2);
        result.AsT0.Should().BeEquivalentTo(cartDtos);
    }
}

