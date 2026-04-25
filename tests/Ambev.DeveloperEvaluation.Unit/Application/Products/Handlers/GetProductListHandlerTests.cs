using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Common.Interfaces;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Products.Handlers;
using Ambev.DeveloperEvaluation.Application.Features.Products.Queries;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Products.Fakers;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.Handlers;

/// <summary>
/// Testes unitários para o Handler de listagem de produtos.
/// Cobre lógica de cache, filtragem por categoria e paginação.
/// </summary>
public class GetProductListHandlerTests
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;
    private readonly GetProductListHandler _handler;

    public GetProductListHandlerTests()
    {
        _repository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _cache = Substitute.For<ICacheService>();
        _handler = new GetProductListHandler(_repository, _mapper, _cache);
    }

    [Fact(DisplayName = "Deve buscar produtos no repositório e salvar no cache quando o cache estiver vazio")]
    public async Task Handle_SemCache_DeveBuscarNoRepoESalvarNoCache()
    {
        // Arrange
        var query = new GetProductListQuery { Page = 1, Size = 10 };
        var products = ProdutoFaker.GerarListaDeProdutos(5);
        var productDtos = ProdutoFaker.GerarListaDeProductDtos(5);

        _cache.GetAsync<List<ProductDto>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((List<ProductDto>?)null);
        
        _repository.GetListAllAsync(Arg.Any<CancellationToken>())
            .Returns(products);

        _mapper.Map<List<ProductDto>>(products).Returns(productDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.TotalItems.Should().Be(5);
        
        await _repository.Received(1).GetListAllAsync(Arg.Any<CancellationToken>());
        await _cache.Received(1).SetAsync(Arg.Any<string>(), productDtos, ct: Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Deve usar dados do cache e não chamar o repositório quando o cache estiver preenchido")]
    public async Task Handle_ComCache_NaoDeveChamarRepositorio()
    {
        // Arrange
        var query = new GetProductListQuery { Page = 1, Size = 10 };
        var productDtos = ProdutoFaker.GerarListaDeProductDtos(5);

        _cache.GetAsync<List<ProductDto>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(productDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Data.Should().HaveCount(5);
        
        await _repository.DidNotReceive().GetListAllAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Deve filtrar produtos por categoria corretamente")]
    public async Task Handle_ComFiltroCategoria_DeveFiltrarResultados()
    {
        // Arrange
        int categoriaAlvo = 1;
        var query = new GetProductListQuery { Page = 1, Size = 10, CategoryId = categoriaAlvo };
        
        var dtos = new List<ProductDto>
        {
            ProdutoFaker.GerarProductDto(id: 1, categoriaId: 1),
            ProdutoFaker.GerarProductDto(id: 2, categoriaId: 1),
            ProdutoFaker.GerarProductDto(id: 3, categoriaId: 2) // Outra categoria
        };

        _cache.GetAsync<List<ProductDto>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(dtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var pagedResult = result.AsT0;
        pagedResult.TotalItems.Should().Be(2);
        pagedResult.Data.Should().AllSatisfy(p => p.CategoryId.Should().Be(categoriaAlvo));
    }

    [Fact(DisplayName = "Deve paginar corretamente os resultados (segunda página)")]
    public async Task Handle_Paginacao_DeveRetornarItensDaPaginaSolicitada()
    {
        // Arrange
        var query = new GetProductListQuery { Page = 2, Size = 2 };
        var dtos = ProdutoFaker.GerarListaDeProductDtos(5); // IDs 1, 2, 3, 4, 5

        _cache.GetAsync<List<ProductDto>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(dtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var pagedResult = result.AsT0;
        pagedResult.Data.Should().HaveCount(2);
        pagedResult.Data.First().Id.Should().Be(3); // P1: 1,2 | P2: 3,4 | P3: 5
        pagedResult.TotalPages.Should().Be(3);
    }
}
