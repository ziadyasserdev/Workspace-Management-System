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

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Commands.CreateMembershipBenefit
{
    public class CreateMembershipBenefitCommandHandler
      : IRequestHandler<
          CreateMembershipBenefitCommand,
          Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateMembershipBenefitCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(
            CreateMembershipBenefitCommand request,
            CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

           
            var membership = await _unitOfWork.Memberships
                .Query()
                .FirstOrDefaultAsync(
                    x =>
                        x.Id == request.MembershipId &&
                        !x.IsDeleted,
                    cancellationToken);

            if (membership == null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Membership not found.");
            }

          
            if (!membership.IsActive)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Cannot add benefits to an inactive membership.");
            }

     
            if (request.WorkspaceTypeId.HasValue)
            {
                var workspaceType = await _unitOfWork.WorkspaceTypes
                    .Query()
                    .FirstOrDefaultAsync(
                        x =>
                            x.Id == request.WorkspaceTypeId.Value &&
                            !x.IsDeleted,
                        cancellationToken);

                if (workspaceType == null)
                {
                    return Result<int>.Failure(
                        ResultStatus.NotFound,
                        "Workspace type not found.");
                }
            }

          
            var now = DateTime.UtcNow;

            var benefit = new MembershipBenefit
            {
                MembershipId = request.MembershipId,

                BenefitType = request.BenefitType,

                WorkspaceTypeId = request.WorkspaceTypeId,

                Hours = request.Hours,

                DiscountPercentage =
                    request.DiscountPercentage,

                Description =
                    string.IsNullOrWhiteSpace(request.Description)
                        ? null
                        : request.Description.Trim(),

                CreatedAt = now,
                CreatedBy = currentUserId
            };

            await _unitOfWork.MembershipBenefits
                .AddAsync(benefit);

            await _unitOfWork.SaveAsync();

            return Result<int>.Success(
                benefit.Id,
                "Membership benefit created successfully.");
        }
    }
}
