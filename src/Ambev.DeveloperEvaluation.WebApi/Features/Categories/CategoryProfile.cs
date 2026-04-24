using Ambev.DeveloperEvaluation.Application.Features.Categories.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryRequest, CategoryDto>();
            CreateMap<CategoryRequest, CreateCategoryCommand>()
                .ForCtorParam("CategoryDto", opt => opt.MapFrom(src => src));
            CreateMap<CategoryRequest, UpdateCategoryCommand>()
                .ForCtorParam("CategoryDto", opt => opt.MapFrom(src => src));
            CreateMap<CategoryDto, CategoryResponse>();
        }
    }
}
