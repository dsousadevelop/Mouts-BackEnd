using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands;

namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItems.DeleteCartItem;

public class DeleteCartItemProfile : Profile
{
    public DeleteCartItemProfile()
    {
        CreateMap<DeleteCartItemRequest, DeleteCartItemCommand>()
            .ConstructUsing(src => new DeleteCartItemCommand(src.Id));
    }
}

