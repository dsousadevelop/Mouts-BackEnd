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
    public class GetCartByIdHandler(ICartRepository _repo, IMapper _mapper) : IRequestHandler<GetCartByIdQuery, CartDto?>
    {
        public async Task<CartDto?> Handle(GetCartByIdQuery request, CancellationToken ct)
        {
            var productRet = await _repo.GetByIdAsync(request.Id, ct);
            return _mapper.Map<CartDto?>(productRet);
        }

    }
}
