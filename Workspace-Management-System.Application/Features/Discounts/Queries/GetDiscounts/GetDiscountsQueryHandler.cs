using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Discounts.Dtos;

namespace Workspace_Management_System.Application.Features.Discounts.Queries.GetDiscounts
{
    public class GetDiscountsQueryHandler
        : IRequestHandler<
            GetDiscountsQuery,
            Result<List<DiscountResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDiscountsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<DiscountResponseDto>>> Handle(
            GetDiscountsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Discounts
                .Query()
                .Where(x => !x.IsDeleted);

            if (request.IsActive.HasValue)
            {
                query = query.Where(
                    x => x.IsActive == request.IsActive.Value);
            }

            var discounts = await query
                .OrderBy(x => x.Name)
                .Select(x => new DiscountResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Type = x.DiscountType,
                    Value = x.Value,
                    IsActive = x.IsActive,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                })
                .ToListAsync(cancellationToken);

            return Result<List<DiscountResponseDto>>.Success(
                discounts);
        }
    }
}