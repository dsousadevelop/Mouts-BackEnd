using Ambev.DeveloperEvaluation.Application.Features.CartItems.Commands;
using Ambev.DeveloperEvaluation.Application.Features.CartItems.Queries;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.CartItems.CreateCartItem;
using Ambev.DeveloperEvaluation.WebApi.Features.CartItems.DeleteCartItem;
using Ambev.DeveloperEvaluation.WebApi.Features.CartItems.GetCartItems;
using Ambev.DeveloperEvaluation.WebApi.Features.CartItems.UpdateCartItem;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using Ambev.DeveloperEvaluation.Application.Common.Errors;

namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItems;

[Authorize]
[ApiController]
[Route("api/[controller]")]
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
        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => CreatedAtAction(nameof(Get), new { cartId = success.CartId }, new ApiResponseWithData<CreateCartItemResponse>
            {
                Success = true,
                Message = "Cart item created successfully",
                Data = _mapper.Map<CreateCartItemResponse>(success)
            }),
            validationError => BadRequest(validationError.Detail)
        );
    }

    [HttpGet("{cartId}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetCartItemsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get([FromRoute] int cartId, CancellationToken cancellationToken)
    {
        var request = new GetCartItemsRequest { CartId = cartId };
        var validator = new GetCartItemsRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var query = new GetItemsCartByIdCartQuery(request.CartId);
        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(
            success => Ok(new ApiResponseWithData<GetCartItemsResponse>
            {
                Success = true,
                Message = "Cart items retrieved successfully",
                Data = _mapper.Map<GetCartItemsResponse>(success)
            }),
            validationError => BadRequest(validationError.Detail)
        );
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new DeleteCartItemCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(new ApiResponse { Message = "Cart item deleted successfully", Success = true }),
            notFound => NotFound(notFound.Detail)
        );
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateCartItemRequest request, CancellationToken cancellationToken)
    {
        request.Id = id;
        var validator = new UpdateCartItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateCartItemCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(new ApiResponse { Message = "Cart item updated successfully", Success = true }),
            notFound => NotFound(notFound.Detail),
            validationError => BadRequest(validationError.Detail)
        );
    }
}
