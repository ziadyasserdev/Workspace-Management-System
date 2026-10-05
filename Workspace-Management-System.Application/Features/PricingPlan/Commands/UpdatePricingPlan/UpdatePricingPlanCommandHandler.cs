using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.UpdatePricingPlan
{
    public class UpdatePricingPlanCommandHandler
        : IRequestHandler<UpdatePricingPlanCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdatePricingPlanCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            UpdatePricingPlanCommand request,
            CancellationToken cancellationToken)
        {
            var pricingPlan = await _unitOfWork.PricingPlans
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.IsDeleted,
                    cancellationToken);

            if (pricingPlan == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Pricing plan not found.");
            }

            var nameEn = request.NameEn.Trim();
            var nameAr = request.NameAr.Trim();

            var duplicateName = await _unitOfWork.PricingPlans
                .Query()
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        !x.IsDeleted &&
                        (
                            x.NameEn.ToLower() == nameEn.ToLower() ||
                            x.NameAr.ToLower() == nameAr.ToLower()
                        ),
                    cancellationToken);

            if (duplicateName)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "A pricing plan with the same name already exists.");
            }

            pricingPlan.NameEn = nameEn;
            pricingPlan.NameAr = nameAr;
            pricingPlan.DescriptionEn = request.DescriptionEn?.Trim();
            pricingPlan.DescriptionAr = request.DescriptionAr?.Trim();
            pricingPlan.IsActive = request.IsActive;
            pricingPlan.UpdatedAt = DateTime.UtcNow;
            pricingPlan.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Pricing plan updated successfully.");
        }
    }
}