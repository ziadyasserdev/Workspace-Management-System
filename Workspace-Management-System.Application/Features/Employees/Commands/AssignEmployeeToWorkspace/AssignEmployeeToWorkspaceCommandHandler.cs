using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Employees.Commands.AssignEmployeeToWorkspace
{
    public class AssignEmployeeToWorkspaceCommandHandler
       : IRequestHandler<AssignEmployeeToWorkspaceCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AssignEmployeeToWorkspaceCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            AssignEmployeeToWorkspaceCommand request,
            CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Employees
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.EmployeeId && !x.IsDeleted,
                    cancellationToken);

            if (employee is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["EmployeeNotFound"].Value);
            }

            var workspace = await _unitOfWork.Workspaces
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.WorkspaceId && !x.IsDeleted,
                    cancellationToken);

            if (workspace is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["WorkspaceNotFound"].Value);
            }

            if (!workspace.IsActive)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["WorkspaceIsInactive"].Value);
            }

            if (employee.WorkspaceId == request.WorkspaceId)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["EmployeeAlreadyAssignedToWorkspace"].Value);
            }

            var now = DateTime.UtcNow;

            employee.WorkspaceId = request.WorkspaceId;
            employee.UpdatedAt = now;
            employee.UpdatedBy = _currentUserService.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["EmployeeAssignedToWorkspaceSuccessfully"].Value);
        }
    }
}
