using Ambev.DeveloperEvaluation.Application.Features.Categories.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Queries;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories.CreateCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories.GetCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories.ListCategories;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories.UpdateCategory;
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
        [ProducesResponseType(typeof(ApiResponseWithData<CreateCategoryResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateCategoryRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CreateCategoryCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);

            return result.Match(
                category => CreatedAtAction(nameof(Get), new { id = category.Id }, new ApiResponseWithData<CreateCategoryResponse>
                {
                    Success = true,
                    Message = "Category created successfully",
                    Data = _mapper.Map<CreateCategoryResponse>(category)
                }),
                validationError => BadRequest(validationError.Detail)
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetCategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);

            return result.Match(
                category => Ok(new ApiResponseWithData<GetCategoryResponse>
                {
                    Success = true,
                    Message = "Category retrieved successfully",
                    Data = _mapper.Map<GetCategoryResponse>(category)
                }),
                notFoundError => NotFound(notFoundError.Detail)
            );
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<ListCategoriesResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCategoryListQuery(), cancellationToken);
            var response = _mapper.Map<IEnumerable<ListCategoriesResponse>>(result);
            return Ok(new ApiResponseWithData<IEnumerable<ListCategoriesResponse>>
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
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateCategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            request.Id = id;
            var validator = new UpdateCategoryRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<UpdateCategoryCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);

            return result.Match(
                category => Ok(new ApiResponseWithData<UpdateCategoryResponse>
                {
                    Success = true,
                    Message = "Category updated successfully",
                    Data = _mapper.Map<UpdateCategoryResponse>(category)
                }),
                notFoundError => NotFound(notFoundError.Detail),
                validationError => BadRequest(validationError.Detail)
            );
        }
    }
}
