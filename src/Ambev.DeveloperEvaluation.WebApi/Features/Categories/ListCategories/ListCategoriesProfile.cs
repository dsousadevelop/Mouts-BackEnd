using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.ListCategories
{
    public class ListCategoriesProfile : Profile
    {
        public ListCategoriesProfile()
        {
            CreateMap<CategoryDto, ListCategoriesResponse>();
        }
    }
}
