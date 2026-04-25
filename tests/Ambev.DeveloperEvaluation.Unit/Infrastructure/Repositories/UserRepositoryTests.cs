using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Bogus;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure.Repositories;

public class UserRepositoryTests : RepositoryTestsBase
{
    private readonly UserRepository _repository;
    private readonly Faker<User> _userFaker;

    public UserRepositoryTests()
    {
        _repository = new UserRepository(Context);
        _userFaker = new Faker<User>()
            .CustomInstantiator(f => new User(
                f.Internet.UserName(),
                f.Internet.Email(),
                f.Phone.PhoneNumber(),
                f.Internet.Password(),
                f.Name.FirstName(),
                f.Name.LastName(),
                UserRole.Customer,
                UserStatus.Active
            ));
    }

    [Fact(DisplayName = "Deve criar um usuário com sucesso")]
    public async Task CreateAsync_DeveCriarUsuario()
    {
        // Arrange
        var user = _userFaker.Generate();

        // Act
        var result = await _repository.CreateAsync(user, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(0);
        var dbUser = await Context.Users.FindAsync(result.Id);
        dbUser.Should().NotBeNull();
        dbUser!.Email.Should().Be(user.Email);
    }

    [Fact(DisplayName = "Deve buscar um usuário por ID")]
    public async Task GetByIdAsync_DeveRetornarUsuario_QuandoExiste()
    {
        // Arrange
        var user = _userFaker.Generate();
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(user.Id!.Value, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(user.Id);
        result.Email.Should().Be(user.Email);
    }

    [Fact(DisplayName = "Deve retornar null ao buscar por ID inexistente")]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        // Act
        var result = await _repository.GetByIdAsync(999, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact(DisplayName = "Deve buscar usuário por e-mail")]
    public async Task GetByEmailAsync_DeveRetornarUsuario_QuandoExiste()
    {
        // Arrange
        var user = _userFaker.Generate();
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByEmailAsync(user.Email, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be(user.Email);
    }

    [Fact(DisplayName = "Deve excluir um usuário com sucesso")]
    public async Task DeleteAsync_DeveExcluirUsuario_QuandoExiste()
    {
        // Arrange
        var user = _userFaker.Generate();
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(user.Id!.Value, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var dbUser = await Context.Users.FindAsync(user.Id);
        dbUser.Should().BeNull();
    }
}
