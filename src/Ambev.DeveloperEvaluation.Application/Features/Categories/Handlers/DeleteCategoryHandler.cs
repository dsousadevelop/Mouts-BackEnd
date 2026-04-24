using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Commands;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Ambev.DeveloperEvaluation.Application.Features.Categories.Handlers
{
    public class DeleteCategoryHandler(ICategoryRepository _repo) : IRequestHandler<DeleteCategoryCommand, OneOf<Success, ResourceNotFoundError>>
    {
        public async Task<OneOf<Success, ResourceNotFoundError>> Handle(DeleteCategoryCommand request, CancellationToken ct)
        {
            var existingCategory = await _repo.GetByIdAsync(request.Id, ct);
            if (existingCategory == null)
                return new ResourceNotFoundError($"The Category with ID {request.Id} does not exist in our database");

            await _repo.DeleteAsync(request.Id, ct);
            return new Success();
        }
    }
}
