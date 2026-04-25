namespace Ambev.DeveloperEvaluation.Application.Common
{
    public class PagedResult<T>
    {
        public List<T> Data { get; init; } = new();
        public int TotalItems { get; init; }
        public int CurrentPage { get; init; }
        public int TotalPages { get; init; }
    }
}
