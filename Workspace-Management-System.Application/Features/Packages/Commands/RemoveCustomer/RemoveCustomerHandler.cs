using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Packages.Commands.RemoveCustomer;

public class RemoveCustomerHandler
    : IRequestHandler<RemoveCustomerCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer _localizer;

    public RemoveCustomerHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<int>> Handle(
        RemoveCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var customerPackage =
            await _unitOfWork.CustomerPackages
                .Query()
                .FirstOrDefaultAsync(
                    x =>
                        x.PackageId == request.PackageId &&
                        x.CustomerId == request.CustomerId &&
                        x.Status == CustomerPackageStatus.Active &&
                        !x.IsDeleted,
                    cancellationToken);

        if (customerPackage is null)
        {
            return Result<int>.Failure(
                ResultStatus.NotFound,
                _localizer["ActiveCustomerPackageNotFound"]);
        }

        customerPackage.Status =
            CustomerPackageStatus.Cancelled;

        customerPackage.EndDate = DateTime.UtcNow;

        customerPackage.UpdatedAt = DateTime.UtcNow;
        customerPackage.UpdatedBy = _currentUser.UserId;

        _unitOfWork.CustomerPackages.Update(customerPackage);

        await _unitOfWork.SaveAsync();

        return Result<int>.Success(
            customerPackage.Id,
            _localizer["CustomerRemovedFromPackageSuccessfully"]);
    }
}
