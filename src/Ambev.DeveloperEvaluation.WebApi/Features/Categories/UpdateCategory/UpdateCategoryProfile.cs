using Ambev.DeveloperEvaluation.Application.Features.Categories.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.UpdateCategory
{
    public class UpdateCategoryProfile : Profile
    {
        public UpdateCategoryProfile()
        {
            CreateMap<UpdateCategoryRequest, CategoryDto>();
            CreateMap<UpdateCategoryRequest, UpdateCategoryCommand>()
                .ConstructUsing((src, ctx) => new UpdateCategoryCommand(ctx.Mapper.Map<CategoryDto>(src)));
            CreateMap<CategoryDto, UpdateCategoryResponse>();
        }
    }
}
