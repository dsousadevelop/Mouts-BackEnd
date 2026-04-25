using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Cart.DTOs;
using MediatR;
using OneOf;

namespace Ambev.DeveloperEvaluation.Application.Features.Cart.Commands
{
    public record CreateCartCommand(CartDto entityDto) : IRequest<OneOf<CartDto, ValidationError>>;
}

