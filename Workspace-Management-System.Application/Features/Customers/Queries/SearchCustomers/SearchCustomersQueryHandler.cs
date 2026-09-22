using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Customers.Dtos;
namespace Workspace_Management_System.Application.Features.Customers.Queries.SearchCustomers
{
    public class SearchCustomersQueryHandler
        : IRequestHandler<
            SearchCustomersQuery,
            Result<PaginatedResult<CustomerDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchCustomersQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<CustomerDto>>> Handle(
            SearchCustomersQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Customers
                .Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            var search = request.SearchTerm.Trim();

            query = query.Where(x =>
                x.FullName.Contains(search) ||
                x.MobileNumber.Contains(search) ||
                (x.Email != null &&
                 x.Email.Contains(search)));

            var totalCount = await query.CountAsync(
                cancellationToken);

            var items = await query
                .OrderBy(x => x.FullName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CustomerDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    MobileNumber = x.MobileNumber,
                    Email = x.Email,
                    CompanyId = x.CompanyId,
                    CustomerType = x.CustomerType,
                    Notes = x.Notes,
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
                "Customers searched successfully.");
        }
    }
}