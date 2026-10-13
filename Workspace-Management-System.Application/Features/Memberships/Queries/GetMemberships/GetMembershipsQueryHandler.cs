using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Memberships.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Memberships.Queries.GetMemberships
{
    public class GetMembershipsQueryHandler
         : IRequestHandler<
             GetMembershipsQuery,
             Result<PaginatedResult<MembershipListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMembershipsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<MembershipListDto>>> Handle(
            GetMembershipsQuery request,
            CancellationToken cancellationToken)
        {
            var isArabic =
                CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

            var query = _unitOfWork.Memberships
                .Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            // Search in the selected language only.
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = isArabic
                    ? query.Where(x =>
                        x.NameAr.Contains(search) ||
                        (x.DescriptionAr != null &&
                         x.DescriptionAr.Contains(search)))
                    : query.Where(x =>
                        x.NameEn.Contains(search) ||
                        (x.DescriptionEn != null &&
                         x.DescriptionEn.Contains(search)));
            }

            // Filter by active status.
            if (request.IsActive.HasValue)
            {
                query = query.Where(x =>
                    x.IsActive == request.IsActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            // Sort by creation date.
            query = request.SortDescending
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt);

            // Return only the selected language.
            var items = await query
                .Select(x => new MembershipListDto
                {
                    Id = x.Id,

                    Name = isArabic ? x.NameAr : x.NameEn,

                    Description = isArabic
                        ? x.DescriptionAr
                        : x.DescriptionEn,

                    DurationDays = x.DurationDays,
                    Price = x.Price,
                    IsActive = x.IsActive,

                    BenefitsCount = x.Benefits
                        .Count(b => !b.IsDeleted),

                    ActiveCustomerMembershipsCount =
                        x.CustomerMemberships.Count(cm =>
                            !cm.IsDeleted &&
                            cm.Status == CustomerMembershipStatus.Active)
                })
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<MembershipListDto>(
                items,
                totalCount,
                request.PageNumber,
                request.PageSize);

            return Result<PaginatedResult<MembershipListDto>>
                .Success(result);
        }
    }
}
