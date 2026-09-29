using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Discounts.Dtos;

namespace Workspace_Management_System.Application.Features.Discounts.Queries.GetDiscountById
{
    public class GetDiscountByIdQueryHandler
        : IRequestHandler<GetDiscountByIdQuery, Result<DiscountResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDiscountByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<DiscountResponseDto>> Handle(
            GetDiscountByIdQuery request,
            CancellationToken cancellationToken)
        {
            var discount = await _unitOfWork.Discounts
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.IsDeleted,
                    cancellationToken);

            if (discount is null)
            {
                return Result<DiscountResponseDto>.Failure(
                    ResultStatus.NotFound,
                    "Discount not found.");
            }

            var dto = new DiscountResponseDto
            {
                Id = discount.Id,
                Name = discount.Name,
                Type = discount.DiscountType,
                Value = discount.Value,
                IsActive = discount.IsActive,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate
            };

            return Result<DiscountResponseDto>.Success(dto);
        }
    }
}