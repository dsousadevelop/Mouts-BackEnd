using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Products.Queries;
using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products
{
    [Authorize]
    public class ProductController(IMediator _mediator, IMapper _mapper) : BaseController
    {
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<ProductResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(ProductRequest request, CancellationToken ct)
        {
            var validator = new ProductRequestValidator();
            var validationResult = await validator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CreateProductCommand>(request);
            var result = await _mediator.Send(command, ct);

            return result.Match(
                data => CreatedAtAction(nameof(Get), new { id = data.Id }, new ApiResponseWithData<ProductResponse> 
                { 
                    Data = _mapper.Map<ProductResponse>(data), 
                    Success = true,
                    Message = "Product created successfully"
                }),
                error => BadRequest(error.Detail)
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken ct)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query, ct);

            return result.Match(
                data => Ok(new ApiResponseWithData<ProductResponse> 
                { 
                    Data = _mapper.Map<ProductResponse>(data), 
                    Success = true,
                    Message = "Product retrieved successfully"
                }),
                error => NotFound(error.Detail)
            );
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<List<ProductResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetProductListQuery(), ct);
            var response = _mapper.Map<List<ProductResponse>>(result);
            return Ok(new ApiResponseWithData<List<ProductResponse>> 
            { 
                Data = response, 
                Success = true,
                Message = "Products retrieved successfully"
            });
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            var command = new DeleteProductCommand(id);
            var result = await _mediator.Send(command, ct);

            return result.Match(
                success => Ok(new ApiResponse { Message = "Product deleted successfully", Success = true }),
                error => NotFound(error.Detail)
            );
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ProductRequest request, CancellationToken ct)
        {
            request.Id = id;
            var validator = new ProductRequestValidator();
            var validationResult = await validator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<UpdateProductCommand>(request);
            var result = await _mediator.Send(command, ct);

            return result.Match(
                success => Ok(new ApiResponse { Message = "Product updated successfully", Success = true }),
                error => NotFound(error.Detail),
                validationError => BadRequest(validationError.Detail)
            );
        }
    }
}

