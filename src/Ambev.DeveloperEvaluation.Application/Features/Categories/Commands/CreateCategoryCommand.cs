using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using MediatR;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Categories.Commands
{
    public record CreateCategoryCommand(CategoryDto CategoryDto) : IRequest<OneOf<CategoryDto, ValidationError>>;
}
