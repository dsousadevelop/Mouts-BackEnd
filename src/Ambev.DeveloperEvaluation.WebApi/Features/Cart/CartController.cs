using Ambev.DeveloperEvaluation.Application.Features.Cart.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Cart.Queries;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.ListCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.UpdateCart;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using Ambev.DeveloperEvaluation.Application.Common.Errors;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CartController(IMediator _mediator, IMapper _mapper) : BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateCartResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] CreateCartRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateCartCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => CreatedAtAction(nameof(Get), new { id = success.Id }, new ApiResponseWithData<CreateCartResponse>
            {
                Success = true,
                Message = "Cart created successfully",
                Data = _mapper.Map<CreateCartResponse>(success)
            }),
            validationError => BadRequest(validationError.Detail)
        );
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetCartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetCartByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(
            success => Ok(new ApiResponseWithData<GetCartResponse>
            {
                Success = true,
                Message = "Cart retrieved successfully",
                Data = _mapper.Map<GetCartResponse>(success)
            }),
            notFound => NotFound(notFound.Detail)
        );
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<ListCartsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var query = new GetCartListQuery();
        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(
            success => Ok(new ApiResponseWithData<ListCartsResponse>
            {
                Success = true,
                Message = "Carts retrieved successfully",
                Data = new ListCartsResponse { Carts = _mapper.Map<List<GetCartResponse>>(success) }
            }),
            validationError => BadRequest(validationError.Detail)
        );
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new DeleteCartCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(new ApiResponse { Message = "Cart deleted successfully", Success = true }),
            notFound => NotFound(notFound.Detail)
        );
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateCartRequest request, CancellationToken cancellationToken)
    {
        request.Id = id;
        var validator = new UpdateCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        request.Id = id;

        var command = _mapper.Map<UpdateCartCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            success => Ok(new ApiResponseWithData<GetCartResponse>
            {
                Success = true,
                Message = "Cart updated successfully",
                Data = _mapper.Map<GetCartResponse>(success)
            }),
            notFound => NotFound(notFound.Detail),
            validationError => BadRequest(validationError.Detail)
        );
    }
}
