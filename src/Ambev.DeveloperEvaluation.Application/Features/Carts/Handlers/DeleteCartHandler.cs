using Ambev.DeveloperEvaluation.Application.Features.Carts.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Carts.Handlers
{
    public class DeleteCartHandler(ICategoryRepository _repo) : IRequestHandler<DeleteCartCommand>
    {
        public async Task Handle(DeleteCartCommand request, CancellationToken ct)
        {
            await _repo.DeleteAsync(request.Ìd, ct);
        }
    }
}
