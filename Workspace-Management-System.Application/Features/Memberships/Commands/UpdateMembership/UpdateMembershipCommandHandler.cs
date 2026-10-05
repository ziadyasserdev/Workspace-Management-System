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

namespace Workspace_Management_System.Application.Features.Memberships.Commands.UpdateMembership
{
    public class UpdateMembershipCommandHandler
    : IRequestHandler<UpdateMembershipCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateMembershipCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
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
                    "User is not authenticated.");
            }

            var membership = await _unitOfWork.Memberships
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id &&
                         !x.IsDeleted,
                    cancellationToken);

            if (membership == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Membership not found.");
            }

            var normalizedName = request.Name.Trim();

            var duplicateNameExists = await _unitOfWork.Memberships
                .Query()
                .AnyAsync(
                    x => !x.IsDeleted &&
                         x.Id != request.Id &&
                         x.Name.ToLower() == normalizedName.ToLower(),
                    cancellationToken);

            if (duplicateNameExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "A membership with the same name already exists.");
            }

            membership.Name = normalizedName;

            membership.Description =
                string.IsNullOrWhiteSpace(request.Description)
                    ? null
                    : request.Description.Trim();

            membership.DurationDays = request.DurationDays;

            membership.Price = request.Price;

            membership.UpdatedAt = DateTime.UtcNow;
            membership.UpdatedBy = currentUserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Membership updated successfully.");
        }
    }
}
