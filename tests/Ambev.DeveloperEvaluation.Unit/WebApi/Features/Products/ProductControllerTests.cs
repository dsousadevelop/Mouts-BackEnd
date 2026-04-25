using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Products.Queries;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Unit.Application.Products.Fakers;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Application.Common.Errors;
using OneOf;
using OneOf.Types;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.Products;

/// <summary>
/// Testes unitários para o ProductController.
/// Valida a interação entre a API e a camada de Application via Mediator.
/// </summary>
public class ProductControllerTests
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();
        _controller = new ProductController(_mediator, _mapper);
    }

    [Fact(DisplayName = "Post deve retornar 201 Created quando o comando for bem-sucedido")]
    public async Task Post_ComDadosValidos_DeveRetornarCreated()
    {
        // Arrange
        var request = new CreateProductRequest 
        { 
            Title = "Produto Teste", 
            Price = 10, 
            Description = "Desc", 
            CategoryId = 1,
            Image = "http://image.com" 
        };
        var productDto = ProdutoFaker.GerarProductDto(id: 1);
        var command = new CreateProductCommand(productDto);
        var response = new CreateProductResponse { Id = 1 };

        _mapper.Map<CreateProductCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns((OneOf<ProductDto, ValidationError>)productDto);
        _mapper.Map<CreateProductResponse>(productDto).Returns(response);

        // Act
        var result = await _controller.Post(request, CancellationToken.None);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        var apiResponse = createdResult.Value.Should().BeOfType<ApiResponseWithData<CreateProductResponse>>().Subject;
        apiResponse.Success.Should().BeTrue();
        apiResponse.Data.Id.Should().Be(1);
    }

    [Fact(DisplayName = "Get deve retornar 200 Ok quando o produto existir")]
    public async Task Get_ProdutoExistente_DeveRetornarOk()
    {
        // Arrange
        int productId = 1;
        var productDto = ProdutoFaker.GerarProductDto(id: productId);
        var response = new GetProductResponse { Id = productId };

        _mediator.Send(Arg.Any<GetProductByIdQuery>(), Arg.Any<CancellationToken>()).Returns((OneOf<ProductDto, NotFoundError>)productDto);
        _mapper.Map<GetProductResponse>(productDto).Returns(response);

        // Act
        var result = await _controller.Get(productId, CancellationToken.None);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var apiResponse = okResult.Value.Should().BeOfType<ApiResponseWithData<GetProductResponse>>().Subject;
        apiResponse.Data.Id.Should().Be(productId);
    }

    [Fact(DisplayName = "Get deve retornar 404 NotFound quando o produto não existir")]
    public async Task Get_ProdutoInexistente_DeveRetornarNotFound()
    {
        // Arrange
        int productId = 99;
        _mediator.Send(Arg.Any<GetProductByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(new NotFoundError("Product not found"));

        // Act
        var result = await _controller.Get(productId, CancellationToken.None);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName = "Put deve retornar 200 Ok quando a atualização for bem-sucedida")]
    public async Task Put_ProdutoExistente_DeveRetornarOk()
    {
        // Arrange
        int productId = 1;
        var request = new UpdateProductRequest 
        { 
            Id = productId,
            Title = "Novo Titulo",
            Price = 20,
            Description = "Nova Descrição",
            CategoryId = 1,
            Image = "http://image.com/new"
        };
        var productDto = ProdutoFaker.GerarProductDto(id: productId);
        var command = new UpdateProductCommand(productDto);

        _mapper.Map<UpdateProductCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns((OneOf<ProductDto, NotFoundError, ValidationError>)productDto);
        _mapper.Map<UpdateProductResponse>(productDto).Returns(new UpdateProductResponse { Id = productId });

        // Act
        var result = await _controller.Put(productId, request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Delete deve retornar 200 Ok quando a exclusão for bem-sucedida")]
    public async Task Delete_IdValido_DeveRetornarOk()
    {
        // Arrange
        int productId = 1;
        _mediator.Send(Arg.Any<DeleteProductCommand>(), Arg.Any<CancellationToken>()).Returns((OneOf<Success, NotFoundError>)new Success());

        // Act
        var result = await _controller.Delete(productId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }
}
