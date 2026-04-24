using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<RatingRequest, RatingDto>();
            CreateMap<ProductRequest, ProductDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category_Id));

            CreateMap<ProductRequest, CreateProductCommand>()
                .ForCtorParam("ProductDto", opt => opt.MapFrom(src => src));

            CreateMap<ProductRequest, UpdateProductCommand>()
                .ForCtorParam("ProductDto", opt => opt.MapFrom(src => src));

            CreateMap<ProductDto, ProductResponse>()
                .ForMember(dest => dest.Category_Id, opt => opt.MapFrom(src => src.CategoryId));

            CreateMap<RatingDto, RatingResponse>();
        }
    }
}
