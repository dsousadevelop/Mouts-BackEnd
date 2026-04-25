using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Products.Queries;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using OneOf;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Handlers
{
    public class GetProductListHandler(IProductRepository _repo, IMapper _mapper) : IRequestHandler<GetProductListQuery, OneOf<List<ProductDto>, BaseError>>
    {
        public async Task<OneOf<List<ProductDto>, BaseError>> Handle(GetProductListQuery request, CancellationToken ct)
        {
            var products = await _repo.GetListAllAsync(ct);
            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}
