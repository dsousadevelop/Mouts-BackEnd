using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

public class ListProductsProfile : Profile
{
    public ListProductsProfile()
    {
        CreateMap<List<ProductDto>, ListProductsResponse>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src));
    }
}
