using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Queries;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
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
    public class GetCategoryByIdHandler(ICategoryRepository _repo, IMapper _mapper) : IRequestHandler<GetCategoryByIdQuery, OneOf<CategoryDto, ResourceNotFoundError>>
    {
        public async Task<OneOf<CategoryDto, ResourceNotFoundError>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var productRet = await _repo.GetByIdAsync(request.Id, cancellationToken);

            if (productRet == null)
                return new ResourceNotFoundError($"The Category with ID {request.Id} does not exist in our database");

            return _mapper.Map<CategoryDto>(productRet);
        }

    }
}
