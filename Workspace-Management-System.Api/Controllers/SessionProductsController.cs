using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Application.Features.SessionProducts.Commands.AddProduct;
using Workspace_Management_System.Application.Features.SessionProducts.Commands.ClearProducts;
using Workspace_Management_System.Application.Features.SessionProducts.Commands.RemoveProduct;
using Workspace_Management_System.Application.Features.SessionProducts.Commands.UpdateProductQuantity;
using Workspace_Management_System.Application.Features.SessionProducts.Queries.GetSessionProducts;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/sessions/{sessionId:int}/products")]
    [ApiController]
    public class SessionProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SessionProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get session products",
            Description = "Returns all products currently added to a session."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProducts(
            int sessionId,
            CancellationToken cancellationToken)
        {
            var query = new GetSessionProductsQuery
            {
                SessionId = sessionId
            };

            var result = await _mediator.Send(
                query,
                cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Add product to session",
            Description = "Adds a product to an active session. If the product already exists, its quantity is increased."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddProduct(
            int sessionId,
            [FromBody] AddProductCommand command,
            CancellationToken cancellationToken)
        {
            command.SessionId = sessionId;

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpPut("{productId:int}")]
        [SwaggerOperation(
            Summary = "Update session product quantity",
            Description = "Updates the quantity of a product already added to an active session."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateQuantity(
            int sessionId,
            int productId,
            [FromBody] UpdateProductQuantityCommand command,
            CancellationToken cancellationToken)
        {
            command.SessionId = sessionId;
            command.ProductId = productId;

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{productId:int}")]
        [SwaggerOperation(
            Summary = "Remove product from session",
            Description = "Removes a specific product from an active session."
        )]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Product removed successfully."
        )]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveProduct(
            int sessionId,
            int productId,
            CancellationToken cancellationToken)
        {
            var command = new RemoveProductCommand
            {
                SessionId = sessionId,
                ProductId = productId
            };

            await _mediator.Send(
                command,
                cancellationToken);

            return Ok(new
            {
                message = "Product removed successfully."
            });
        }

        [HttpDelete]
        [SwaggerOperation(
            Summary = "Clear session products",
            Description = "Removes all products currently added to an active session."
        )]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "All products cleared successfully."
        )]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ClearProducts(
            int sessionId,
            CancellationToken cancellationToken)
        {
            var command = new ClearProductsCommand
            {
                SessionId = sessionId
            };

            await _mediator.Send(
                command,
                cancellationToken);

            return Ok(new
            {
                message = "All products cleared successfully."
            });
        }
    }
}