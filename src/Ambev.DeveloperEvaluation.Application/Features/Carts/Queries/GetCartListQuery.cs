using Ambev.DeveloperEvaluation.Application.Features.Carts.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Carts.Queries
{
    public record GetCartListQuery : IRequest<List<CartDto>>;
}
