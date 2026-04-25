using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

public class UpdateProductProfile : Profile
{
    public UpdateProductProfile()
    {
        CreateMap<UpdateProductRequest, ProductDto>();
        CreateMap<RatingRequest, RatingDto>();
        CreateMap<UpdateProductRequest, UpdateProductCommand>()
            .ConstructUsing((src, ctx) => new UpdateProductCommand(ctx.Mapper.Map<ProductDto>(src)));
        CreateMap<ProductDto, UpdateProductCommand>()
            .ConstructUsing(src => new UpdateProductCommand(src));
    }
}
