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
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Workspaces.Commands.RestoreWorkspace
{
    public class RestoreWorkspaceCommandHandler
         : IRequestHandler<RestoreWorkspaceCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public RestoreWorkspaceCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            RestoreWorkspaceCommand request,
            CancellationToken cancellationToken)
        {
            var workspace = await _unitOfWork.Workspaces
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id &&
                         x.IsDeleted,
                    cancellationToken);

            if (workspace is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Deleted workspace not found.");
            }

            var workspaceType = await _unitOfWork.WorkspaceTypes
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == workspace.WorkspaceTypeId &&
                         !x.IsDeleted,
                    cancellationToken);

            if (workspaceType is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Cannot restore workspace because its workspace type no longer exists.");
            }

            if (!workspaceType.IsActive)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Cannot restore workspace because its workspace type is inactive.");
            }

            workspace.IsDeleted = false;
            workspace.IsActive = true;
            workspace.Status = WorkspaceStatus.Available;

            workspace.IsDeletedBy = null;
            workspace.UpdatedAt = DateTime.UtcNow;
            workspace.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Workspace restored successfully.");
        }
    }
}
