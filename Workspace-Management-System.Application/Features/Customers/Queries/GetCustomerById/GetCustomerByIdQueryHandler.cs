
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Customers.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler
    : IRequestHandler<GetCustomerByIdQuery, Result<CustomerEditDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer _localizer;

    public GetCustomerByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<CustomerEditDto>> Handle(
        GetCustomerByIdQuery request,
        CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers
            .Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (customer is null)
        {
            return Result<CustomerEditDto>.Failure(
                ResultStatus.NotFound,
                _localizer["CustomerNotFound"]);
        }

        var result = new CustomerEditDto
        {
            Id = customer.Id,
            FullNameEn = customer.FullNameEn,
            FullNameAr = customer.FullNameAr,
            MobileNumber = customer.MobileNumber,
            Email = customer.Email,
            CompanyId = customer.CompanyId,
            CustomerType = customer.CustomerType.ToString(),
            NotesEn = customer.NotesEn,
            NotesAr = customer.NotesAr,
            RegistrationDate = customer.RegistrationDate,
            Status = customer.Status.ToString()
        };

        return Result<CustomerEditDto>.Success(
            result,
            _localizer["CustomerRetrievedSuccessfully"]);
    }
}
