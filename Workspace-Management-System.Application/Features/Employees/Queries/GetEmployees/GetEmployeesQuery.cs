using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Employees.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Employees.Queries.GetEmployees
{
    public class GetEmployeesQuery : IRequest<Result<PaginatedResult<EmployeeListDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public EmployeeStatus? Status { get; set; }

        public int? WorkspaceId { get; set; }
    }
}
