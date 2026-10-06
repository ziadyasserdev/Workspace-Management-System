using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Customers.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Customers.Queries.SearchCustomers;

public class SearchCustomersQueryHandler
    : IRequestHandler<
        SearchCustomersQuery,
        Result<PaginatedResult<CustomerDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IStringLocalizer _localizer;

    public SearchCustomersQueryHandler(
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _localizer = factory.Create(typeof(SharedResources));
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
            x.FullNameEn.Contains(search) ||
            x.FullNameAr.Contains(search) ||
            x.MobileNumber.Contains(search) ||
            (x.Email != null &&
             x.Email.Contains(search)) ||
            (x.NotesEn != null &&
             x.NotesEn.Contains(search)) ||
            (x.NotesAr != null &&
             x.NotesAr.Contains(search)));

        var totalCount = await query.CountAsync(
            cancellationToken);

        var customers = await query
            .OrderBy(x => x.FullNameEn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = customers
            .Select(x => new CustomerDto
            {
                Id = x.Id,
                FullName = _localizationService.GetLocalizedValue(
                    x.FullNameEn,
                    x.FullNameAr),
                MobileNumber = x.MobileNumber,
                Email = x.Email,
                CompanyId = x.CompanyId,
                CustomerType = _localizer[
                    $"CustomerType_{x.CustomerType}"],
                Notes = _localizationService.GetLocalizedValue(
                    x.NotesEn,
                    x.NotesAr),
                RegistrationDate = x.RegistrationDate,
                Status = _localizer[
                    $"CustomerStatus_{x.Status}"]
            })
            .ToList();

        var result = new PaginatedResult<CustomerDto>(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result<PaginatedResult<CustomerDto>>.Success(
            result,
            _localizer["CustomersSearchedSuccessfully"]);
    }
}
