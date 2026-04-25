using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using OneOf;

using Ambev.DeveloperEvaluation.Application.Common.Interfaces;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Handlers
{
    public class CreateProductHandler(IProductRepository _repo, IMapper _mapper, ICacheService _cache) : IRequestHandler<CreateProductCommand, OneOf<ProductDto, ValidationError>>
    {
        public async Task<OneOf<ProductDto, ValidationError>> Handle(CreateProductCommand request, CancellationToken ct)
        {
            var model = _mapper.Map<Product>(request.ProductDto);
            
            var productRet = await _repo.CreateAsync(model, ct);
            
            await _cache.RemoveAsync("Products_List", ct);

            return _mapper.Map<ProductDto>(productRet);
        }
    }
}
