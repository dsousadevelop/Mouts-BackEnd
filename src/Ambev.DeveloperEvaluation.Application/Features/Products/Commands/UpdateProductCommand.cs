using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Commands
{
    public record UpdateProductCommand(ProductDto ProductDto) : IRequest<OneOf<ProductDto, NotFoundError, ValidationError>>;
}