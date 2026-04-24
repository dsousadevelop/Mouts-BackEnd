using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using MediatR;
using OneOf;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Queries
{
    public record GetProductByIdQuery(int Id) : IRequest<OneOf<ProductDto, NotFoundError>>;
}

