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
    public class UpdateCartHandler(ICartRepository _repo, IProductRepository _productRepo, IMapper _mapper) : IRequestHandler<UpdateCartCommand>
    {
        public async Task Handle(UpdateCartCommand request, CancellationToken ct)
        {
            var model = _mapper.Map<Cart>(request.CartDto);

            if (model.CartItems != null)
            {
                foreach (var item in model.CartItems)
                {
                    var product = await _productRepo.GetByIdAsync(item.ProductId, ct);
                    if (product != null)
                    {
                        item.CalculateDiscount(product.Price);
                    }
                }

                model.CalculateTotalAmount();
            }

            await _repo.UpdateAsync(model, ct);
        }
    }
}
