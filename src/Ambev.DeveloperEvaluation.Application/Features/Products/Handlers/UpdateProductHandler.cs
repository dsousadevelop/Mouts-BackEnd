using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Handlers
{
    public class UpdateProductHandler(IProductRepository _repo, IMapper _mapper) : IRequestHandler<UpdateProductCommand, OneOf<Success, NotFoundError, ValidationError>>
    {
        public async Task<OneOf<Success, NotFoundError, ValidationError>> Handle(UpdateProductCommand request, CancellationToken ct)
        {
            var existingProduct = await _repo.GetByIdAsync(request.ProductDto.Id ?? 0, ct);
            if (existingProduct == null)
                return new NotFoundError($"Product with ID {request.ProductDto.Id} not found");

            _mapper.Map(request.ProductDto, existingProduct);
            existingProduct.UpdateAtDate();
            
            await _repo.UpdateAsync(existingProduct, ct);

            return new Success();
        }
    }
}
