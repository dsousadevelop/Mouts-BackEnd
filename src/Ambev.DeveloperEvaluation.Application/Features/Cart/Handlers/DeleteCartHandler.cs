using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Commands;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using OneOf;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Cart.Handlers
{
    public class DeleteCartHandler(ICartRepository _repo) : IRequestHandler<DeleteCartCommand, OneOf<bool, NotFoundError>>
    {
        public async Task<OneOf<bool, NotFoundError>> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (cart == null)
                return new NotFoundError($"Cart with ID {request.Id} not found");

            await _repo.DeleteAsync(request.Id, cancellationToken);    
            return true;
        }
    }
}

