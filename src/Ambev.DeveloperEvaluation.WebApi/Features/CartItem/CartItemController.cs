using Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Queries;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.CartItem.CreateCartItem;
using Ambev.DeveloperEvaluation.WebApi.Features.CartItem.DeleteCartItem;
using Ambev.DeveloperEvaluation.WebApi.Features.CartItem.GetCartItems;
using Ambev.DeveloperEvaluation.WebApi.Features.CartItem.UpdateCartItem;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItem;

[Authorize]
public class CartItemController(IMediator _mediator, IMapper _mapper) : BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateCartItemResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] CreateCartItemRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateCartItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateCartItemCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(nameof(Get), new { idcart = request.CartId }, _mapper.Map<CreateCartItemResponse>(response));
    }

    [HttpGet("{idcart}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetCartItemsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int idcart, CancellationToken cancellationToken)
    {
        var request = new GetCartItemsRequest { CartId = idcart };
        var validator = new GetCartItemsRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var query = new GetItemsCartByIdCartQuery(request.CartId);
        var items = await _mediator.Send(query, cancellationToken);

        if (items == null)
            return NotFound($"Items for cart with ID {idcart} not found");

        return Ok(_mapper.Map<GetCartItemsResponse>(items));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var request = new DeleteCartItemRequest { Id = id };
        var validator = new DeleteCartItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<DeleteCartItemCommand>(request);
        await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse { Message = "Cart item deleted successfully", Success = true });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateCartItemRequest request, CancellationToken cancellationToken)
    {
        request.Id = id;
        var validator = new UpdateCartItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateCartItemCommand>(request);
        await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse { Message = "Cart item updated successfully", Success = true });
    }
}
