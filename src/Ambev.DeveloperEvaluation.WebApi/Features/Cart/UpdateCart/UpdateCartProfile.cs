using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.Carts.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Carts.DTOs;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.UpdateCart;

public class UpdateCartProfile : Profile
{
    public UpdateCartProfile()
    {
        CreateMap<UpdateCartRequest, CartDto>();
        CreateMap<UpdateCartRequest, UpdateCartCommand>()
            .ConstructUsing((src, ctx) => new UpdateCartCommand(ctx.Mapper.Map<CartDto>(src)));
        CreateMap<CartDto, UpdateCartCommand>()
            .ConstructUsing(src => new UpdateCartCommand(src));
    }
}
