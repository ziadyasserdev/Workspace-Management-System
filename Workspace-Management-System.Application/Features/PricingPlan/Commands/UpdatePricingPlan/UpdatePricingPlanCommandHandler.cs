using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.UpdatePricingPlan
{
    public class UpdatePricingPlanCommandHandler
        : IRequestHandler<UpdatePricingPlanCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer _localizer;

        public UpdatePricingPlanCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IStringLocalizerFactory factory)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _localizer = factory.Create(typeof(SharedResources));
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
                    _localizer["PricingPlanNotFound"]);
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
                    _localizer["PricingPlanWithSameNameAlreadyExists"]);
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
                _localizer["PricingPlanUpdatedSuccessfully"]);
        }
    }
}