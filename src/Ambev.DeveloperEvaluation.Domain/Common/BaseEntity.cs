using Ambev.DeveloperEvaluation.Common.Validation;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public class BaseEntity : IComparable<BaseEntity>
{
    public int? Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public async Task<ValidationResultDetail> ValidateAsync()
    {
        var method = typeof(Validator)
            .GetMethod(nameof(Validator.ValidateAsync), BindingFlags.Public | BindingFlags.Static)
            ?.MakeGenericMethod(this.GetType());

        if (method == null)
            throw new InvalidOperationException($"Could not find generic ValidateAsync method for type {this.GetType().Name}");

        return await (Task<ValidationResultDetail>)method.Invoke(null, new[] { this })!;
    }

    public int CompareTo(BaseEntity? other)
    {
        if (other == null)
            return 1;

        if (Id == null && other.Id == null)
            return 0;
        if (Id == null)
            return -1;
        if (other.Id == null)
            return 1;

        return Id.Value.CompareTo(other.Id.Value);

    }
    public void UpdateAtDate()
    {
        UpdatedAt = DateTime.UtcNow;
    }
    public void CreatedAtDate()
    {
        CreatedAt = DateTime.UtcNow;
    }
}
