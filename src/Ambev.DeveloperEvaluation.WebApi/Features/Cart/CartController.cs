using Ambev.DeveloperEvaluation.Application.Features.Carts.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Carts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController(IMediator _mediator) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post(CreateCartCommand command)
        {
            try
            {
                var modelRet = await _mediator.Send(command);
                return CreatedAtAction(nameof(Get), new { id = modelRet }, null);
            }
            catch (ValidationException ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
            
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var modelRet = await _mediator.Send(new GetCartByIdQuery(id));
            return modelRet is null ? NotFound() : Ok(modelRet);
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var modelRet = await _mediator.Send(new GetCartListQuery());
            return modelRet is null ? NotFound() : Ok(modelRet);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteCartCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Put(UpdateCartCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
