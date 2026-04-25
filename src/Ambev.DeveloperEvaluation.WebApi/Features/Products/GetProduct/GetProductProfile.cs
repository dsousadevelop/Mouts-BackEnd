using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

public class GetProductProfile : Profile
{
    public GetProductProfile()
    {
        CreateMap<ProductDto, GetProductResponse>();
        CreateMap<RatingDto, RatingResponse>();
    }
}
