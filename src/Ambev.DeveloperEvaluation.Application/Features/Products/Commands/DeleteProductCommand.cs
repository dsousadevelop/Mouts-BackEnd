using Ambev.DeveloperEvaluation.Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.Commands
{
    public record DeleteProductCommand(int Id) : IRequest<OneOf<Success, NotFoundError>>;
}
