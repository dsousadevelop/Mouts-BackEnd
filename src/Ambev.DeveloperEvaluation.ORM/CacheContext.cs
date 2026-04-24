using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.ORM
{
    public class CacheContext
    {
        private readonly IDistributedCache _cache;

        public CacheContext(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var data = await _cache.GetStringAsync(key);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return data is null ? default : JsonSerializer.Deserialize<T>(data, options);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
            };

            var json = JsonSerializer.Serialize(value, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                IncludeFields = true
            });


            //var json = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, json, options);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

    }
}
