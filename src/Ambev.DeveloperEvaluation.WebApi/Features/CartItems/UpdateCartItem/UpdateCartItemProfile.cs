using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;

namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItems.UpdateCartItem;

public class UpdateCartItemProfile : Profile
{
    public UpdateCartItemProfile()
    {
        CreateMap<UpdateCartItemRequest, CartItemDto>();
        CreateMap<CartItemDto, UpdateCartItemCommand>()
            .ConstructUsing(src => new UpdateCartItemCommand(src));
    }
}

