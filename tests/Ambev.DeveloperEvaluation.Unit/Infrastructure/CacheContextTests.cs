using Ambev.DeveloperEvaluation.ORM;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using NSubstitute;
using System.Text.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure;

public class CacheContextTests
{
    private readonly IDistributedCache _cache;
    private readonly CacheContext _cacheContext;

    public CacheContextTests()
    {
        _cache = Substitute.For<IDistributedCache>();
        _cacheContext = new CacheContext(_cache);
    }

    [Fact]
    public async Task RemoveAsync_ShouldCallDistributedCacheRemoveAsync()
    {
        // Arrange
        const string key = "test-key";

        // Act
        await _cacheContext.RemoveAsync(key);

        // Assert
        await _cache.Received(1).RemoveAsync(key, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAsync_WhenKeyExists_ShouldReturnDeserializedValue()
    {
        // Arrange
        const string key = "test-key";
        var value = new TestModel { Name = "Test" };
        var json = JsonSerializer.Serialize(value);
        var bytes = System.Text.Encoding.UTF8.GetBytes(json);
        _cache.GetAsync(key, Arg.Any<CancellationToken>()).Returns(bytes);

        // Act
        var result = await _cacheContext.GetAsync<TestModel>(key);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(value.Name);
    }

    [Fact]
    public async Task GetAsync_WhenKeyDoesNotExist_ShouldReturnDefault()
    {
        // Arrange
        const string key = "non-existent-key";
        _cache.GetAsync(key, Arg.Any<CancellationToken>()).Returns((byte[]?)null);

        // Act
        var result = await _cacheContext.GetAsync<TestModel>(key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAsync_WhenKeyContainsInvalidJson_ShouldThrowJsonException()
    {
        // Arrange
        const string key = "invalid-json-key";
        const string invalidJson = "{ invalid json }";
        var bytes = System.Text.Encoding.UTF8.GetBytes(invalidJson);
        _cache.GetAsync(key, Arg.Any<CancellationToken>()).Returns(bytes);

        // Act
        var act = () => _cacheContext.GetAsync<TestModel>(key);

        // Assert
        await act.Should().ThrowAsync<JsonException>();
    }

    [Fact]
    public async Task SetAsync_ShouldCallDistributedCacheSetAsyncWithSerializedValue()
    {
        // Arrange
        const string key = "test-key";
        var value = new TestModel { Name = "Test" };

        // Act
        await _cacheContext.SetAsync(key, value);

        // Assert
        await _cache.Received(1).SetAsync(
            key,
            Arg.Any<byte[]>(),
            Arg.Any<DistributedCacheEntryOptions>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SetAsync_WithCustomExpiration_ShouldUseProvidedExpiration()
    {
        // Arrange
        const string key = "test-key";
        var value = new TestModel { Name = "Test" };
        var expiration = TimeSpan.FromHours(1);

        // Act
        await _cacheContext.SetAsync(key, value, expiration);

        // Assert
        await _cache.Received(1).SetAsync(
            key,
            Arg.Any<byte[]>(),
            Arg.Is<DistributedCacheEntryOptions>(opts => opts.AbsoluteExpirationRelativeToNow == expiration),
            Arg.Any<CancellationToken>());
    }

    public class TestModel
    {
        public string Name { get; set; } = string.Empty;
    }
}
