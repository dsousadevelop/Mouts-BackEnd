using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Bogus;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure.Repositories;

public class CartItemRepositoryTests : RepositoryTestsBase
{
    private readonly CartItemRepository _repository;
    private readonly Faker<CartItem> _cartItemFaker;

    public CartItemRepositoryTests()
    {
        _repository = new CartItemRepository(Context);
        _cartItemFaker = new Faker<CartItem>()
            .CustomInstantiator(f => new CartItem(
                f.Random.Int(1, 100),
                f.Random.Int(1, 100),
                f.Random.Decimal(1, 10),
                0,
                100,
                100
            ));
    }

    [Fact(DisplayName = "Deve criar um item de carrinho com sucesso")]
    public async Task CreateAsync_DeveCriarItemCarrinho()
    {
        // Arrange
        var cartItem = _cartItemFaker.Generate();

        // Act
        var result = await _repository.CreateAsync(cartItem, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(0);
        var dbItem = await Context.CartItem.FindAsync(result.Id);
        dbItem.Should().NotBeNull();
        dbItem!.ProductId.Should().Be(cartItem.ProductId);
    }

    [Fact(DisplayName = "Deve buscar um item de carrinho por ID")]
    public async Task GetByIdAsync_DeveRetornarItemCarrinho_QuandoExiste()
    {
        // Arrange
        var cartItem = _cartItemFaker.Generate();
        await Context.CartItem.AddAsync(cartItem);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(cartItem.Id!.Value, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(cartItem.Id);
    }

    [Fact(DisplayName = "Deve retornar null ao buscar por ID inexistente")]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        // Act
        var result = await _repository.GetByIdAsync(999, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact(DisplayName = "Deve excluir um item de carrinho com sucesso")]
    public async Task DeleteAsync_DeveExcluirItemCarrinho_QuandoExiste()
    {
        // Arrange
        var cartItem = _cartItemFaker.Generate();
        await Context.CartItem.AddAsync(cartItem);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(cartItem.Id!.Value, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var dbItem = await Context.CartItem.FindAsync(cartItem.Id);
        dbItem.Should().BeNull();
    }

    [Fact(DisplayName = "Deve atualizar um item de carrinho com sucesso")]
    public async Task UpdateAsync_DeveAtualizarItemCarrinho()
    {
        // Arrange
        var cartItem = _cartItemFaker.Generate();
        await Context.CartItem.AddAsync(cartItem);
        await Context.SaveChangesAsync();

        // Usando reflexão para alterar Quantity para fins de teste
        var quantityProperty = typeof(CartItem).GetProperty("Quantity", BindingFlags.Public | BindingFlags.Instance);
        quantityProperty?.SetValue(cartItem, 15m);

        // Act
        var result = await _repository.UpdateAsync(cartItem, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Quantity.Should().Be(15m);
        var dbItem = await Context.CartItem.FindAsync(cartItem.Id);
        dbItem!.Quantity.Should().Be(15m);
    }
}
