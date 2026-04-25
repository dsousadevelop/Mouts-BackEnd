using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Cart.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Queries;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using OneOf;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Cart.Handlers
{
    public class GetCartListHandler(ICartRepository _repo, IMapper _mapper) : IRequestHandler<GetCartListQuery, OneOf<List<CartDto>, ValidationError>>
    {
        public async Task<OneOf<List<CartDto>, ValidationError>> Handle(GetCartListQuery request, CancellationToken cancellationToken)
        {
            var cartList = await _repo.GetListAllAsync(cancellationToken);
            return _mapper.Map<List<CartDto>>(cartList);
        }
    }
}

