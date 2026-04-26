using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Common.Interfaces;
using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Products.Handlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Products.Fakers;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class UpdateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly UpdateProductHandler _handler;

    public UpdateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _cacheService = Substitute.For<ICacheService>();
        _handler = new UpdateProductHandler(_productRepository, _mapper, _cacheService);
    }

    [Fact(DisplayName = "Given valid data When updating product Then returns updated product")]
    public async Task Handle_ValidRequest_ReturnsUpdatedProductDto()
    {
        // Given
        const int productId = 1;
        var productDto = ProdutoFaker.GerarProductDto(id: productId);
        var command = new UpdateProductCommand(productDto);
        var existingProduct = ProdutoFaker.GerarProdutoValido(id: productId);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(existingProduct);
        _mapper.Map(productDto, existingProduct).Returns(existingProduct);
        _mapper.Map<ProductDto>(existingProduct).Returns(productDto);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(productId);
        await _productRepository.Received(1).UpdateAsync(existingProduct, Arg.Any<CancellationToken>());
        await _cacheService.Received(1).RemoveAsync("Products_List", Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given nonexistent id When updating product Then returns not found error")]
    public async Task Handle_NonExistentProduct_ReturnsNotFoundError()
    {
        // Given
        const int productId = 1;
        var productDto = ProdutoFaker.GerarProductDto(id: productId);
        var command = new UpdateProductCommand(productDto);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns((Product?)null);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT1.Should().BeTrue();
        result.AsT1.Detail.Should().Contain(productId.ToString());
    }
}
