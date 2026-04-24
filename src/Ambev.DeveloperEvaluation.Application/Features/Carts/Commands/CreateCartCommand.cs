using Ambev.DeveloperEvaluation.Application.Features.Carts.DTOs;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Features.Carts.Commands
{
    public record CreateCartCommand(CartDto entityDto) : IRequest<int?>;
}
