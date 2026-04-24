using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Queries;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Categories.Handlers
{
    public class GetCategoryListHandler(ICategoryRepository _repo, IMapper _mapper) : IRequestHandler<GetCategoryListQuery, List<CategoryDto>>
    {
        public async Task<List<CategoryDto>> Handle(GetCategoryListQuery request, CancellationToken ct)
        {
            var producList = await _repo.GetListAllAsync(ct);
            return _mapper.Map<List<CategoryDto>>(producList);
        }
    }
}
