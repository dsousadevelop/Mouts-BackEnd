using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.GetCategory
{
    public class GetCategoryProfile : Profile
    {
        public GetCategoryProfile()
        {
            CreateMap<CategoryDto, GetCategoryResponse>();
        }
    }
}
