using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Products.Queries;
using Ambev.DeveloperEvaluation.Application.Features.Products.Handlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Products.Fakers;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class GetProductByIdHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly GetProductByIdHandler _handler;

    public GetProductByIdHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetProductByIdHandler(_productRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid id When product exists Then returns product details")]
    public async Task Handle_ExistingProduct_ReturnsProductDto()
    {
        // Given
        var productId = 1;
        var query = new GetProductByIdQuery(productId);
        var product = ProdutoFaker.GerarProdutoValido(id: productId);
        var productDto = ProdutoFaker.GerarProductDto(id: productId);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<ProductDto>(product).Returns(productDto);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Should().BeEquivalentTo(productDto);
        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given nonexistent id When product does not exist Then returns not found error")]
    public async Task Handle_NonExistentProduct_ReturnsNotFoundError()
    {
        // Given
        var productId = 1;
        var query = new GetProductByIdQuery(productId);
        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns((Product?)null);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.IsT1.Should().BeTrue();
        result.AsT1.Detail.Should().Contain(productId.ToString());
    }
}

