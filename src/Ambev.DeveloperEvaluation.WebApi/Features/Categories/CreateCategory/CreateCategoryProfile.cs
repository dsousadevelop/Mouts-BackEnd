using Ambev.DeveloperEvaluation.Application.Features.Categories.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.CreateCategory
{
    public class CreateCategoryProfile : Profile
    {
        public CreateCategoryProfile()
        {
            CreateMap<CreateCategoryRequest, CategoryDto>();
            CreateMap<CreateCategoryRequest, CreateCategoryCommand>()
                .ConstructUsing((src, ctx) => new CreateCategoryCommand(ctx.Mapper.Map<CategoryDto>(src)));
            CreateMap<CategoryDto, CreateCategoryResponse>();
        }
    }
}
