using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public class BaseEntity : IComparable<BaseEntity>
{
    public int? Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
    {
        return Validator.ValidateAsync(this);
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
