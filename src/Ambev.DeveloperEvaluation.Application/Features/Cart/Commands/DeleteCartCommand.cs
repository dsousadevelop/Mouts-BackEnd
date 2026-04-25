using Ambev.DeveloperEvaluation.Application.Common.Errors;
using MediatR;
using OneOf;

namespace Ambev.DeveloperEvaluation.Application.Features.Cart.Commands
{
    public record DeleteCartCommand(int Id) : IRequest<OneOf<bool, NotFoundError>>;
}

