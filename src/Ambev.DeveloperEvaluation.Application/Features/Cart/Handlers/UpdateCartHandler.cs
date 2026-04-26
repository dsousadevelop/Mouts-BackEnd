using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Cart.DTOs;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using OneOf;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Cart.Handlers
{
    public class UpdateCartHandler(ICartRepository _repo, IProductRepository _productRepo, IMapper _mapper) : IRequestHandler<UpdateCartCommand, OneOf<CartDto, NotFoundError, ValidationError>>
    {
        public async Task<OneOf<CartDto, NotFoundError, ValidationError>> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            if (request.CartDto.Id != null)
            {
                var cartExists = await _repo.GetByIdAsync((int)request.CartDto.Id, cancellationToken);
                if (cartExists == null)
                    return new NotFoundError($"Cart with ID {request.CartDto.Id} not found");

                _mapper.Map(request.CartDto, cartExists);

                if (cartExists.CartItems != null)
                {
                    foreach (var item in cartExists.CartItems)
                    {
                        var product = await _productRepo.GetByIdAsync(item.ProductId, cancellationToken);
                        if (product != null)
                        {
                            item.CalculateDiscount(product.Price);
                        }
                    }

                    cartExists.CalculateTotalAmount();
                }

                var validationResult = await cartExists.ValidateAsync();
                if (!validationResult.IsValid)
                    return new ValidationError(string.Join(", ", validationResult.Errors.Select(o => o.Description)));

                var updatedCart = await _repo.UpdateAsync(cartExists, cancellationToken);
                return _mapper.Map<CartDto>(updatedCart);
            }
            return new ValidationError("ID is null invalid value");
        }
    }
}
