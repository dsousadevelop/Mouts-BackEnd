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
    public class CreateCartHandler(ICartRepository _repo, IMapper _mapper) : IRequestHandler<CreateCartCommand, int?>
    {
        public async Task<int?> Handle(CreateCartCommand request, CancellationToken ct)
        {
            var model = _mapper.Map<Cart>(request.entityDto);
            var validationResult = model.Validate();
            var teste = string.Join(", ", validationResult.Errors.Select(o => o.Description));
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join(", ", validationResult.Errors.Select(o => o.Description)));

            var modelRet = await _repo.CreateAsync(model, ct);
            return modelRet.Id;
        }
    }
}
