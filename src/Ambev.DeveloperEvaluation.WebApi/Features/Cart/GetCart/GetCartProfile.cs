using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.Carts.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.GetCart;

public class GetCartProfile : Profile
{
    public GetCartProfile()
    {
        CreateMap<CartDto, GetCartResponse>();
        CreateMap<CartItemDto, GetCartItemResponse>();
    }
}
