using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Cart.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Features.CartItems.CreateCartItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart;

public class CreateCartProfile : Profile
{
    public CreateCartProfile()
    {
        CreateMap<CreateCartRequest, CartDto>();
        CreateMap<CreateCartItemRequest, CartItemDto>();
        CreateMap<CreateCartRequest, CreateCartCommand>()
            .ConstructUsing((src, ctx) => new CreateCartCommand(ctx.Mapper.Map<CartDto>(src)));
        CreateMap<CartDto, CreateCartCommand>()
            .ConstructUsing(src => new CreateCartCommand(src));
        CreateMap<CartDto, CreateCartResponse>();
    }
}
