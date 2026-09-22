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

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Commands.UpdateWorkspaceType
{
    public class UpdateWorkspaceTypeCommandHandler
       : IRequestHandler<UpdateWorkspaceTypeCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateWorkspaceTypeCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            UpdateWorkspaceTypeCommand request,
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

            var name = request.Name.Trim();

            var exists = await _unitOfWork.WorkspaceTypes
                .Query()
                .AnyAsync(
                    x => x.Id != request.Id &&
                         x.Name == name &&
                         !x.IsDeleted,
                    cancellationToken);

            if (exists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Workspace type with the same name already exists.");
            }

            workspaceType.Name = name;
            workspaceType.Description = request.Description?.Trim();
            workspaceType.UpdatedAt = DateTime.UtcNow;
            workspaceType.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Workspace type updated successfully.");
        }
    }
}
