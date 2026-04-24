using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Queries
{
    public record GetProductListQuery : IRequest<List<ProductDto>>;
}

