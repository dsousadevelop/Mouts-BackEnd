using Ambev.DeveloperEvaluation.Application.Features.Carts.DTOs;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Features.Carts.Commands
{
    public record UpdateCartCommand(CartDto CartDto) : IRequest;
}
