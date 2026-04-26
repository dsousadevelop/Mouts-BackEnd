using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using MediatR;
using OneOf;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Queries
{
    public record GetProductListQuery(int Page = 1, int Size = 10, int? CategoryId = null) : IRequest<OneOf<PagedResult<ProductDto>, BaseError>>;
}
