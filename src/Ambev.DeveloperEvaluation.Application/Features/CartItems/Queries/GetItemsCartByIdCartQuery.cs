using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using MediatR;
using OneOf;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Features.CartItems.Queries
{
    public record GetItemsCartByIdCartQuery(int CartId) : IRequest<OneOf<List<CartItemDto>, ValidationError>>;
}
