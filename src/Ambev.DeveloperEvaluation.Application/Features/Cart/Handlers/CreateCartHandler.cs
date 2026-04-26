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
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Features.Cart.Handlers
{
    public class CreateCartHandler(ICartRepository _repo, IProductRepository _productRepo, IUserRepository _userRepo, Rebus.Bus.IBus _bus, IMapper _mapper) : IRequestHandler<CreateCartCommand, OneOf<CartDto, ValidationError>>
    {
        public async Task<OneOf<CartDto, ValidationError>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<Domain.Entities.Cart>(request.entityDto);

            if (model.CartItems != null)
            {
                foreach (var item in model.CartItems)
                {
                    var product = await _productRepo.GetByIdAsync(item.ProductId, cancellationToken);
                    if (product == null)
                    {
                        return new ValidationError($"product {item.ProductId} not exists");
                    }
                    item.CalculateDiscount(product.Price);
}

                model.CalculateTotalAmount();
            }

            var validationResult = await model.ValidateAsync();
            if (!validationResult.IsValid)
                return new ValidationError(string.Join(", ", validationResult.Errors.Select(o => o.Description)));

            try
            {
                model.DateSaveCart();
                var modelRet = await _repo.CreateAsync(model, cancellationToken);
                
                // Busca o e-mail do usuário para o evento
                var user = await _userRepo.GetByIdAsync(modelRet.UserId, cancellationToken);
                if (user != null)
                {
                    await _bus.Publish(new CartCreatedEvent
                    {
                        CartId = modelRet.Id ?? 0,
                        UserId = modelRet.UserId,
                        UserEmail = user.Email,
                        TotalAmount = modelRet.TotalAmount
                    });
                }

                return _mapper.Map<CartDto>(modelRet);
            }
            catch (System.Exception ex)
            {
                return new ValidationError($"An error occurred while saving the entity changes: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
