using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using OneOf;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Handlers
{
    public class CreateProductHandler(IProductRepository _repo, IMapper _mapper) : IRequestHandler<CreateProductCommand, OneOf<ProductDto, ValidationError>>
    {
        public async Task<OneOf<ProductDto, ValidationError>> Handle(CreateProductCommand request, CancellationToken ct)
        {
            var model = _mapper.Map<Product>(request.ProductDto);
            
            var productRet = await _repo.CreateAsync(model, ct);
            
            return _mapper.Map<ProductDto>(productRet);
        }
    }
}
