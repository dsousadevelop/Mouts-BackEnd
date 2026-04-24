using Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Features.CartItems.Handlers
{
    public class UpdateCartItemHandler(ICartItemRepository _repo, IMapper _mapper) : IRequestHandler<UpdateCartItemCommand>
    {
        public async Task Handle(UpdateCartItemCommand request, CancellationToken ct)
        {
            var model = _mapper.Map<CartItem>(request.entityDto);
            await _repo.UpdateAsync(model, ct);
        }
    }
}
