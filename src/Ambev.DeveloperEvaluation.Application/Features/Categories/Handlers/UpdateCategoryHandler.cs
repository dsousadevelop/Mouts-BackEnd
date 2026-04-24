using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Commands;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Categories.Handlers
{
    public class UpdateCategoryHandler(ICategoryRepository _repo, IMapper _mapper) : IRequestHandler<UpdateCategoryCommand, OneOf<Success, ResourceNotFoundError, ValidationError>>
    {
        public async Task<OneOf<Success, ResourceNotFoundError, ValidationError>> Handle(UpdateCategoryCommand request, CancellationToken ct)
        {
            var existingCategory = await _repo.GetByIdAsync(request.CategoryDto.Id ?? 0, ct);
            if (existingCategory == null)
                return new ResourceNotFoundError($"The Category with ID {request.CategoryDto.Id} does not exist in our database");

            var existingCategoryWithSameDesc = await _repo.GetByDescriptionAsync(request.CategoryDto.Description, ct);
            if (existingCategoryWithSameDesc != null && existingCategoryWithSameDesc.Id != request.CategoryDto.Id)
                return new ValidationError($"Category {request.CategoryDto.Description} already exists");

            existingCategory.UpdateDescription(request.CategoryDto.Description);
            await _repo.UpdateAsync(existingCategory, ct);
            return new Success();
        }
    }
}
