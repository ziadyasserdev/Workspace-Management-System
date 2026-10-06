using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Discounts.Dtos;

namespace Workspace_Management_System.Application.Features.Discounts.Queries.GetDiscounts
{
    public class GetDiscountsQueryHandler
        : IRequestHandler<
            GetDiscountsQuery,
            Result<PaginatedResult<DiscountResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDiscountsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<DiscountResponseDto>>> Handle(
            GetDiscountsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Discounts
                .Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    x.NameEn.Contains(search) ||
                    x.NameAr.Contains(search) ||
                    (x.DescriptionEn != null &&
                     x.DescriptionEn.Contains(search)) ||
                    (x.DescriptionAr != null &&
                     x.DescriptionAr.Contains(search)));
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(
                    x => x.IsActive == request.IsActive.Value);
            }

            var totalCount = await query.CountAsync(
                cancellationToken);

            var items = await query
                .OrderBy(x => x.NameEn)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new DiscountResponseDto
                {
                    Id = x.Id,
                    NameEn = x.NameEn,
                    NameAr = x.NameAr,
                    DescriptionEn = x.DescriptionEn,
                    DescriptionAr = x.DescriptionAr,
                    Type = x.DiscountType,
                    Value = x.Value,
                    IsActive = x.IsActive,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<DiscountResponseDto>(
                items,
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result<PaginatedResult<DiscountResponseDto>>.Success(
                result,
                "Discounts retrieved successfully.");
        }
    }
}