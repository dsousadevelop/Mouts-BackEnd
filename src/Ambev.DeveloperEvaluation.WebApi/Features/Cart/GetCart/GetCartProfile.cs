using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.Cart.DTOs;
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
