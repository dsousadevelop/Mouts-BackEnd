using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Testes unitários da entidade de domínio Categoria.
/// Valida criação, atualização de descrição e regras de validação.
/// </summary>
public class CategoriaEntityTests
{
    private readonly Faker _faker = new("pt_BR");

    [Fact(DisplayName = "Cria categoria válida com ID e descrição preenchidos")]
    public void CriacaoCategoria_ComDadosValidos_DevePassarValidacao()
    {
        // Arrange & Act
        var categoria = new Category(1, "Eletrônicos");

        // Assert
        var resultado = categoria.Validate();
        resultado.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Categoria com descrição vazia deve falhar na validação")]
    public void CriacaoCategoria_DescricaoVazia_DeveFalharValidacao()
    {
        // Arrange
        var categoria = new Category(2, "");

        // Act
        var resultado = categoria.Validate();

        // Assert
        resultado.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Atualiza descrição da categoria com texto válido")]
    public void AtualizaDescricao_ComTextoValido_AlteraDescricao()
    {
        // Arrange
        var categoria = new Category(1, "Antigo");
        var novaDescricao = _faker.Commerce.Department();

        // Act
        categoria.UpdateDescription(novaDescricao);

        // Assert
        categoria.Description.Should().Be(novaDescricao);
    }

    [Fact(DisplayName = "Atualiza descrição da categoria com texto vazio deve lançar exceção")]
    public void AtualizaDescricao_ComTextoVazio_DeveLancarExcecao()
    {
        // Arrange
        var categoria = new Category(1, "Eletrônicos");

        // Act & Assert
        var acao = () => categoria.UpdateDescription("   ");
        acao.Should().Throw<ArgumentException>()
            .WithMessage("*Description cannot be empty*");
    }

    [Theory(DisplayName = "Cria categorias com IDs positivos e descrições variadas")]
    [InlineData(1, "Eletrônicos")]
    [InlineData(10, "Vestuário")]
    [InlineData(999, "Móveis")]
    public void CriacaoCategoria_ComDiversosIdsEDescricoes_DevePassarValidacao(int id, string descricao)
    {
        // Arrange & Act
        var categoria = new Category(id, descricao);

        // Assert
        var resultado = categoria.Validate();
        resultado.IsValid.Should().BeTrue();
        categoria.Description.Should().Be(descricao);
    }
}
