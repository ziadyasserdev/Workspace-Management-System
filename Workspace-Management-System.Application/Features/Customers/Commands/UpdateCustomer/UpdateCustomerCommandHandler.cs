
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler
        : IRequestHandler<UpdateCustomerCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer _localizer;

        public UpdateCustomerCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IStringLocalizerFactory factory)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _localizer = factory.Create(typeof(SharedResources));
        }

        public async Task<Result<bool>> Handle(
            UpdateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Customers
                .Query()
                .FirstOrDefaultAsync(
                    x =>
                        x.Id == request.Id &&
                        !x.IsDeleted,
                    cancellationToken);

            if (customer is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["CustomerNotFound"]);
            }

            var exists = await _unitOfWork.Customers
                .Query()
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        x.MobileNumber == request.MobileNumber &&
                        !x.IsDeleted,
                    cancellationToken);

            if (exists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["CustomerSameMobileNumberAlreadyExists"]);
            }

            customer.FullNameEn = request.FullNameEn.Trim();
            customer.FullNameAr = request.FullNameAr.Trim();
            customer.MobileNumber = request.MobileNumber;
            customer.Email = request.Email;
            customer.CompanyId = request.CompanyId;
            customer.CustomerType = request.CustomerType;
            customer.NotesEn = request.NotesEn?.Trim();
            customer.NotesAr = request.NotesAr?.Trim();
            customer.UpdatedAt = DateTime.UtcNow;
            customer.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["CustomerUpdatedSuccessfully"]);
        }
    }
}
