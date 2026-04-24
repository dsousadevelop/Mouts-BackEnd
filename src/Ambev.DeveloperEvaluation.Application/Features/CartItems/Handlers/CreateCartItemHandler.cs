using Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Features.CartItems.Handlers
{
    public class CreateCartItemHandler(ICartItemRepository _repo, IMapper _mapper) : IRequestHandler<CreateCartItemCommand, int?>
    {
        public async Task<int?> Handle(CreateCartItemCommand request, CancellationToken ct)
        {
            var model = _mapper.Map<CartItem>(request.entityDto);
            var modelRet = await _repo.CreateAsync(model, ct);
            return modelRet.Id;
        }
    }
}
