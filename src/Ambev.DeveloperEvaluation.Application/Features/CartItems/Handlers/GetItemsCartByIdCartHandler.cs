using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Queries;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Features.CartItems.Handlers
{
    public class GetItemsCartByIdCartHandler(ICartItemRepository _repo, IMapper _mapper) : IRequestHandler<GetItemsCartByIdCartQuery, List<CartItemDto>?>
    {
        public async Task<List<CartItemDto>?> Handle(GetItemsCartByIdCartQuery request, CancellationToken ct)
        {
            var producList = await _repo.GetListAllAsync(request.IdCart, ct);
            return _mapper.Map<List<CartItemDto>?>(producList);
        }
    }
}
