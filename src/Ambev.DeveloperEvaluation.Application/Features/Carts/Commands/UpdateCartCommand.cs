using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Carts.Commands
{
    public record UpdateCartCommand(CategoryDto CategoryDto) : IRequest;
}
