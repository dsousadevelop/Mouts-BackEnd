using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands
{
    public record CreateCartItemCommand(CartItemDto entityDto) : IRequest<int?>;
}
