using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Employees.Dtos;

namespace Workspace_Management_System.Application.Features.Employees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandler
    : IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<EmployeeDetailsDto>> Handle(
            GetEmployeeByIdQuery request,
            CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Employees
                .Query()
                .AsNoTracking()
                .Where(x => x.Id == request.Id && !x.IsDeleted)
                .Select(x => new EmployeeDetailsDto
                {
                    Id = x.Id,

                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FullName = x.FirstName + " " + x.LastName,

                    Phone = x.Phone,
                    Email = x.Email,

                    HireDate = x.HireDate,
                    Status = x.Status,

                    WorkspaceId = x.WorkspaceId,
                    WorkspaceName = x.Workspace != null
                        ? x.Workspace.Name
                        : null,

                    WorkspaceCode = x.Workspace != null
                        ? x.Workspace.Code
                        : null
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (employee is null)
            {
                return Result<EmployeeDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    "Employee not found.");
            }

            return Result<EmployeeDetailsDto>.Success(employee);
        }
    }
}
