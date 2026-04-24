using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Features.CartItems.Queries
{
    public record GetItemsCartByIdCartQuery(int IdCart) : IRequest<List<CartItemDto>?>;
}
