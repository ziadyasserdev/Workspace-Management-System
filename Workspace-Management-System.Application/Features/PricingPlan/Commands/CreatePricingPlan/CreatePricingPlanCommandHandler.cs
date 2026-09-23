using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using PricingPlanModel = Workspace_Management_System.Domain.Models.PricingPlan;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.CreatePricingPlan
{
    public class CreatePricingPlanCommandHandler
        : IRequestHandler<CreatePricingPlanCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;

        public CreatePricingPlanCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
        }

        public async Task<Result<int>> Handle(
            CreatePricingPlanCommand request,
            CancellationToken cancellationToken)
        {
            var checkExist = await unitOfWork.PricingPlans
                .Query()
                .AnyAsync(
                    x => x.Name == request.Name && !x.IsDeleted,
                    cancellationToken);

            if (checkExist)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "A pricing plan with the same name already exists.");
            }

            var pricingPlan = new PricingPlanModel
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.UserId
            };

            await unitOfWork.PricingPlans.AddAsync(pricingPlan);

            await unitOfWork.SaveAsync();

            return Result<int>.Success(
                pricingPlan.Id,
                "Pricing plan created successfully.");
        }
    }
}