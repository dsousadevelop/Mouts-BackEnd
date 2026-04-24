using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
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
    public record UpdateCategoryCommand(CategoryDto CategoryDto) : IRequest<OneOf<Success, ResourceNotFoundError, ValidationError>>;
}
