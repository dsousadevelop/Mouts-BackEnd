using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Products.Queries;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using OneOf;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Common.Errors;
using OneOf.Types;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.Products;

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

    [Fact(DisplayName = "Post deve retornar 201 Created")]
    public async Task Post_ComDadosValidos_DeveRetornarCreated()
    {
        // Arrange
        var request = new CreateProductRequest 
        { 
            Title = "Product 1", 
            Price = 10.5m, 
            Description = "A great product", 
            CategoryId = 1, 
            Image = "image.png" 
        };
        var productDto = new ProductDto { Id = 1, Title = "Product 1" };
        var command = new CreateProductCommand(productDto);

        _mapper.Map<CreateProductCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(OneOf<ProductDto, ValidationError>.FromT0(productDto)));
        _mapper.Map<CreateProductResponse>(productDto).Returns(new CreateProductResponse { Id = 1 });

        // Act
        var result = await _controller.Post(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>().Which.StatusCode.Should().Be(201);
    }

    [Fact(DisplayName = "Delete deve retornar 200 Ok")]
    public async Task Delete_IdValido_DeveRetornarOk()
    {
        // Arrange
        int productId = 1;
        _mediator.Send(Arg.Any<DeleteProductCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(OneOf<Success, NotFoundError>.FromT0(new Success())));

        // Act
        var result = await _controller.Delete(productId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }
}
