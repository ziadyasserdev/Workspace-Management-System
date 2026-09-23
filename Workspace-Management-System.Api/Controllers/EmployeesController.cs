using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.Employees.Commands.AssignEmployeeToWorkspace;
using Workspace_Management_System.Application.Features.Employees.Commands.ChangeEmployeeStatus;
using Workspace_Management_System.Application.Features.Employees.Commands.DeleteEmployee;
using Workspace_Management_System.Application.Features.Employees.Commands.RestoreEmployee;
using Workspace_Management_System.Application.Features.Employees.Commands.UpdateEmployee;
using Workspace_Management_System.Application.Features.Employees.Queries.GetEmployeeById;
using Workspace_Management_System.Application.Features.Employees.Queries.GetEmployees;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [HttpPatch("{employeeId:int}/workspace")]
        [SwaggerOperation(
        Summary = "Assign employee to workspace",
        Description = "Assigns an employee to a specific workspace."
    )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AssignToWorkspace(
        int employeeId,
        [FromBody] AssignEmployeeToWorkspaceCommand command,
        CancellationToken cancellationToken)
        {
            command.EmployeeId = employeeId;

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpGet("{id:int}")]
        [SwaggerOperation(
    Summary = "Get employee by id",
    Description = "Returns employee details including assigned workspace."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
    int id,
    CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetEmployeeByIdQuery { Id = id },
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpGet]
        [SwaggerOperation(
    Summary = "Get all employees",
    Description = "Returns a paginated list of employees with optional search and filters."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll(
    [FromQuery] GetEmployeesQuery query,
    CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpPut("{employeeId:int}")]
        [SwaggerOperation(
    Summary = "Update employee",
    Description = "Updates the basic information of an employee."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(
    int employeeId,
    [FromBody] UpdateEmployeeCommand command,
    CancellationToken cancellationToken)
        {
            command.EmployeeId = employeeId;

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpPatch("{employeeId:int}/status")]
        [SwaggerOperation(
    Summary = "Change employee status",
    Description = "Changes the current status of an employee."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ChangeStatus(
    int employeeId,
    [FromBody] ChangeEmployeeStatusCommand command,
    CancellationToken cancellationToken)
        {
            command.EmployeeId = employeeId;

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpDelete("{employeeId:int}")]
        [SwaggerOperation(
    Summary = "Delete employee",
    Description = "Soft deletes an employee."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
    int employeeId,
    CancellationToken cancellationToken)
        {
            var command = new DeleteEmployeeCommand
            {
                EmployeeId = employeeId
            };

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpPatch("{employeeId:int}/restore")]
        [SwaggerOperation(
    Summary = "Restore employee",
    Description = "Restores a previously soft-deleted employee."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Restore(
    int employeeId,
    CancellationToken cancellationToken)
        {
            var command = new RestoreEmployeeCommand
            {
                EmployeeId = employeeId
            };

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
    }
}
