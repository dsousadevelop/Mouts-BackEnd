using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Rating_Rate, opt => opt.MapFrom(src => src.Rating.Rate))
                .ForMember(dest => dest.Rating_Count, opt => opt.MapFrom(src => (short)src.Rating.Count))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new RatingDto { Rate = src.Rating_Rate ?? 0, Count = src.Rating_Count ?? 0 }))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
        }
    }
}
