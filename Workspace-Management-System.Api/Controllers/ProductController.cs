using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Attributes;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.Products.Commands.CreateProduct;
using Workspace_Management_System.Application.Features.Products.Commands.DeleteProduct;
using Workspace_Management_System.Application.Features.Products.Commands.RestoreProduct;
using Workspace_Management_System.Application.Features.Products.Commands.UpdateProduct;
using Workspace_Management_System.Application.Features.Products.Queries.GetProductById;
using Workspace_Management_System.Application.Features.Products.Queries.GetProducts;
using Workspace_Management_System.Domain.Constants;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [RequirePermission(Permissions.ProductsCreate)]
        [SwaggerOperation(
            Summary = "Create product",
            Description = "Creates a new product."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(
            [FromBody] CreateProductCommand command,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpPut("{id:int}")]
        [RequirePermission(Permissions.ProductsUpdate)]
        [SwaggerOperation(
            Summary = "Update product",
            Description = "Updates an existing product."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateProductCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpDelete("{id:int}")]
        [RequirePermission(Permissions.ProductsDelete)]
        [SwaggerOperation(
            Summary = "Delete product",
            Description = "Soft deletes an existing product."
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
            var command = new DeleteProductCommand
            {
                Id = id
            };

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpGet]
        [RequirePermission(Permissions.ProductsView)]
        [SwaggerOperation(
            Summary = "Get products",
            Description = "Retrieves a paginated list of products with optional search and filters."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetProducts(
            [FromQuery] GetProductsQuery query,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpGet("{id:int}")]
        [RequirePermission(Permissions.ProductsViewDetails)]
        [SwaggerOperation(
            Summary = "Get product by ID",
            Description = "Retrieves a product by ID."
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
            var query = new GetProductByIdQuery
            {
                Id = id
            };

            var result = await mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpPatch("{id:int}/restore")]
        [RequirePermission(Permissions.ProductsRestore)]
        [SwaggerOperation(
            Summary = "Restore product",
            Description = "Restores a previously soft-deleted product."
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
            var command = new RestoreProductCommand
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
 