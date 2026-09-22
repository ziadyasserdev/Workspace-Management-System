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

namespace Workspace_Management_System.Application.Features.Workspaces.Commands.UpdateWorkspace
{
    public class UpdateWorkspaceCommandHandler
          : IRequestHandler<UpdateWorkspaceCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateWorkspaceCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            UpdateWorkspaceCommand request,
            CancellationToken cancellationToken)
        {
            var workspace = await _unitOfWork.Workspaces
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id &&
                         !x.IsDeleted,
                    cancellationToken);

            if (workspace is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Workspace not found.");
            }

            var workspaceType = await _unitOfWork.WorkspaceTypes
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.WorkspaceTypeId &&
                         !x.IsDeleted,
                    cancellationToken);

            if (workspaceType is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Workspace type not found.");
            }

            if (!workspaceType.IsActive)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Cannot assign workspace to an inactive workspace type.");
            }

            var code = request.Code.Trim();

            var codeExists = await _unitOfWork.Workspaces
                .Query()
                .AnyAsync(
                    x => x.Id != request.Id &&
                         x.Code == code &&
                         !x.IsDeleted,
                    cancellationToken);

            if (codeExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Workspace code already exists.");
            }

            workspace.WorkspaceTypeId = request.WorkspaceTypeId;
            workspace.Name = request.Name.Trim();
            workspace.Code = code;
            workspace.Floor = request.Floor?.Trim();
            workspace.Location = request.Location?.Trim();
            workspace.Capacity = request.Capacity;
            workspace.Description = request.Description?.Trim();

            workspace.UpdatedAt = DateTime.UtcNow;
            workspace.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Workspace updated successfully.");
        }
    }
}
