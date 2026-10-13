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

namespace Workspace_Management_System.Application.Features.Memberships.Commands.UpdateMembership
{
    public class UpdateMembershipCommandHandler
        : IRequestHandler<UpdateMembershipCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public UpdateMembershipCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            UpdateMembershipCommand request,
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
                    x => x.Id == request.Id && !x.IsDeleted,
                    cancellationToken);

            if (membership == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["MembershipNotFound"].Value);
            }

            var normalizedNameEn = request.NameEn.Trim();
            var normalizedNameAr = request.NameAr.Trim();

            var duplicateNameExists = await _unitOfWork.Memberships
                .Query()
                .AnyAsync(
                    x => !x.IsDeleted &&
                         x.Id != request.Id &&
                         (x.NameEn.ToLower() == normalizedNameEn.ToLower() ||
                          x.NameAr == normalizedNameAr),
                    cancellationToken);

            if (duplicateNameExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["MembershipNameAlreadyExists"].Value);
            }

            membership.NameEn = normalizedNameEn;
            membership.NameAr = normalizedNameAr;

            membership.DescriptionEn =
                string.IsNullOrWhiteSpace(request.DescriptionEn)
                    ? null
                    : request.DescriptionEn.Trim();

            membership.DescriptionAr =
                string.IsNullOrWhiteSpace(request.DescriptionAr)
                    ? null
                    : request.DescriptionAr.Trim();

            membership.DurationDays = request.DurationDays;
            membership.Price = request.Price;

            membership.UpdatedAt = DateTime.UtcNow;
            membership.UpdatedBy = currentUserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["MembershipUpdatedSuccessfully"].Value);
        }
    }
}
