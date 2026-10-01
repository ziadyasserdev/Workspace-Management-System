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
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Memberships.Commands.CreateMembership
{
    public class CreateMembershipCommandHandler
      : IRequestHandler<CreateMembershipCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateMembershipCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
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
                    "User is not authenticated.");
            }

            var normalizedName = request.Name.Trim();

            var membershipExists = await _unitOfWork.Memberships
                .Query()
                .AnyAsync(
                    x => !x.IsDeleted &&
                         x.Name.ToLower() == normalizedName.ToLower(),
                    cancellationToken);

            if (membershipExists)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "A membership with the same name already exists.");
            }

            var now = DateTime.UtcNow;

            var membership = new Membership
            {
                Name = normalizedName,
                Description = string.IsNullOrWhiteSpace(request.Description)
                    ? null
                    : request.Description.Trim(),

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
                "Membership created successfully.");
        }
    }
}
