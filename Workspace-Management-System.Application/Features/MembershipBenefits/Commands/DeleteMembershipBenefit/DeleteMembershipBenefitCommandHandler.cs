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

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Commands.DeleteMembershipBenefit
{
    public class DeleteMembershipBenefitCommandHandler
          : IRequestHandler<DeleteMembershipBenefitCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public DeleteMembershipBenefitCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            DeleteMembershipBenefitCommand request,
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

            var now = DateTime.UtcNow;

            benefit.IsDeleted = true;
            benefit.IsDeletedBy = currentUserId;
            benefit.UpdatedAt = now;
            benefit.UpdatedBy = currentUserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["MembershipBenefitDeletedSuccessfully"].Value);
        }
    }
}
