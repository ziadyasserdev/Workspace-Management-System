using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Memberships.Dtos;

namespace Workspace_Management_System.Application.Features.Memberships.Queries.GetMembershipById
{
    public class GetMembershipByIdQueryHandler
      : IRequestHandler<
          GetMembershipByIdQuery,
          Result<MembershipDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMembershipByIdQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<MembershipDetailsDto>> Handle(
            GetMembershipByIdQuery request,
            CancellationToken cancellationToken)
        {
            var membership = await _unitOfWork.Memberships
                .Query()
                .AsNoTracking()
                .Where(x =>
                    x.Id == request.Id &&
                    !x.IsDeleted)
                .Select(x => new MembershipDetailsDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    DurationDays = x.DurationDays,
                    Price = x.Price,
                    IsActive = x.IsActive,

                    Benefits = x.Benefits
                        .Where(b => !b.IsDeleted)
                        .Select(b => new MembershipBenefitDto
                        {
                            Id = b.Id,

                            BenefitType = b.BenefitType,

                            WorkspaceTypeId = b.WorkspaceTypeId,

                            WorkspaceTypeName =
                                b.WorkspaceType != null
                                    ? b.WorkspaceType.Name
                                    : null,

                            Hours = b.Hours,

                            DiscountPercentage =
                                b.DiscountPercentage,

                            Description = b.Description
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (membership == null)
            {
                return Result<MembershipDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    "Membership not found.");
            }

            return Result<MembershipDetailsDto>.Success(
                membership);
        }
    }
}
