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

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Commands.ChangeWorkspaceTypeStatus
{
    public class ChangeWorkspaceTypeStatusCommandHandler
        : IRequestHandler<
            ChangeWorkspaceTypeStatusCommand,
            Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public ChangeWorkspaceTypeStatusCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            ChangeWorkspaceTypeStatusCommand request,
            CancellationToken cancellationToken)
        {
            var workspaceType = await _unitOfWork.WorkspaceTypes
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id &&
                         !x.IsDeleted,
                    cancellationToken);

            if (workspaceType is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Workspace type not found.");
            }

            if (workspaceType.IsActive == request.IsActive)
            {
                return Result<bool>.Success(
                    true,
                    request.IsActive
                        ? "Workspace type is already active."
                        : "Workspace type is already inactive.");
            }

            workspaceType.IsActive = request.IsActive;
            workspaceType.UpdatedAt = DateTime.UtcNow;
            workspaceType.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                request.IsActive
                    ? "Workspace type activated successfully."
                    : "Workspace type deactivated successfully.");
        }
    }
}
