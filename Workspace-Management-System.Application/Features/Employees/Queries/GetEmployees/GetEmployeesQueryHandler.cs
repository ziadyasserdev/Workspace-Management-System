using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Employees.Dtos;

namespace Workspace_Management_System.Application.Features.Employees.Queries.GetEmployees
{
    public class GetEmployeesQueryHandler
      : IRequestHandler<GetEmployeesQuery, Result<PaginatedResult<EmployeeListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<EmployeeListDto>>> Handle(
            GetEmployeesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Employees
                .Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    x.FirstName.Contains(search) ||
                    x.LastName.Contains(search) ||
                    x.Phone.Contains(search) ||
                    (x.Email != null && x.Email.Contains(search)));
            }

            // Filter by status
            if (request.Status.HasValue)
            {
                query = query.Where(x =>
                    x.Status == request.Status.Value);
            }

            // Filter by workspace
            if (request.WorkspaceId.HasValue)
            {
                query = query.Where(x =>
                    x.WorkspaceId == request.WorkspaceId.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var employees = await query
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new EmployeeListDto
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
                        : null
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<EmployeeListDto>(employees, request.PageNumber,
                request.PageSize, totalCount);
       

            return Result<PaginatedResult<EmployeeListDto>>.Success(result);
        }
    }
}
