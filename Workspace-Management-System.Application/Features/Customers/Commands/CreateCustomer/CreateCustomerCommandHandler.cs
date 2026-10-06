using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler
    : IRequestHandler<CreateCustomerCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer _localizer;

    public CreateCustomerCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<int>> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var checkExist = await _unitOfWork.Customers
            .Query()
            .AnyAsync(
                x =>
                    x.MobileNumber == request.MobileNumber &&
                    !x.IsDeleted,
                cancellationToken);

        if (checkExist)
        {
            return Result<int>.Failure(
                ResultStatus.Conflict,
                _localizer["CustomerSameMobileNumberAlreadyExists"]);
        }

        var now = DateTime.UtcNow;

        var customer = new Customer
        {
            FullNameEn = request.FullNameEn.Trim(),
            FullNameAr = request.FullNameAr.Trim(),
            MobileNumber = request.MobileNumber,
            Email = request.Email,
            CompanyId = request.CompanyId,
            CustomerType = request.CustomerType,
            NotesEn = request.NotesEn?.Trim(),
            NotesAr = request.NotesAr?.Trim(),
            RegistrationDate = now,
            Status = CustomerStatus.Active,
            CreatedAt = now,
            CreatedBy = _currentUser.UserId
        };

        await _unitOfWork.Customers.AddAsync(customer);

        await _unitOfWork.SaveAsync();

        return Result<int>.Success(
            customer.Id,
            _localizer["CustomerCreatedSuccessfully"]);
    }
}
