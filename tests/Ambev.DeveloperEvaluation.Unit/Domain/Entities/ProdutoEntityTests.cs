using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Testes unitários da entidade de domínio Produto.
/// Valida criação, regras de negócio e comportamento do validador.
/// </summary>
public class ProdutoEntityTests
{
    private readonly Faker _faker = new("pt_BR");

    private Product CriarProdutoValido(
        string? titulo = null,
        decimal? preco = null,
        string? descricao = null,
        int? categoriaId = null)
    {
        var fakerTitle = _faker.Commerce.ProductName();
        var fakerDesc = _faker.Commerce.ProductDescription();
        var categoria = new Category(1, "Eletrônicos");
        return new Product(
            title: titulo ?? fakerTitle[..Math.Min(50, fakerTitle.Length)],
            price: preco ?? _faker.Random.Decimal(1, 999),
            description: descricao ?? fakerDesc[..Math.Min(80, fakerDesc.Length)],
            categoryId: categoriaId ?? 1,
            image: _faker.Internet.Url(),
            rating_Rate: _faker.Random.Decimal(1, 5),
            rating_Count: (short)_faker.Random.Int(1, 1000),
            category: categoria,
            createdAt: DateTime.UtcNow,
            updatedAt: null
        );
    }

    [Fact(DisplayName = "Cria produto válido com todos os campos preenchidos corretamente")]
    public void CriacaoProduto_ComDadosValidos_DevePassarValidacao()
    {
        // Arrange & Act
        var produto = CriarProdutoValido();

        // Assert
        var resultado = produto.Validate();
        resultado.IsValid.Should().BeTrue();
        resultado.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Produto com título vazio deve falhar na validação")]
    public void CriacaoProduto_TituloVazio_DeveFalharValidacao()
    {
        // Arrange
        var produto = CriarProdutoValido(titulo: "");

        // Act
        var resultado = produto.Validate();

        // Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.Description.Contains("cannot be empty") || e.Description.Contains("description"));
    }

    [Fact(DisplayName = "Produto com título acima de 60 caracteres deve falhar na validação")]
    public void CriacaoProduto_TituloMaiorQue60Caracteres_DeveFalharValidacao()
    {
        // Arrange
        var tituloLongo = new string('A', 61);
        var produto = CriarProdutoValido(titulo: tituloLongo);

        // Act
        var resultado = produto.Validate();

        // Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.Description.Contains("longer than"));
    }

    [Fact(DisplayName = "Produto com preço zero deve falhar na validação")]
    public void CriacaoProduto_PrecoZero_DeveFalharValidacao()
    {
        // Arrange
        var produto = CriarProdutoValido(preco: 0);

        // Act
        var resultado = produto.Validate();

        // Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.Description.Contains("greater than zero"));
    }

    [Fact(DisplayName = "Produto com preço negativo deve falhar na validação")]
    public void CriacaoProduto_PrecoNegativo_DeveFalharValidacao()
    {
        // Arrange
        var produto = CriarProdutoValido(preco: -10.50m);

        // Act
        var resultado = produto.Validate();

        // Assert
        resultado.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Produto com categoria zerada deve falhar na validação")]
    public void CriacaoProduto_CategoriaIdZero_DeveFalharValidacao()
    {
        // Arrange
        var produto = CriarProdutoValido(categoriaId: 0);

        // Act
        var resultado = produto.Validate();

        // Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.Description.Contains("zeroed out") || e.Description.Contains("Category"));
    }

    [Fact(DisplayName = "Produto com descrição vazia deve falhar na validação")]
    public void CriacaoProduto_DescricaoVazia_DeveFalharValidacao()
    {
        // Arrange
        var produto = CriarProdutoValido(descricao: "");

        // Act
        var resultado = produto.Validate();

        // Assert
        resultado.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Produto com descrição acima de 100 caracteres deve falhar na validação")]
    public void CriacaoProduto_DescricaoMaiorQue100Caracteres_DeveFalharValidacao()
    {
        // Arrange
        var descricaoLonga = new string('D', 101);
        var produto = CriarProdutoValido(descricao: descricaoLonga);

        // Act
        var resultado = produto.Validate();

        // Assert
        resultado.IsValid.Should().BeFalse();
    }

    [Theory(DisplayName = "Produto com preços válidos deve passar na validação")]
    [InlineData(0.01)]
    [InlineData(1.00)]
    [InlineData(9999.99)]
    public void CriacaoProduto_ComPrecoValido_DevePassarValidacao(decimal preco)
    {
        // Arrange
        var produto = CriarProdutoValido(preco: preco);

        // Act
        var resultado = produto.Validate();

        // Assert
        resultado.IsValid.Should().BeTrue();
    }
}
