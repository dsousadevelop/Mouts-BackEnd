using Ambev.DeveloperEvaluation.Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Categories.Commands
{
    public record DeleteCategoryCommand(int Id) : IRequest<OneOf<Success, ResourceNotFoundError>>;
}
