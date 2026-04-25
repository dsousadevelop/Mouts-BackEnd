using Ambev.DeveloperEvaluation.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.ORM.Services
{
    /// <summary>
    /// Implementação de ICacheService usando IDistributedCache.
    /// Mantém a dependência de infraestrutura fora da camada de Application.
    /// </summary>
    public class CacheService(IDistributedCache _cache) : ICacheService
    {
        private static readonly JsonSerializerOptions _readOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static readonly JsonSerializerOptions _writeOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            IncludeFields = true
        };

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var data = await _cache.GetStringAsync(key, cancellationToken);
            return data is null ? default : JsonSerializer.Deserialize<T>(data, _readOptions);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
            };

            var json = JsonSerializer.Serialize(value, _writeOptions);
            await _cache.SetStringAsync(key, json, options, cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(key, cancellationToken);
        }
    }
}
