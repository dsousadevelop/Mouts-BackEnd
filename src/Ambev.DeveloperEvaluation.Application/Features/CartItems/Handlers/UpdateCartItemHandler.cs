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
    public class UpdateCartItemHandler(ICartItemRepository _repo, IMapper _mapper) : IRequestHandler<UpdateCartItemCommand, OneOf<CartItemDto, NotFoundError, ValidationError>>
    {
        public async Task<OneOf<CartItemDto, NotFoundError, ValidationError>> Handle(UpdateCartItemCommand request, CancellationToken ct)
        {
            if (!request.entityDto.Id.HasValue)
                return new ValidationError("Cart Item Id is required for update");

            var cartItemExists = await _repo.GetByIdAsync(request.entityDto.Id.Value, ct);
            if (cartItemExists == null)
                return new NotFoundError("Cart item not found");

            _mapper.Map(request.entityDto, cartItemExists);

            // Discount logic
            cartItemExists.CalculateDiscount(request.entityDto.UnitPrice);

            var updated = await _repo.UpdateAsync(cartItemExists, ct);
            return _mapper.Map<CartItemDto>(updated);
        }
    }
}
