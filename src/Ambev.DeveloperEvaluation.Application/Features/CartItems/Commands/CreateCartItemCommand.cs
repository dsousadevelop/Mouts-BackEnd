using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using MediatR;
using OneOf;

namespace Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands
{
    public record CreateCartItemCommand(CartItemDto CartItemDto) : IRequest<OneOf<CartItemDto, ValidationError>>;
}
