using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Customers.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler
    : IRequestHandler<GetCustomerByIdQuery, Result<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IStringLocalizer _localizer;

    public GetCustomerByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<CustomerDto>> Handle(
        GetCustomerByIdQuery request,
        CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers
            .Query()
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (customer is null)
        {
            return Result<CustomerDto>.Failure(
                ResultStatus.NotFound,
                _localizer["CustomerNotFound"]);
        }

        var result = new CustomerDto
        {
            Id = customer.Id,
            FullName = _localizationService.GetLocalizedValue(
                customer.FullNameEn,
                customer.FullNameAr),
            MobileNumber = customer.MobileNumber,
            Email = customer.Email,
            CompanyId = customer.CompanyId,
            CustomerType = _localizer[
                $"CustomerType_{customer.CustomerType}"],
            Notes = _localizationService.GetLocalizedValue(
                customer.NotesEn,
                customer.NotesAr),
            RegistrationDate = customer.RegistrationDate,
            Status = _localizer[
                $"CustomerStatus_{customer.Status}"]
        };

        return Result<CustomerDto>.Success(
            result,
            _localizer["CustomerRetrievedSuccessfully"]);
    }
}
