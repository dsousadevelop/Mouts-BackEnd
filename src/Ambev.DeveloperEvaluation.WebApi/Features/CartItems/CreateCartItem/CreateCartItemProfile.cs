using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;

namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItems.CreateCartItem;

public class CreateCartItemProfile : Profile
{
    public CreateCartItemProfile()
    {
        CreateMap<CreateCartItemRequest, CartItemDto>();
        CreateMap<CartItemDto, CreateCartItemCommand>()
            .ConstructUsing(src => new CreateCartItemCommand(src));
        CreateMap<int, CreateCartItemResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));
    }
}

