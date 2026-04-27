using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Common.Interfaces;
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

public class GetProductListHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly GetProductListHandler _handler;

    public GetProductListHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _cacheService = Substitute.For<ICacheService>();
        _handler = new GetProductListHandler(_productRepository, _mapper, _cacheService);
    }

    [Fact(DisplayName = "Dado dados em cache, ao listar produtos, retorna os dados em cache")]
    public async Task Handle_CachedDataExists_ReturnsCachedData()
    {
        // Given
        var query = new GetProductListQuery { Page = 1, Size = 10 };
        var cachedProducts = new List<ProductDto>
        {
            ProdutoFaker.GerarProductDto(id: 1),
            ProdutoFaker.GerarProductDto(id: 2)
        };

        _cacheService.GetAsync<List<ProductDto>>("Products_List", Arg.Any<CancellationToken>())
            .Returns(cachedProducts);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Data.Should().HaveCount(2);
        result.AsT0.TotalItems.Should().Be(2);
        await _productRepository.DidNotReceive().GetListAllAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Dado que não há cache, ao listar produtos, busca do repositório e armazena no cache")]
    public async Task Handle_NoCache_FetchesFromRepoAndCaches()
    {
        // Given
        var query = new GetProductListQuery { Page = 1, Size = 10 };
        var products = new List<Product>
        {
            ProdutoFaker.GerarProdutoValido(id: 1)
        };
        var productDtos = new List<ProductDto>
        {
            ProdutoFaker.GerarProductDto(id: 1)
        };

        _cacheService.GetAsync<List<ProductDto>>("Products_List", Arg.Any<CancellationToken>())
            .Returns((List<ProductDto>?)null);
        _productRepository.GetListAllAsync(Arg.Any<CancellationToken>()).Returns(products);
        _mapper.Map<List<ProductDto>>(products).Returns(productDtos);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Data.Should().HaveCount(1);
        await _cacheService.Received(1).SetAsync("Products_List", productDtos, cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Dado um CategoryId, ao listar produtos, filtra por categoria")]
    public async Task Handle_WithCategoryId_FiltersByCategory()
    {
        // Given
        const int categoryId = 2;
        var query = new GetProductListQuery { Page = 1, Size = 10, CategoryId = categoryId };
        var cachedProducts = new List<ProductDto>
        {
            ProdutoFaker.GerarProductDto(id: 1, categoryId: 1),
            ProdutoFaker.GerarProductDto(id: 2, categoryId: categoryId)
        };

        _cacheService.GetAsync<List<ProductDto>>("Products_List", Arg.Any<CancellationToken>())
            .Returns(cachedProducts);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Data.Should().HaveCount(1);
        result.AsT0.Data[0].Id.Should().Be(2);
        result.AsT0.TotalItems.Should().Be(1);
    }
}
