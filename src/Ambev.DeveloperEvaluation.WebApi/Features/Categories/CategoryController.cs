using Ambev.DeveloperEvaluation.Application.Features.Categories.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Queries;
using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories
{
    [Authorize]
    public class CategoryController(IMediator _mediator, IMapper _mapper) : BaseController
    {
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CategoryResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CategoryRequest request, CancellationToken cancellationToken)
        {
            var validator = new CategoryRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CreateCategoryCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);

            return result.Match(
                category => CreatedAtAction(nameof(Get), new { id = category.Id }, new ApiResponseWithData<CategoryResponse>
                {
                    Success = true,
                    Message = "Category created successfully",
                    Data = _mapper.Map<CategoryResponse>(category)
                }),
                validationError => BadRequest(validationError.Detail)
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);

            return result.Match(
                category => Ok(new ApiResponseWithData<CategoryResponse>
                {
                    Success = true,
                    Message = "Category retrieved successfully",
                    Data = _mapper.Map<CategoryResponse>(category)
                }),
                notFoundError => NotFound(notFoundError.Detail)
            );
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<CategoryResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCategoryListQuery(), cancellationToken);
            var response = _mapper.Map<IEnumerable<CategoryResponse>>(result);
            return Ok(new ApiResponseWithData<IEnumerable<CategoryResponse>>
            {
                Success = true,
                Message = "Categories retrieved successfully",
                Data = response
            });
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(id), cancellationToken);

            return result.Match(
                success => Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Category deleted successfully"
                }),
                notFoundError => NotFound(notFoundError.Detail)
            );
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] CategoryRequest request, CancellationToken cancellationToken)
        {
            request.Id = id;
            var validator = new CategoryRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<UpdateCategoryCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);

            return result.Match(
                success => Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Category updated successfully"
                }),
                notFoundError => NotFound(notFoundError.Detail),
                validationError => BadRequest(validationError.Detail)
            );
        }
    }
}
