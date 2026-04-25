using Ambev.DeveloperEvaluation.Application.Common.Errors;
using MediatR;
using OneOf;

namespace Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands
{
    public record DeleteCartItemCommand(int Id) : IRequest<OneOf<bool, NotFoundError>>;
}
