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

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Commands.RestoreWorkspaceType
{
    public class RestoreWorkspaceTypeCommandHandler
         : IRequestHandler<RestoreWorkspaceTypeCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer _localizer;

        public RestoreWorkspaceTypeCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IStringLocalizerFactory factory)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _localizer = factory.Create(typeof(SharedResources));
        }

        public async Task<Result<bool>> Handle(
            RestoreWorkspaceTypeCommand request,
            CancellationToken cancellationToken)
        {
            var workspaceType = await _unitOfWork.WorkspaceTypes
                .Query()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && x.IsDeleted,
                    cancellationToken);

            if (workspaceType is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["DeletedWorkspaceTypeNotFound"]);
            }

            workspaceType.IsDeleted = false;
            workspaceType.IsDeletedBy = null;
            workspaceType.UpdatedAt = DateTime.UtcNow;
            workspaceType.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["WorkspaceTypeRestoredSuccessfully"]);
        }
    }
}
