using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.Carts.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.GetCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.ListCarts;

public class ListCartsProfile : Profile
{
    public ListCartsProfile()
    {
        CreateMap<List<CartDto>, ListCartsResponse>()
            .ForMember(dest => dest.Carts, opt => opt.MapFrom(src => src));
    }
}
