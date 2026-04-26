using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Products.Queries;
using Ambev.DeveloperEvaluation.WebApi.Features.Products;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Common.Errors;
using OneOf;
using OneOf.Types;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Features.Products;

public class ProductControllerTests : ControllerTestsBase
{
    private readonly ProductController _controller;
    private readonly Faker _faker;

    public ProductControllerTests()
    {
        _controller = new ProductController(Mediator, Mapper);
        _faker = new Faker();
    }

    [Fact]
    public async Task Get_ExistingId_ReturnsOk()
    {
        // Arrange
        var id = _faker.Random.Int(1, 100);
        var productDto = new ProductDto { Id = id, Title = _faker.Commerce.ProductName() };
        var response = new GetProductResponse { Id = id, Title = productDto.Title };

        Mediator.Send(Arg.Any<GetProductByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(productDto);

        Mapper.Map<GetProductResponse>(productDto).Returns(response);

        // Act
        var result = await _controller.Get(id, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var apiResponse = okResult?.Value as ApiResponseWithData<GetProductResponse>;
        apiResponse.Should().NotBeNull();
        apiResponse!.Data!.Id.Should().Be(id);
        apiResponse.Success.Should().BeTrue();
    }

    [Fact]
    public async Task Get_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var id = _faker.Random.Int(1, 100);
        var notFoundError = new NotFoundError("Product not found");

        Mediator.Send(Arg.Any<GetProductByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(notFoundError);

        // Act
        var result = await _controller.Get(id, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Post_ValidRequest_ReturnsCreated()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Title = _faker.Commerce.ProductName(),
            Price = _faker.Random.Decimal(1, 100),
            Description = _faker.Commerce.ProductDescription(),
            CategoryId = 1,
            Image = _faker.Internet.Avatar()
        };

        var productDto = new ProductDto { Id = 1, Title = request.Title };
        var command = new CreateProductCommand(productDto);
        var response = new CreateProductResponse { Id = 1, Title = request.Title };

        Mapper.Map<CreateProductCommand>(request).Returns(command);
        Mediator.Send(Arg.Any<CreateProductCommand>(), Arg.Any<CancellationToken>())
            .Returns(productDto);
        Mapper.Map<CreateProductResponse>(productDto).Returns(response);

        // Act
        var result = await _controller.Post(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        var apiResponse = createdResult?.Value as ApiResponseWithData<CreateProductResponse>;
        apiResponse.Should().NotBeNull();
        apiResponse!.Data!.Id.Should().Be(1);
    }

    [Fact]
    public async Task Delete_ExistingId_ReturnsOk()
    {
        // Arrange
        var id = _faker.Random.Int(1, 100);
        Mediator.Send(Arg.Any<DeleteProductCommand>(), Arg.Any<CancellationToken>())
            .Returns(new Success());

        // Act
        var result = await _controller.Delete(id, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task List_ReturnsOk()
    {
        // Arrange
        var products = new List<ProductDto> { new ProductDto { Id = 1, Title = "Test" } };
        var pagedResult = new Ambev.DeveloperEvaluation.Application.Common.PagedResult<ProductDto>
        {
            Data = products,
            CurrentPage = 1,
            TotalPages = 1,
            TotalItems = 1
        };
        var response = new ListProductsResponse { Data = new List<GetProductResponse> { new GetProductResponse { Id = 1, Title = "Test" } } };

        Mediator.Send(Arg.Any<GetProductListQuery>(), Arg.Any<CancellationToken>())
            .Returns(pagedResult);

        Mapper.Map<ListProductsResponse>(Arg.Any<object>()).Returns(response);

        // Act
        var result = await _controller.List(1, 10, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
}
