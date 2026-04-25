namespace Ambev.DeveloperEvaluation.Application.Common.Interfaces
{
    /// <summary>
    /// Abstração de cache para uso na camada de Application.
    /// A implementação concreta fica na camada de Infrastructure (ORM).
    /// </summary>
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default);
        Task RemoveAsync(string key, CancellationToken ct = default);
    }
}
