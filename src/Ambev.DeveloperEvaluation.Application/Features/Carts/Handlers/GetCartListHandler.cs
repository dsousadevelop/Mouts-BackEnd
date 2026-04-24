using Ambev.DeveloperEvaluation.Application.Features.Carts.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Carts.Queries;
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

namespace Ambev.DeveloperEvaluation.Application.Features.Carts.Handlers
{
    public class GetCartListHandler(ICategoryRepository _repo, IMapper _mapper) : IRequestHandler<GetCartListQuery, List<CartDto>>
    {
        public async Task<List<CartDto>> Handle(GetCartListQuery request, CancellationToken ct)
        {
            var producList = await _repo.GetListAllAsync(ct);
            return _mapper.Map<List<CartDto>>(producList);
        }
    }
}
