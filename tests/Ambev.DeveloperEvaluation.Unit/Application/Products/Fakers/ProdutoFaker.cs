using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.Fakers;

/// <summary>
/// Fakers centralizados para geração de dados fictícios de Produto e ProductDto.
/// Usados por todos os testes da camada Application de Produtos.
/// </summary>
public static class ProdutoFaker
{
    private static readonly Faker _faker = new("pt_BR");

    /// <summary>Gera um Product (entidade de domínio) com dados aleatórios válidos.</summary>
    public static Product GerarProdutoValido(int? id = null, int? categoriaId = null)
    {
        var categoria = new Category(categoriaId ?? 1, _faker.Commerce.Department());
        var title = _faker.Commerce.ProductName();
        var description = _faker.Commerce.ProductDescription();

        return new Product(
            title: title[..Math.Min(50, title.Length)],
            price: Math.Round(_faker.Random.Decimal(1, 999), 2),
            description: description[..Math.Min(80, description.Length)],
            categoryId: categoriaId ?? 1,
            image: _faker.Internet.Url(),
            rating_Rate: Math.Round(_faker.Random.Decimal(1, 5), 1),
            rating_Count: (short)_faker.Random.Int(1, 500),
            category: categoria,
            createdAt: DateTime.UtcNow.AddDays(-_faker.Random.Int(1, 365)),
            updatedAt: null
        );
    }

    /// <summary>Gera uma lista de Products válidos.</summary>
    public static List<Product> GerarListaDeProdutos(int quantidade = 10, int? categoriaId = null)
        => Enumerable.Range(1, quantidade)
            .Select(i => GerarProdutoValido(id: i, categoriaId: categoriaId))
            .ToList();

    /// <summary>Gera um ProductDto com dados aleatórios válidos.</summary>
    public static ProductDto GerarProductDto(int? id = null, int? categoriaId = null)
    {
        var title = _faker.Commerce.ProductName();
        var description = _faker.Commerce.ProductDescription();

        return new ProductDto(
            id: id ?? _faker.Random.Int(1, 1000),
            title: title[..Math.Min(50, title.Length)],
            price: Math.Round(_faker.Random.Decimal(1, 999), 2),
            description: description[..Math.Min(80, description.Length)],
            categoryId: categoriaId ?? _faker.Random.Int(1, 10),
            image: _faker.Internet.Url(),
            rating: new RatingDto { Rate = _faker.Random.Decimal(1, 5), Count = _faker.Random.Short(1, 500) }
        );
    }

    /// <summary>Gera uma lista de ProductDtos com dados aleatórios válidos.</summary>
    public static List<ProductDto> GerarListaDeProductDtos(int quantidade = 10, int? categoriaId = null)
        => Enumerable.Range(1, quantidade)
            .Select(i => GerarProductDto(id: i, categoriaId: categoriaId))
            .ToList();
}
