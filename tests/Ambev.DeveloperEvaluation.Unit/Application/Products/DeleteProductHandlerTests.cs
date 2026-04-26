using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Common.Interfaces;
using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Products.Handlers;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using OneOf.Types;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class DeleteProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly ICacheService _cacheService;
    private readonly DeleteProductHandler _handler;

    public DeleteProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _cacheService = Substitute.For<ICacheService>();
        _handler = new DeleteProductHandler(_productRepository, _cacheService);
    }

    [Fact(DisplayName = "Given valid id When deleting product Then returns success")]
    public async Task Handle_ValidRequest_ReturnsSuccess()
    {
        // Given
        var command = new DeleteProductCommand(1);
        _productRepository.DeleteAsync(1, Arg.Any<CancellationToken>()).Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        await _productRepository.Received(1).DeleteAsync(1, Arg.Any<CancellationToken>());
        await _cacheService.Received(1).RemoveAsync("Products_List", Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given nonexistent id When deleting product Then returns not found error")]
    public async Task Handle_NonExistentProduct_ReturnsNotFoundError()
    {
        // Given
        var command = new DeleteProductCommand(1);
        _productRepository.DeleteAsync(1, Arg.Any<CancellationToken>()).Returns(false);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT1.Should().BeTrue();
        result.AsT1.Detail.Should().Contain("1");
    }
}
