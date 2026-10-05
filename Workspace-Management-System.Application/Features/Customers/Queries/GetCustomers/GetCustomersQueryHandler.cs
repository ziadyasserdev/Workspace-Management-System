using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Customers.Dtos;

namespace Workspace_Management_System.Application.Features.Customers.Queries.GetCustomers
{
    public class GetCustomersQueryHandler
        : IRequestHandler<
            GetCustomersQuery,
            Result<PaginatedResult<CustomerDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCustomersQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<CustomerDto>>> Handle(
            GetCustomersQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Customers
                .Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            var totalCount = await query.CountAsync(
                cancellationToken);

            var items = await query
                .OrderBy(x => x.FullNameEn)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CustomerDto
                {
                    Id = x.Id,
                    FullNameEn = x.FullNameEn,
                    FullNameAr = x.FullNameAr,
                    MobileNumber = x.MobileNumber,
                    Email = x.Email,
                    CompanyId = x.CompanyId,
                    CustomerType = x.CustomerType,
                    NotesEn = x.NotesEn,
                    NotesAr = x.NotesAr,
                    RegistrationDate = x.RegistrationDate,
                    Status = x.Status
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<CustomerDto>(
                items,
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result<PaginatedResult<CustomerDto>>.Success(
                result,
                "Customers retrieved successfully.");
        }
    }
}