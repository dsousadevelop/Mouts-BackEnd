using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Queries;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.CartItems;
using Ambev.DeveloperEvaluation.WebApi.Features.CartItems.CreateCartItem;
using Ambev.DeveloperEvaluation.WebApi.Features.CartItems.GetCartItems;
using Ambev.DeveloperEvaluation.WebApi.Features.CartItems.UpdateCartItem;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using OneOf;
using OneOf.Types;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.CartItems;

public class CartItemControllerTests
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly CartItemController _controller;
    private readonly Faker _faker;

    public CartItemControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();
        _controller = new CartItemController(_mediator, _mapper);
        _faker = new Faker();
    }

    [Fact(DisplayName = "Post deve retornar 201 Created quando o item é adicionado com sucesso")]
    public async Task Post_ComDadosValidos_DeveRetornarCreated()
    {
        // Arrange
        var request = new CreateCartItemRequest { ProductId = 1, Quantity = 2 };
        var cartItemDto = new CartItemDto { Id = 1, CartId = 1, ProductId = 1, Quantity = 2 };
        var command = new CreateCartItemCommand(cartItemDto);
        var response = new CreateCartItemResponse { Id = 1 };

        _mapper.Map<CreateCartItemCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(OneOf<CartItemDto, ValidationError>.FromT0(cartItemDto)));
        _mapper.Map<CreateCartItemResponse>(cartItemDto).Returns(response);

        // Act
        var actionResult = await _controller.Post(request, CancellationToken.None);

        // Assert
        var createdResult = actionResult.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
    }

    [Fact(DisplayName = "Post deve retornar 400 BadRequest quando ocorre erro de validação no Mediator")]
    public async Task Post_ValidationError_DeveRetornarBadRequest()
    {
        // Arrange
        var request = new CreateCartItemRequest { ProductId = 1, Quantity = 2 };
        var command = new CreateCartItemCommand(new CartItemDto { ProductId = 1, Quantity = 2 });
        var validationError = new ValidationError("Product does not exist");

        _mapper.Map<CreateCartItemCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(OneOf<CartItemDto, ValidationError>.FromT1(validationError)));

        // Act
        var actionResult = await _controller.Post(request, CancellationToken.None);

        // Assert
        var badRequestResult = actionResult.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName = "Get deve retornar 200 Ok quando itens são encontrados")]
    public async Task Get_CartIdValido_DeveRetornarOk()
    {
        // Arrange
        var cartId = 1;
        var items = new List<CartItemDto> { new CartItemDto { Id = 1, CartId = cartId } };
        var response = new GetCartItemsResponse { Items = new List<Ambev.DeveloperEvaluation.WebApi.Features.CartItems.GetCartItems.GetCartItemsItemResponse> { new Ambev.DeveloperEvaluation.WebApi.Features.CartItems.GetCartItems.GetCartItemsItemResponse { Id = 1 } } };

        _mediator.Send(Arg.Any<GetItemsCartByIdCartQuery>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(OneOf<List<CartItemDto>, ValidationError>.FromT0(items)));
        _mapper.Map<GetCartItemsResponse>(items).Returns(response);

        // Act
        var actionResult = await _controller.Get(cartId, CancellationToken.None);

        // Assert
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Delete deve retornar 200 Ok quando removido com sucesso")]
    public async Task Delete_IdExistente_DeveRetornarOk()
    {
        // Arrange
        var itemId = 1;
        _mediator.Send(Arg.Any<DeleteCartItemCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(OneOf<bool, NotFoundError>.FromT0(true)));

        // Act
        var actionResult = await _controller.Delete(itemId, CancellationToken.None);

        // Assert
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }
    [Fact(DisplayName = "Delete deve retornar 404 NotFound quando o item não existe")]
    public async Task Delete_IdInexistente_DeveRetornarNotFound()
    {
        // Arrange
        var itemId = 99;
        _mediator.Send(Arg.Any<DeleteCartItemCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(OneOf<bool, NotFoundError>.FromT1(new NotFoundError("Item not found"))));

        // Act
        var actionResult = await _controller.Delete(itemId, CancellationToken.None);

        // Assert
        var notFoundResult = actionResult.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }
}
