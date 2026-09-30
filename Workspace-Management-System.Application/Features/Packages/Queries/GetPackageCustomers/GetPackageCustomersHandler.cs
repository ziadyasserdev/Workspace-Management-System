using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Packages.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Packages.Queries.GetPackageCustomers;

public class GetPackageCustomersHandler
    : IRequestHandler<
        GetPackageCustomersQuery,
        Result<PaginatedResult<PackageCustomerDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPackageCustomersHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginatedResult<PackageCustomerDto>>> Handle(
        GetPackageCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var packageExists = await _unitOfWork.Packages
            .Query()
            .AnyAsync(
                x =>
                    x.Id == request.PackageId &&
                    !x.IsDeleted,
                cancellationToken);

        if (!packageExists)
        {
            return Result<PaginatedResult<PackageCustomerDto>>.Failure(
                ResultStatus.NotFound,
                "Package not found.");
        }

        var query = _unitOfWork.CustomerPackages
            .Query()
            .Where(x =>
                x.PackageId == request.PackageId &&
                !x.IsDeleted &&
                !x.Customer.IsDeleted &&
                x.Status == CustomerPackageStatus.Active);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x =>
                x.Customer.FullName.Contains(search) ||
                x.Customer.MobileNumber.Contains(search) ||
                (x.Customer.Email != null &&
                 x.Customer.Email.Contains(search)));
        }

        var totalCount = await query
            .CountAsync(cancellationToken);

        var customers = await query
            .OrderBy(x => x.Customer.FullName)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new PackageCustomerDto
            {
                CustomerId = x.CustomerId,
                FullName = x.Customer.FullName,
                MobileNumber = x.Customer.MobileNumber,
                Status = x.Status,
                PurchaseDate = x.PurchaseDate,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                RemainingHours = x.RemainingHours
            })
            .ToListAsync(cancellationToken);

        var result = new PaginatedResult<PackageCustomerDto>(
            customers,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result<PaginatedResult<PackageCustomerDto>>.Success(
            result,
            "Package customers retrieved successfully.");
    }
}