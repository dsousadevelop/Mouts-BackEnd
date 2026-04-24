using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Products.Queries;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using OneOf;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Handlers
{
    public class GetProductByIdHandler(IProductRepository _repo, IMapper _mapper) : IRequestHandler<GetProductByIdQuery, OneOf<ProductDto, NotFoundError>>
    {
        public async Task<OneOf<ProductDto, NotFoundError>> Handle(GetProductByIdQuery request, CancellationToken ct)
        {
            var product = await _repo.GetByIdAsync(request.Id, ct);
            if (product == null)
                return new NotFoundError($"Product with ID {request.Id} not found");

            return _mapper.Map<ProductDto>(product);
        }
    }
}
