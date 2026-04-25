using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Queries;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using OneOf;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Features.CartItems.Handlers
{
    public class GetItemsCartByIdCartHandler(ICartItemRepository _repo, IMapper _mapper) : IRequestHandler<GetItemsCartByIdCartQuery, OneOf<List<CartItemDto>, ValidationError>>
    {
        public async Task<OneOf<List<CartItemDto>, ValidationError>> Handle(GetItemsCartByIdCartQuery request, CancellationToken cancellationToken)
        {
            var items = await _repo.GetListAllAsync(request.CartId, cancellationToken);
            return _mapper.Map<List<CartItemDto>>(items);
        }
    }
}
