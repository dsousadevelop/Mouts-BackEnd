using Ambev.DeveloperEvaluation.Application.Features.Carts.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Carts.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Carts.Handlers
{
    public class UpdateCartHandler(ICartRepository _repo, IMapper _mapper) : IRequestHandler<UpdateCartCommand>
    {
        public async Task Handle(UpdateCartCommand request, CancellationToken ct)
        {
            var model = _mapper.Map<Cart>(request.CartDto);
            await _repo.UpdateAsync(model, ct);
        }
    }
}
