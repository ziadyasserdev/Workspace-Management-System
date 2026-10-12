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

namespace Workspace_Management_System.Application.Features.Employees.Commands.RestoreEmployee
{
    public class RestoreEmployeeCommandHandler
    : IRequestHandler<RestoreEmployeeCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public RestoreEmployeeCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            RestoreEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Employees
                .Query()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(
                    x => x.Id == request.EmployeeId && x.IsDeleted,
                    cancellationToken);

            if (employee is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["DeletedEmployeeNotFound"].Value);
            }

            employee.IsDeleted = false;
            employee.IsDeletedBy = null;
            employee.UpdatedAt = DateTime.UtcNow;
            employee.UpdatedBy = _currentUserService.UserId;

            _unitOfWork.Employees.Update(employee);

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["EmployeeRestoredSuccessfully"].Value);
        }
    }
}
