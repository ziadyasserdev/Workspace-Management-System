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

namespace Workspace_Management_System.Application.Features.Employees.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommandHandler
         : IRequestHandler<DeleteEmployeeCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public DeleteEmployeeCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            DeleteEmployeeCommand request,
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

            var now = DateTime.UtcNow;
            var currentUserId = _currentUserService.UserId;

            employee.IsDeleted = true;
            employee.IsDeletedBy = currentUserId;
            employee.UpdatedAt = now;
            employee.UpdatedBy = currentUserId;

            _unitOfWork.Employees.Update(employee);

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["EmployeeDeletedSuccessfully"].Value);
        }
    }
}
