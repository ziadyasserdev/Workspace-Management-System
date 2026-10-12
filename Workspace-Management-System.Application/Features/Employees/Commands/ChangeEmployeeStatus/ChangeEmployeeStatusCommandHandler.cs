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

namespace Workspace_Management_System.Application.Features.Employees.Commands.ChangeEmployeeStatus
{

    public class ChangeEmployeeStatusCommandHandler
        : IRequestHandler<ChangeEmployeeStatusCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ChangeEmployeeStatusCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            ChangeEmployeeStatusCommand request,
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

            if (employee.Status == request.Status)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["EmployeeAlreadyHasThisStatus"].Value);
            }

            employee.Status = request.Status;
            employee.UpdatedAt = DateTime.UtcNow;
            employee.UpdatedBy = _currentUserService.UserId;

            _unitOfWork.Employees.Update(employee);

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["EmployeeStatusChangedSuccessfully"].Value);
        }
    }
}
