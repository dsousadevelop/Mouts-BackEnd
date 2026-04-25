using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Cart.DTOs;
using MediatR;
using OneOf;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Features.Cart.Queries
{
    public record GetCartListQuery : IRequest<OneOf<List<CartDto>, ValidationError>>;
}

