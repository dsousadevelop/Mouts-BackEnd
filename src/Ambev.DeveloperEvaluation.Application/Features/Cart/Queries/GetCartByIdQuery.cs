using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Cart.DTOs;
using MediatR;
using OneOf;

namespace Ambev.DeveloperEvaluation.Application.Features.Cart.Queries
{
    public record GetCartByIdQuery(int Id) : IRequest<OneOf<CartDto, NotFoundError>>;
}

