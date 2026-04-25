using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using OneOf;
using OneOf.Types;

using Ambev.DeveloperEvaluation.Application.Common.Interfaces;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Handlers
{
    public class DeleteProductHandler(IProductRepository _repo, ICacheService _cache) : IRequestHandler<DeleteProductCommand, OneOf<Success, NotFoundError>>
    {
        public async Task<OneOf<Success, NotFoundError>> Handle(DeleteProductCommand request, CancellationToken ct)
        {
            var success = await _repo.DeleteAsync(request.Id, ct);
            if (!success)
                return new NotFoundError($"Product with ID {request.Id} not found");

            await _cache.RemoveAsync("Products_List", ct);

            return new Success();
        }
    }
}
