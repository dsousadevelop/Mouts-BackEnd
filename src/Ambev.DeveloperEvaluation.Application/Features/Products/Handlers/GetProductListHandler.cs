using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Products.Queries;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Handlers
{
    public class GetProductListHandler(IProductRepository _repo, IMapper _mapper) : IRequestHandler<GetProductListQuery, List<ProductDto>>
    {
        public async Task<List<ProductDto>> Handle(GetProductListQuery request, CancellationToken ct)
        {
            var products = await _repo.GetListAllAsync(ct);
            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}
