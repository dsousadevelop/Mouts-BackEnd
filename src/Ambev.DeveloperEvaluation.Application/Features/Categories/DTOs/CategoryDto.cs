namespace Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs
{
    public class CategoryDto
    {
        public CategoryDto(int? id, string description)
        {
            Id = id;
            Description = description;
        }

        public int? Id { get; set; }
        public string Description { get; set; }
    }
}
