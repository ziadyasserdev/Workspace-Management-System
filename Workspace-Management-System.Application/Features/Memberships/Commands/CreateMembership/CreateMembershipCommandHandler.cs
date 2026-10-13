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
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Memberships.Commands.CreateMembership
{
    public class CreateMembershipCommandHandler
        : IRequestHandler<CreateMembershipCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CreateMembershipCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(
            CreateMembershipCommand request,
            CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    _localizer["UserNotAuthenticated"].Value);
            }

            var normalizedNameEn = request.NameEn.Trim();
            var normalizedNameAr = request.NameAr.Trim();

            var membershipExists = await _unitOfWork.Memberships
                .Query()
                .AnyAsync(
                    x => !x.IsDeleted &&
                         (x.NameEn.ToLower() == normalizedNameEn.ToLower() ||
                          x.NameAr == normalizedNameAr),
                    cancellationToken);

            if (membershipExists)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    _localizer["MembershipNameAlreadyExists"].Value);
            }

            var now = DateTime.UtcNow;

            var membership = new Membership
            {
                NameEn = normalizedNameEn,
                NameAr = normalizedNameAr,

                DescriptionEn = string.IsNullOrWhiteSpace(request.DescriptionEn)
                    ? null
                    : request.DescriptionEn.Trim(),

                DescriptionAr = string.IsNullOrWhiteSpace(request.DescriptionAr)
                    ? null
                    : request.DescriptionAr.Trim(),

                DurationDays = request.DurationDays,
                Price = request.Price,
                IsActive = true,

                CreatedAt = now,
                CreatedBy = currentUserId
            };

            await _unitOfWork.Memberships.AddAsync(membership);
            await _unitOfWork.SaveAsync();

            return Result<int>.Success(
                membership.Id,
                _localizer["MembershipCreatedSuccessfully"].Value);
        }
    }
}
