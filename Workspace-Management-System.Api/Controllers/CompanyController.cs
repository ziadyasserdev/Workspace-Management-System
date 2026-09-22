using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.Companies.Commands.CreateCompany;
using Workspace_Management_System.Application.Features.Companies.Commands.DeleteCompany;
using Workspace_Management_System.Application.Features.Companies.Commands.UpdateCompany;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator mediator;

        public CompanyController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create company",
            Description = "Creates a new company."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(
            [FromBody] CreateCompanyCommand command,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpPut("{id}")]
        [SwaggerOperation(
    Summary = "Update company",
    Description = "Updates an existing company."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateCompanyCommand command,
    CancellationToken cancellationToken)
        {
            command.Id = id;

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Delete company",
    Description = "Soft deletes an existing company."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(
    int id,
    CancellationToken cancellationToken)
        {
            var command = new DeleteCompanyCommand
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