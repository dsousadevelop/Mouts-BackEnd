using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Cart.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Queries;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using OneOf;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Cart.Handlers
{
    public class GetCartByIdHandler(ICartRepository _repo, IMapper _mapper) : IRequestHandler<GetCartByIdQuery, OneOf<CartDto, NotFoundError>>
    {
        public async Task<OneOf<CartDto, NotFoundError>> Handle(GetCartByIdQuery request, CancellationToken ct)
        {
            var cart = await _repo.GetByIdAsync(request.Id, ct);
            if (cart == null)
                return new NotFoundError($"Cart with ID {request.Id} not found");

            return _mapper.Map<CartDto>(cart);
        }
    }
}

