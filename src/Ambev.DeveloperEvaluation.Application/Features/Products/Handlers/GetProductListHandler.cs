using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Common.Interfaces;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Products.Queries;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using OneOf;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Handlers
{
    public class GetProductListHandler(IProductRepository _repo, IMapper _mapper, ICacheService _cache)
        : IRequestHandler<GetProductListQuery, OneOf<PagedResult<ProductDto>, BaseError>>
    {
        public async Task<OneOf<PagedResult<ProductDto>, BaseError>> Handle(GetProductListQuery request, CancellationToken ct)
        {
            const string cacheKey = "Products_List";
            var productList = await _cache.GetAsync<List<ProductDto>>(cacheKey, ct);

            if (productList == null)
            {
                var products = await _repo.GetListAllAsync(ct);
                productList = _mapper.Map<List<ProductDto>>(products);
                await _cache.SetAsync(cacheKey, productList, ct: ct);
            }

            if (request.CategoryId.HasValue)
                productList = productList.Where(p => p.CategoryId == request.CategoryId.Value).ToList();

            var totalItems = productList.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.Size);
            var data = productList.Skip((request.Page - 1) * request.Size).Take(request.Size).ToList();

            return new PagedResult<ProductDto>
            {
                Data = data,
                TotalItems = totalItems,
                CurrentPage = request.Page,
                TotalPages = totalPages
            };
        }
    }
}
