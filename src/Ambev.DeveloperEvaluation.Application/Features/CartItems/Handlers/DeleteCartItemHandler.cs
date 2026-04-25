using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using OneOf;

namespace Ambev.DeveloperEvaluation.Application.Features.CartItems.Handlers
{
    public class DeleteCartItemHandler(ICartItemRepository _repo) : IRequestHandler<DeleteCartItemCommand, OneOf<bool, NotFoundError>>
    {
        public async Task<OneOf<bool, NotFoundError>> Handle(DeleteCartItemCommand request, CancellationToken ct)
        {
            var deleted = await _repo.DeleteAsync(request.Id, ct);
            if (!deleted)
                return new NotFoundError("Cart item not found");

            return true;
        }
    }
}
