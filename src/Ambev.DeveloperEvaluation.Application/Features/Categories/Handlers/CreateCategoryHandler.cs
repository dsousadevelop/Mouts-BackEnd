using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Categories.Handlers
{
    public class CreateCategoryHandler(ICategoryRepository _repo, IMapper _mapper) : IRequestHandler<CreateCategoryCommand, OneOf<CategoryDto, ValidationError>>
    {
        public async Task<OneOf<CategoryDto, ValidationError>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryDTO = new CategoryDto(id: null, description: request.CategoryDto.Description);

            var existingCategory = await _repo.GetByDescriptionAsync(categoryDTO.Description, cancellationToken);
            if (existingCategory != null)
                return new ValidationError($"Category {categoryDTO.Description} already exists");

            var model = _mapper.Map<Category>(categoryDTO);
            var modelRet = await _repo.CreateAsync(model, cancellationToken);
            categoryDTO.Id = modelRet.Id;
            return categoryDTO;
        }
    }
}
