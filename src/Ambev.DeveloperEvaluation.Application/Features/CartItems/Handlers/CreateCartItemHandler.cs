using Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Features.CartItems.Handlers
{
    public class CreateCartItemHandler(ICartItemRepository _repo, IProductRepository _productRepo, IMapper _mapper) : IRequestHandler<CreateCartItemCommand, int?>
    {
        public async Task<int?> Handle(CreateCartItemCommand request, CancellationToken ct)
        {
            var product = await _productRepo.GetByIdAsync(request.entityDto.ProductId, ct);
            if (product == null)
                throw new Exception("Product not found");

            var model = _mapper.Map<CartItem>(request.entityDto);
            model.CalculateDiscount(product.Price);

            var modelRet = await _repo.CreateAsync(model, ct);
            return modelRet.Id;
        }
    }
}
