using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Attributes;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.ProductCategories.Commands.CreateProductCategory;
using Workspace_Management_System.Application.Features.ProductCategories.Commands.DeleteProductCategory;
using Workspace_Management_System.Application.Features.ProductCategories.Commands.RestoreProductCategory;
using Workspace_Management_System.Application.Features.ProductCategories.Commands.UpdateProductCategory;
using Workspace_Management_System.Application.Features.ProductCategories.Queries.GetProductCategories;
using Workspace_Management_System.Application.Features.ProductCategories.Queries.GetProductCategoryById;
using Workspace_Management_System.Domain.Constants;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductCategoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [RequirePermission(Permissions.ProductCategoriesCreate)]
        [SwaggerOperation(
            Summary = "Create product category",
            Description = "Creates a new product category."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(
            [FromBody] CreateProductCategoryCommand command,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpPut("{id:int}")]
        [RequirePermission(Permissions.ProductCategoriesUpdate)]
        [SwaggerOperation(
            Summary = "Update product category",
            Description = "Updates an existing product category."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateProductCategoryCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpDelete("{id:int}")]
        [RequirePermission(Permissions.ProductCategoriesDelete)]
        [SwaggerOperation(
            Summary = "Delete product category",
            Description = "Soft deletes an existing product category."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteProductCategoryCommand
            {
                Id = id
            };

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpGet]
        [RequirePermission(Permissions.ProductCategoriesView)]
        [SwaggerOperation(
            Summary = "Get product categories",
            Description = "Retrieves a paginated list of active product categories."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetProductCategories(
            [FromQuery] GetProductCategoriesQuery query,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpGet("{id:int}")]
        [RequirePermission(Permissions.ProductCategoriesViewDetails)]
        [SwaggerOperation(
            Summary = "Get product category by ID",
            Description = "Retrieves a product category by ID."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var query = new GetProductCategoryByIdQuery
            {
                Id = id
            };

            var result = await mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpPatch("{id:int}/restore")]
        [RequirePermission(Permissions.ProductCategoriesRestore)]
        [SwaggerOperation(
            Summary = "Restore product category",
            Description = "Restores a previously soft-deleted product category."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Restore(
            int id,
            CancellationToken cancellationToken)
        {
            var command = new RestoreProductCategoryCommand
            {
                Id = id
            };

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
    }
}
