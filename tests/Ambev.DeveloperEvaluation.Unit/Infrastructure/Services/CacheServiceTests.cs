using Ambev.DeveloperEvaluation.ORM.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using NSubstitute;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure.Services
{
    public class CacheServiceTests
    {
        private readonly IDistributedCache _cacheMock;
        private readonly CacheService _cacheService;

        public CacheServiceTests()
        {
            _cacheMock = Substitute.For<IDistributedCache>();
            _cacheService = new CacheService(_cacheMock);
        }

        [Fact(DisplayName = "GetAsync deve retornar o valor desserializado quando a chave existe")]
        public async Task GetAsync_WhenKeyExists_ReturnsDeserializedValue()
        {
            // Arrange
            var key = "test-key";
            var expectedValue = new TestObject { Id = 1, Name = "Test" };
            var json = JsonSerializer.Serialize(expectedValue);
            var bytes = Encoding.UTF8.GetBytes(json);

            _cacheMock.GetAsync(key, Arg.Any<CancellationToken>()).Returns(Task.FromResult((byte[]?)bytes));

            // Act
            var result = await _cacheService.GetAsync<TestObject>(key);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(expectedValue.Id);
            result.Name.Should().Be(expectedValue.Name);
        }

        [Fact(DisplayName = "GetAsync deve retornar null quando a chave não existe")]
        public async Task GetAsync_WhenKeyDoesNotExist_ReturnsDefault()
        {
            // Arrange
            var key = "non-existent-key";
            _cacheMock.GetAsync(key, Arg.Any<CancellationToken>()).Returns(Task.FromResult((byte[]?)null));

            // Act
            var result = await _cacheService.GetAsync<TestObject>(key);

            // Assert
            result.Should().BeNull();
        }

        [Fact(DisplayName = "SetAsync deve serializar o valor e salvar no cache com a expiração correta")]
        public async Task SetAsync_ShouldSerializeAndSetCache()
        {
            // Arrange
            var key = "test-key";
            var value = new TestObject { Id = 1, Name = "Test" };
            var expiration = TimeSpan.FromMinutes(10);

            // Act
            await _cacheService.SetAsync(key, value, expiration);

            // Assert
            await _cacheMock.Received(1).SetAsync(
                key,
                Arg.Is<byte[]>(b => Encoding.UTF8.GetString(b).Contains("\"id\":1") && Encoding.UTF8.GetString(b).Contains("\"name\":\"Test\"")),
                Arg.Is<DistributedCacheEntryOptions>(o => o.AbsoluteExpirationRelativeToNow == expiration),
                Arg.Any<CancellationToken>()
            );
        }

        [Fact(DisplayName = "SetAsync deve usar expiração padrão de 5 minutos quando não informada")]
        public async Task SetAsync_WhenExpirationIsNull_ShouldUseDefaultExpiration()
        {
            // Arrange
            var key = "test-key";
            var value = new TestObject { Id = 1, Name = "Test" };

            // Act
            await _cacheService.SetAsync(key, value, null);

            // Assert
            await _cacheMock.Received(1).SetAsync(
                key,
                Arg.Any<byte[]>(),
                Arg.Is<DistributedCacheEntryOptions>(o => o.AbsoluteExpirationRelativeToNow == TimeSpan.FromMinutes(5)),
                Arg.Any<CancellationToken>()
            );
        }

        [Fact(DisplayName = "RemoveAsync deve chamar o método Remove do cache")]
        public async Task RemoveAsync_ShouldCallCacheRemove()
        {
            // Arrange
            var key = "test-key";

            // Act
            await _cacheService.RemoveAsync(key);

            // Assert
            await _cacheMock.Received(1).RemoveAsync(key, Arg.Any<CancellationToken>());
        }

        private class TestObject
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }
}
