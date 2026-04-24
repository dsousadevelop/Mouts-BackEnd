
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItem
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartItemController(IMediator _mediator) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post(CreateCartItemCommand command)
        {
            var modelRet = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = modelRet }, null);
        }
        [HttpGet("{idcart}")]
        public async Task<IActionResult> Get(int idcart)
        {
            var modelRet = await _mediator.Send(new GetItemsCartByIdCartQuery(idcart));
            return modelRet is null ? NotFound() : Ok(modelRet);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteCartItemCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Put(UpdateCartItemCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
