using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands
{
    public record DeleteCartItemCommand(int Ìd) : IRequest;
}
