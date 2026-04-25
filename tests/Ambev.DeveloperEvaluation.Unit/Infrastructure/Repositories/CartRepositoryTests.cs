using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Bogus;
using System;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure.Repositories;

public class CartRepositoryTests : RepositoryTestsBase
{
    private readonly CartRepository _repository;
    private readonly Faker<Cart> _cartFaker;

    public CartRepositoryTests()
    {
        _repository = new CartRepository(Context);
        _cartFaker = new Faker<Cart>()
            .CustomInstantiator(f => new Cart(f.Random.Int(1, 100), DateTime.UtcNow));
    }

    [Fact(DisplayName = "Deve criar um carrinho com sucesso")]
    public async Task CreateAsync_DeveCriarCarrinho()
    {
        // Arrange
        var cart = _cartFaker.Generate();

        // Act
        var result = await _repository.CreateAsync(cart, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(0);
        var dbCart = await Context.Cart.FindAsync(result.Id);
        dbCart.Should().NotBeNull();
        dbCart!.UserId.Should().Be(cart.UserId);
    }

    [Fact(DisplayName = "Deve buscar um carrinho por ID")]
    public async Task GetByIdAsync_DeveRetornarCarrinho_QuandoExiste()
    {
        // Arrange
        var cart = _cartFaker.Generate();
        await Context.Cart.AddAsync(cart);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(cart.Id!.Value, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(cart.Id);
    }

    [Fact(DisplayName = "Deve retornar null ao buscar por ID inexistente")]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        // Act
        var result = await _repository.GetByIdAsync(999, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact(DisplayName = "Deve excluir um carrinho com sucesso")]
    public async Task DeleteAsync_DeveExcluirCarrinho_QuandoExiste()
    {
        // Arrange
        var cart = _cartFaker.Generate();
        await Context.Cart.AddAsync(cart);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(cart.Id!.Value, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var dbCart = await Context.Cart.FindAsync(cart.Id);
        dbCart.Should().BeNull();
    }

    [Fact(DisplayName = "Deve atualizar um carrinho com sucesso")]
    public async Task UpdateAsync_DeveAtualizarCarrinho()
    {
        // Arrange
        var cart = _cartFaker.Generate();
        await Context.Cart.AddAsync(cart);
        await Context.SaveChangesAsync();

        // Usando reflexão para alterar UserId para fins de teste
        var userIdProperty = typeof(Cart).GetProperty("UserId", BindingFlags.Public | BindingFlags.Instance);
        userIdProperty?.SetValue(cart, 999);

        // Act
        var result = await _repository.UpdateAsync(cart, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().Be(999);
        var dbCart = await Context.Cart.FindAsync(cart.Id);
        dbCart!.UserId.Should().Be(999);
    }
}
