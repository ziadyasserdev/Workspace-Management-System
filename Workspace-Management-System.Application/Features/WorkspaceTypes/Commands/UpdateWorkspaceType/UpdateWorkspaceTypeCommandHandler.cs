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

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Commands.UpdateWorkspaceType
{
    public class UpdateWorkspaceTypeCommandHandler
      : IRequestHandler<UpdateWorkspaceTypeCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer _localizer;

        public UpdateWorkspaceTypeCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IStringLocalizerFactory factory)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _localizer = factory.Create(typeof(SharedResources));
        }

        public async Task<Result<bool>> Handle(
            UpdateWorkspaceTypeCommand request,
            CancellationToken cancellationToken)
        {
            var workspaceType = await _unitOfWork.WorkspaceTypes
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.IsDeleted,
                    cancellationToken);

            if (workspaceType is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["WorkspaceTypeNotFound"]);
            }

            var nameEn = request.NameEn.Trim();
            var nameAr = request.NameAr.Trim();

            var exists = await _unitOfWork.WorkspaceTypes
                .Query()
                .AnyAsync(
                    x => x.Id != request.Id &&
                         !x.IsDeleted &&
                         (x.NameEn == nameEn || x.NameAr == nameAr),
                    cancellationToken);

            if (exists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["WorkspaceTypeAlreadyExists"]);
            }

            workspaceType.NameEn = nameEn;
            workspaceType.NameAr = nameAr;
            workspaceType.DescriptionEn = request.DescriptionEn?.Trim();
            workspaceType.DescriptionAr = request.DescriptionAr?.Trim();

            workspaceType.UpdatedAt = DateTime.UtcNow;
            workspaceType.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["WorkspaceTypeUpdatedSuccessfully"]);
        }
    }
}
