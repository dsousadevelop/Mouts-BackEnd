using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Commands;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.DeleteCart;

public class DeleteCartProfile : Profile
{
    public DeleteCartProfile()
    {
        CreateMap<DeleteCartRequest, DeleteCartCommand>()
            .ConstructUsing(src => new DeleteCartCommand(src.Id));
    }
}

