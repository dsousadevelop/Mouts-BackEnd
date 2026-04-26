using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Cart.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Queries;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.ListCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.UpdateCart;
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

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.Cart;

public class CartControllerTests
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly CartController _controller;
    private readonly Faker _faker;

    public CartControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();
        _controller = new CartController(_mediator, _mapper);
        _faker = new Faker();
    }

    [Fact(DisplayName = "Post deve retornar 201 Created quando o carrinho é criado com sucesso")]
    public async Task Post_ComDadosValidos_DeveRetornarCreated()
    {
        // Arrange
        var request = new CreateCartRequest { UserId = 1 };
        var cartDto = new CartDto { Id = 1, UserId = 1 };
        var command = new CreateCartCommand(cartDto);
        var response = new CreateCartResponse { Id = 1 };

        _mapper.Map<CreateCartCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(OneOf<CartDto, ValidationError>.FromT0(cartDto)));
        _mapper.Map<CreateCartResponse>(cartDto).Returns(response);

        // Act
        var actionResult = await _controller.Post(request, CancellationToken.None);

        // Assert
        var createdResult = actionResult.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
    }

    [Fact(DisplayName = "Get deve retornar 200 Ok quando o carrinho existe")]
    public async Task Get_IdExistente_DeveRetornarOk()
    {
        // Arrange
        var cartId = 1;
        var cartDto = new CartDto { Id = cartId, UserId = 1 };
        var response = new GetCartResponse { Id = cartId };

        _mediator.Send(Arg.Any<GetCartByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(OneOf<CartDto, NotFoundError>.FromT0(cartDto)));
        _mapper.Map<GetCartResponse>(cartDto).Returns(response);

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
        var cartId = 1;
        _mediator.Send(Arg.Any<DeleteCartCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(OneOf<bool, NotFoundError>.FromT0(true)));

        // Act
        var actionResult = await _controller.Delete(cartId, CancellationToken.None);

        // Assert
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Get deve retornar 404 NotFound quando o carrinho não existe")]
    public async Task Get_IdInexistente_DeveRetornarNotFound()
    {
        // Arrange
        var cartId = 99;
        _mediator.Send(Arg.Any<GetCartByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(OneOf<CartDto, NotFoundError>.FromT1(new NotFoundError("Cart not found"))));

        // Act
        var actionResult = await _controller.Get(cartId, CancellationToken.None);

        // Assert
        var notFoundResult = actionResult.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName = "Delete deve retornar 404 NotFound quando o carrinho não existe")]
    public async Task Delete_IdInexistente_DeveRetornarNotFound()
    {
        // Arrange
        var cartId = 99;
        _mediator.Send(Arg.Any<DeleteCartCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(OneOf<bool, NotFoundError>.FromT1(new NotFoundError("Cart not found"))));

        // Act
        var actionResult = await _controller.Delete(cartId, CancellationToken.None);

        // Assert
        var notFoundResult = actionResult.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }
}
