using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler
       : IRequestHandler<UpdateEmployeeCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public UpdateEmployeeCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            UpdateEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Employees
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.EmployeeId && !x.IsDeleted,
                    cancellationToken);

            if (employee is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["EmployeeNotFound"].Value);
            }

            var phone = request.Phone.Trim();

            var phoneExists = await _unitOfWork.Employees
                .Query()
                .AnyAsync(
                    x => x.Id != request.EmployeeId
                         && !x.IsDeleted
                         && x.Phone == phone,
                    cancellationToken);

            if (phoneExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["EmployeePhoneAlreadyExists"].Value);
            }

            string? email = string.IsNullOrWhiteSpace(request.Email)
                ? null
                : request.Email.Trim();

            if (email is not null)
            {
                var normalizedEmail = email.ToLower();

                var emailExists = await _unitOfWork.Employees
                    .Query()
                    .AnyAsync(
                        x => x.Id != request.EmployeeId
                             && !x.IsDeleted
                             && x.Email != null
                             && x.Email.ToLower() == normalizedEmail,
                        cancellationToken);

                if (emailExists)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Conflict,
                        _localizer["EmployeeEmailAlreadyExists"].Value);
                }
            }

            employee.FirstNameEn = request.FirstNameEn.Trim();
            employee.FirstNameAr = request.FirstNameAr.Trim();
            employee.LastNameEn = request.LastNameEn.Trim();
            employee.LastNameAr = request.LastNameAr.Trim();

            employee.Phone = phone;
            employee.Email = email;

            employee.UpdatedAt = DateTime.UtcNow;
            employee.UpdatedBy = _currentUserService.UserId;

            _unitOfWork.Employees.Update(employee);

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["EmployeeUpdatedSuccessfully"].Value);
        }
    }
}
