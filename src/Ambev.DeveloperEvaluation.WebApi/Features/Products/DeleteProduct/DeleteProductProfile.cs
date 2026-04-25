using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;

public class DeleteProductProfile : Profile
{
    public DeleteProductProfile()
    {
        CreateMap<DeleteProductRequest, DeleteProductCommand>()
            .ConstructUsing(src => new DeleteProductCommand(src.Id));
    }
}
