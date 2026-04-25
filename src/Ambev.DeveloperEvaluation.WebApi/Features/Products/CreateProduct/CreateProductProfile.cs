using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductProfile : Profile
{
    public CreateProductProfile()
    {
        CreateMap<CreateProductRequest, ProductDto>();
        CreateMap<RatingRequest, RatingDto>();
        CreateMap<ProductDto, CreateProductCommand>()
            .ConstructUsing(src => new CreateProductCommand(src));
        CreateMap<ProductDto, CreateProductResponse>();
        CreateMap<RatingDto, RatingResponse>();
    }
}
