using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.Carts.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Carts.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Features.CartItem.CreateCartItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart;

public class CreateCartProfile : Profile
{
    public CreateCartProfile()
    {
        CreateMap<CreateCartRequest, CartDto>();
        CreateMap<CreateCartItemRequest, CartItemDto>();
        CreateMap<CartDto, CreateCartCommand>()
            .ConstructUsing(src => new CreateCartCommand(src));
        CreateMap<int, CreateCartResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));
    }
}
