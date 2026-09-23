using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Employees.Commands.AssignEmployeeToWorkspace
{
    public class AssignEmployeeToWorkspaceCommandHandler
     : IRequestHandler<AssignEmployeeToWorkspaceCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AssignEmployeeToWorkspaceCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(
            AssignEmployeeToWorkspaceCommand request,
            CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Employees
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.EmployeeId &&
                         !x.IsDeleted,
                    cancellationToken);

            if (employee is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Employee not found.");
            }

            var workspace = await _unitOfWork.Workspaces
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.WorkspaceId &&
                         !x.IsDeleted
                         &&x.IsActive,
                    cancellationToken);

            if (workspace is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Workspace not found.");
            }

            if (!workspace.IsActive)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Workspace is inactive.");
            }

            if (employee.WorkspaceId == request.WorkspaceId)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Employee is already assigned to this workspace.");
            }

            var now = DateTime.UtcNow;

            employee.WorkspaceId = request.WorkspaceId;
            employee.UpdatedAt = now;
            employee.UpdatedBy = _currentUserService.UserId;

      

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Employee assigned to workspace successfully.");
        }
    }
}
