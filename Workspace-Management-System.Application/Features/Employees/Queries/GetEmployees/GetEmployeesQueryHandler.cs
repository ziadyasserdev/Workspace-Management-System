using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            var isArabic = CultureInfo.CurrentUICulture
                .TwoLetterISOLanguageName == "ar";

            var query = _unitOfWork.Employees
                .Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            // Search in Arabic and English names, phone, and email.
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    x.FirstNameEn.Contains(search) ||
                    x.FirstNameAr.Contains(search) ||
                    x.LastNameEn.Contains(search) ||
                    x.LastNameAr.Contains(search) ||
                    x.Phone.Contains(search) ||
                    (x.Email != null && x.Email.Contains(search)));
            }

            // Filter by employee status.
            if (request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status.Value);
            }

            // Filter by workspace.
            if (request.WorkspaceId.HasValue)
            {
                query = query.Where(
                    x => x.WorkspaceId == request.WorkspaceId.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var employees = await query
      .OrderBy(x => isArabic ? x.FirstNameAr : x.FirstNameEn)
      .ThenBy(x => isArabic ? x.LastNameAr : x.LastNameEn)
      .Skip((request.PageNumber - 1) * request.PageSize)
      .Take(request.PageSize)
      .Select(x => new EmployeeListDto
      {
          Id = x.Id,

          FirstName = isArabic
              ? x.FirstNameAr
              : x.FirstNameEn,

          LastName = isArabic
              ? x.LastNameAr
              : x.LastNameEn,

          FullName = isArabic
              ? x.FirstNameAr + " " + x.LastNameAr
              : x.FirstNameEn + " " + x.LastNameEn,

          Phone = x.Phone,
          Email = x.Email,

          HireDate = x.HireDate,
          Status = x.Status,

          WorkspaceId = x.WorkspaceId,

          WorkspaceName = x.Workspace != null
              ? (isArabic
                  ? x.Workspace.NameAr
                  : x.Workspace.NameEn)
              : null
      })
      .ToListAsync(cancellationToken);

            var result = new PaginatedResult<EmployeeListDto>(
                employees,
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result<PaginatedResult<EmployeeListDto>>.Success(result);
        }
    }
}
