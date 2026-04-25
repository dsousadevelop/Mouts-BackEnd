using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Bogus;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure.Repositories;

public class AddressRepositoryTests : RepositoryTestsBase
{
    private readonly AddressRepository _repository;
    private readonly Faker<Address> _addressFaker;

    public AddressRepositoryTests()
    {
        _repository = new AddressRepository(Context);
        _addressFaker = new Faker<Address>()
            .CustomInstantiator(f => new Address(
                f.Random.Int(1, 100),
                f.Address.City(),
                f.Address.StreetName(),
                f.Random.Int(1, 1000),
                f.Address.ZipCode(),
                f.Address.Latitude().ToString(),
                f.Address.Longitude().ToString()
            ));
    }

    [Fact(DisplayName = "Deve criar um endereço com sucesso")]
    public async Task CreateAsync_DeveCriarEndereco()
    {
        // Arrange
        var address = _addressFaker.Generate();

        // Act
        var result = await _repository.CreateAsync(address, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(0);
        var dbAddress = await Context.Address.FindAsync(result.Id);
        dbAddress.Should().NotBeNull();
        dbAddress!.City.Should().Be(address.City);
    }

    [Fact(DisplayName = "Deve buscar um endereço por ID")]
    public async Task GetByIdAsync_DeveRetornarEndereco_QuandoExiste()
    {
        // Arrange
        var address = _addressFaker.Generate();
        await Context.Address.AddAsync(address);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(address.Id!.Value, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(address.Id);
        result.City.Should().Be(address.City);
    }

    [Fact(DisplayName = "Deve retornar null ao buscar por ID inexistente")]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        // Act
        var result = await _repository.GetByIdAsync(999, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact(DisplayName = "Deve excluir um endereço com sucesso")]
    public async Task DeleteAsync_DeveExcluirEndereco_QuandoExiste()
    {
        // Arrange
        var address = _addressFaker.Generate();
        await Context.Address.AddAsync(address);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(address.Id!.Value, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var dbAddress = await Context.Address.FindAsync(address.Id);
        dbAddress.Should().BeNull();
    }

    [Fact(DisplayName = "Deve atualizar um endereço com sucesso")]
    public async Task UpdateAsync_DeveAtualizarEndereco()
    {
        // Arrange
        var address = _addressFaker.Generate();
        await Context.Address.AddAsync(address);
        await Context.SaveChangesAsync();

        // Usando reflexão para alterar City para fins de teste
        var cityProperty = typeof(Address).GetProperty("City", BindingFlags.Public | BindingFlags.Instance);
        cityProperty?.SetValue(address, "Nova Cidade");

        // Act
        var result = await _repository.UpdateAsync(address, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.City.Should().Be("Nova Cidade");
        var dbAddress = await Context.Address.FindAsync(address.Id);
        dbAddress!.City.Should().Be("Nova Cidade");
    }
}
