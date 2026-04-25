using Ambev.DeveloperEvaluation.Application.Features.Carts.Commands;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using OneOf.Types;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ambev.DeveloperEvaluation.Application.Features.Carts.Handlers
{
    public class CreateCartHandler(ICartRepository _repo, IProductRepository _productRepo, IMapper _mapper) : IRequestHandler<CreateCartCommand, int?>
    {
        public async Task<int?> Handle(CreateCartCommand request, CancellationToken ct)
        {
            var model = _mapper.Map<Cart>(request.entityDto);
            
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

            var validationResult = model.Validate();
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join(", ", validationResult.Errors.Select(o => o.Description)));

            var modelRet = await _repo.CreateAsync(model, ct);
            return modelRet.Id;
        }
    }
}
