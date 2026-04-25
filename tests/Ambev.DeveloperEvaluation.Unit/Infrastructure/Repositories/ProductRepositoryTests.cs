using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Bogus;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using AutoMapper;
using System;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure.Repositories;

public class ProductRepositoryTests : RepositoryTestsBase
{
    private readonly ProductRepository _repository;
    private readonly CacheContext _cacheContext;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _distributedCache;
    private readonly Faker<Product> _productFaker;

    public ProductRepositoryTests()
    {
        _distributedCache = Substitute.For<IDistributedCache>();
        _cacheContext = new CacheContext(_distributedCache);
        _mapper = Substitute.For<IMapper>();
        _repository = new ProductRepository(Context, _cacheContext, _mapper);
        
        var category = new Category(1, "Test Category");

        _productFaker = new Faker<Product>()
            .CustomInstantiator(f => new Product(
                f.Commerce.ProductName(),
                f.Random.Decimal(1, 100),
                f.Commerce.ProductDescription(),
                1,
                f.Internet.Avatar(), // URL da imagem
                4.5m,
                100,
                category,
                DateTime.UtcNow,
                null
            ));
    }

    [Fact(DisplayName = "Deve criar um produto com sucesso")]
    public async Task CreateAsync_DeveCriarProduto()
    {
        // Arrange
        var product = _productFaker.Generate();

        // Act
        var result = await _repository.CreateAsync(product, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(0);
        var dbProduct = await Context.Product.FindAsync(result.Id);
        dbProduct.Should().NotBeNull();
        dbProduct!.Title.Should().Be(product.Title);
    }

    [Fact(DisplayName = "Deve buscar um produto por ID")]
    public async Task GetByIdAsync_DeveRetornarProduto_QuandoExiste()
    {
        // Arrange
        var product = _productFaker.Generate();
        await Context.Product.AddAsync(product);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(product.Id!.Value, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(product.Id);
        result.Title.Should().Be(product.Title);
    }

    [Fact(DisplayName = "Deve retornar null ao buscar por ID inexistente")]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        // Act
        var result = await _repository.GetByIdAsync(999, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact(DisplayName = "Deve excluir um produto com sucesso")]
    public async Task DeleteAsync_DeveExcluirProduto_QuandoExiste()
    {
        // Arrange
        var product = _productFaker.Generate();
        await Context.Product.AddAsync(product);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(product.Id!.Value, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var dbProduct = await Context.Product.FindAsync(product.Id);
        dbProduct.Should().BeNull();
    }

    [Fact(DisplayName = "Deve atualizar um produto com sucesso")]
    public async Task UpdateAsync_DeveAtualizarProduto()
    {
        // Arrange
        var product = _productFaker.Generate();
        await Context.Product.AddAsync(product);
        await Context.SaveChangesAsync();

        // Usando reflexão para alterar a propriedade privada 'Title' para fins de teste
        var titleProperty = typeof(Product).GetProperty("Title", BindingFlags.Public | BindingFlags.Instance);
        titleProperty?.SetValue(product, "Novo Titulo");

        // Act
        var result = await _repository.UpdateAsync(product, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Novo Titulo");
        var dbProduct = await Context.Product.FindAsync(product.Id);
        dbProduct!.Title.Should().Be("Novo Titulo");
    }

    [Fact(DisplayName = "Deve listar todos os produtos do banco quando não há cache")]
    public async Task GetListAllAsync_DeveRetornarProdutosDoBanco_QuandoNaoHaCache()
    {
        // Arrange
        var products = _productFaker.Generate(3);
        await Context.Product.AddRangeAsync(products);
        await Context.SaveChangesAsync();
        
        _distributedCache.GetAsync(Arg.Any<string>()).Returns((byte[])null!);

        // Act
        var result = await _repository.GetListAllAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);
    }
}
