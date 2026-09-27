using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workspace_Management_System.Application.Features.ProductCategories.Commands.CreateProductCategory;
using Workspace_Management_System.Application.Features.ProductCategories.Commands.DeleteProductCategory;
using Workspace_Management_System.Application.Features.ProductCategories.Commands.RestoreProductCategory;
using Workspace_Management_System.Application.Features.ProductCategories.Commands.UpdateProductCategory;
using Workspace_Management_System.Application.Features.ProductCategories.Queries.GetProductCategories;
using Workspace_Management_System.Application.Features.ProductCategories.Queries.GetProductCategoryById;

namespace Workspace_Management_System.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetProductCategoriesQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetProductCategoryByIdQuery
            {
                Id = id
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateProductCategoryCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateProductCategoryCommand command)
        {
            command.Id = id;

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteProductCategoryCommand
            {
                Id = id
            };

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPatch("{id:int}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            var command = new RestoreProductCategoryCommand
            {
                Id = id
            };

            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}