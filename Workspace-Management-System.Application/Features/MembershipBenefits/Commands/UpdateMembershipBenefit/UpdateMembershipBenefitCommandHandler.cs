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

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Commands.UpdateMembershipBenefit
{
    public class UpdateMembershipBenefitCommandHandler
        : IRequestHandler<UpdateMembershipBenefitCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public UpdateMembershipBenefitCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            UpdateMembershipBenefitCommand request,
            CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return Result<bool>.Failure(
                    ResultStatus.Unauthorized,
                    _localizer["UserNotAuthenticated"].Value);
            }

            var membership = await _unitOfWork.Memberships
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.MembershipId && !x.IsDeleted,
                    cancellationToken);

            if (membership == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["MembershipNotFound"].Value);
            }

            if (!membership.IsActive)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["CannotUpdateBenefitsOfInactiveMembership"].Value);
            }

            var benefit = await _unitOfWork.MembershipBenefits
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.BenefitId
                         && x.MembershipId == request.MembershipId
                         && !x.IsDeleted,
                    cancellationToken);

            if (benefit == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["MembershipBenefitNotFound"].Value);
            }

            if (request.WorkspaceTypeId.HasValue)
            {
                var workspaceType = await _unitOfWork.WorkspaceTypes
                    .Query()
                    .FirstOrDefaultAsync(
                        x => x.Id == request.WorkspaceTypeId.Value
                             && !x.IsDeleted,
                        cancellationToken);

                if (workspaceType == null)
                {
                    return Result<bool>.Failure(
                        ResultStatus.NotFound,
                        _localizer["WorkspaceTypeNotFound"].Value);
                }
            }

            benefit.BenefitType = request.BenefitType;
            benefit.WorkspaceTypeId = request.WorkspaceTypeId;
            benefit.Hours = request.Hours;
            benefit.DiscountPercentage = request.DiscountPercentage;

            benefit.DescriptionEn =
                string.IsNullOrWhiteSpace(request.DescriptionEn)
                    ? null
                    : request.DescriptionEn.Trim();

            benefit.DescriptionAr =
                string.IsNullOrWhiteSpace(request.DescriptionAr)
                    ? null
                    : request.DescriptionAr.Trim();

            benefit.UpdatedAt = DateTime.UtcNow;
            benefit.UpdatedBy = currentUserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["MembershipBenefitUpdatedSuccessfully"].Value);
        }
    }


}
