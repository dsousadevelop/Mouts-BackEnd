using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;

namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItems.GetCartItems;

public class GetCartItemsProfile : Profile
{
    public GetCartItemsProfile()
    {
        CreateMap<List<CartItemDto>, GetCartItemsResponse>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src));
        CreateMap<CartItemDto, GetCartItemsItemResponse>();
    }
}

