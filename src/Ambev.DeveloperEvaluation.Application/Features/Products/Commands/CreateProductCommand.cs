using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using MediatR;
using OneOf;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Commands
{
    public record CreateProductCommand(ProductDto ProductDto) : IRequest<OneOf<ProductDto, ValidationError>>;
}
