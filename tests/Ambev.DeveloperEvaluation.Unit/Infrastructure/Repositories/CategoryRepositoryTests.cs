using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.ORM.DTOs;
using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Bogus;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure.Repositories;

public class CategoryRepositoryTests : RepositoryTestsBase
{
    private readonly CategoryRepository _repository;
    private readonly CacheContext _cacheContext;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _distributedCache;
    private readonly Faker<Category> _categoryFaker;

    public CategoryRepositoryTests()
    {
        _distributedCache = Substitute.For<IDistributedCache>();
        _cacheContext = new CacheContext(_distributedCache);
        _mapper = Substitute.For<IMapper>();
        _repository = new CategoryRepository(Context, _cacheContext, _mapper);
        _categoryFaker = new Faker<Category>()
            .CustomInstantiator(f => new Category(null, f.Commerce.Categories(1)[0]));
    }

    [Fact(DisplayName = "Deve criar uma categoria com sucesso")]
    public async Task CreateAsync_DeveCriarCategoria()
    {
        // Arrange
        var category = _categoryFaker.Generate();

        // Act
        var result = await _repository.CreateAsync(category, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(0);
        var dbCategory = await Context.Category.FindAsync(result.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Description.Should().Be(category.Description);
    }

    [Fact(DisplayName = "Deve buscar uma categoria por ID")]
    public async Task GetByIdAsync_DeveRetornarCategoria_QuandoExiste()
    {
        // Arrange
        var category = _categoryFaker.Generate();
        await Context.Category.AddAsync(category);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(category.Id!.Value, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(category.Id);
        result.Description.Should().Be(category.Description);
    }

    [Fact(DisplayName = "Deve retornar null ao buscar por ID inexistente")]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        // Act
        var result = await _repository.GetByIdAsync(999, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact(DisplayName = "Deve excluir uma categoria com sucesso")]
    public async Task DeleteAsync_DeveExcluirCategoria_QuandoExiste()
    {
        // Arrange
        var category = _categoryFaker.Generate();
        await Context.Category.AddAsync(category);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(category.Id!.Value, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var dbCategory = await Context.Category.FindAsync(category.Id);
        dbCategory.Should().BeNull();
    }

    [Fact(DisplayName = "Deve atualizar uma categoria com sucesso")]
    public async Task UpdateAsync_DeveAtualizarCategoria()
    {
        // Arrange
        var category = _categoryFaker.Generate();
        await Context.Category.AddAsync(category);
        await Context.SaveChangesAsync();

        category.UpdateDescription("Updated Category Name");

        // Act
        var result = await _repository.UpdateAsync(category, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Description.Should().Be("Updated Category Name");
        var dbCategory = await Context.Category.FindAsync(category.Id);
        dbCategory!.Description.Should().Be("Updated Category Name");
    }

    [Fact(DisplayName = "Deve listar todas as categorias do banco quando não há cache")]
    public async Task GetListAllAsync_DeveRetornarCategoriasDoBanco_QuandoNaoHaCache()
    {
        // Arrange
        var categories = _categoryFaker.Generate(3);
        await Context.Category.AddRangeAsync(categories);
        await Context.SaveChangesAsync();

        _distributedCache.GetAsync(Arg.Any<string>()).Returns((byte[])null!);

        // Act
        var result = await _repository.GetListAllAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);
    }
}
