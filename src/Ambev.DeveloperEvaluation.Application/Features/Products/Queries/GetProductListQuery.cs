using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using MediatR;
using OneOf;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Queries
{
    public record GetProductListQuery : IRequest<OneOf<List<ProductDto>, BaseError>>;
}

