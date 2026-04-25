using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using OneOf;

namespace Ambev.DeveloperEvaluation.Application.Features.CartItems.Handlers
{
    public class CreateCartItemHandler(ICartItemRepository _repo, IMapper _mapper) : IRequestHandler<CreateCartItemCommand, OneOf<CartItemDto, ValidationError>>
    {
        public async Task<OneOf<CartItemDto, ValidationError>> Handle(CreateCartItemCommand request, CancellationToken ct)
        {
            var cartItem = _mapper.Map<CartItem>(request.CartItemDto);

            // Discount logic
            cartItem.CalculateDiscount(request.CartItemDto.UnitPrice);

            var created = await _repo.CreateAsync(cartItem, ct);
            return _mapper.Map<CartItemDto>(created);
        }
    }
}
