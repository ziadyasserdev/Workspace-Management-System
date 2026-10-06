using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Customers.Commands.Delete_Customer;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandHandler
    : IRequestHandler<DeleteCustomerCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer _localizer;

    public DeleteCustomerCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<bool>> Handle(
        DeleteCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (customer is null)
        {
            return Result<bool>.Failure(
                ResultStatus.NotFound,
                _localizer["CustomerNotFound"]);
        }

        if (customer.IsDeleted)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                _localizer["CustomerAlreadyDeleted"]);
        }

        customer.IsDeleted = true;
        customer.IsDeletedBy = _currentUser.UserId;
        customer.UpdatedAt = DateTime.UtcNow;
        customer.UpdatedBy = _currentUser.UserId;

        await _unitOfWork.SaveAsync();

        return Result<bool>.Success(
            true,
            _localizer["CustomerDeletedSuccessfully"]);
    }
}