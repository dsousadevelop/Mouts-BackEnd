using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.GetCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItem.GetCartItems;

public class GetCartItemsProfile : Profile
{
    public GetCartItemsProfile()
    {
        CreateMap<List<CartItemDto>, GetCartItemsResponse>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src));
    }
}
