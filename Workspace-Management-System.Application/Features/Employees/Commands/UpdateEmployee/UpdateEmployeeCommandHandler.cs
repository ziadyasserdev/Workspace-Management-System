using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler
       : IRequestHandler<UpdateEmployeeCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateEmployeeCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(
            UpdateEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Employees
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.EmployeeId &&
                         !x.IsDeleted,
                    cancellationToken);

            if (employee is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Employee not found.");
            }

        
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var email = request.Email.Trim();

                var emailExists = await _unitOfWork.Employees
                    .Query()
                    .AnyAsync(
                        x => x.Id != request.EmployeeId &&
                             !x.IsDeleted &&
                             x.Email != null &&
                             x.Email.ToLower() == email.ToLower(),
                        cancellationToken);

                if (emailExists)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Conflict,
                        "Email already exists.");
                }
            }

            // Check duplicate phone
            var phone = request.Phone.Trim();

            var phoneExists = await _unitOfWork.Employees
                .Query()
                .AnyAsync(
                    x => x.Id != request.EmployeeId &&
                         !x.IsDeleted &&
                         x.Phone == phone,
                    cancellationToken);

            if (phoneExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Phone number already exists.");
            }

            // Update employee
            employee.FirstName = request.FirstName.Trim();
            employee.LastName = request.LastName.Trim();
            employee.Phone = phone;

            employee.Email = string.IsNullOrWhiteSpace(request.Email)
                ? null
                : request.Email.Trim();

            employee.UpdatedAt = DateTime.UtcNow;
            employee.UpdatedBy = _currentUserService.UserId;

            _unitOfWork.Employees.Update(employee);

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Employee updated successfully.");
        }
    }
}
