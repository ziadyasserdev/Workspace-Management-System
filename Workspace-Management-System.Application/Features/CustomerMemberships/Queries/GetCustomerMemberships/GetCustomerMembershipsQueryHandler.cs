using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.CustomerMemberships.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Queries.GetCustomerMemberships
{
    public class GetCustomerMembershipsQueryHandler
        : IRequestHandler<
            GetCustomerMembershipsQuery,
            Result<List<CustomerMembershipDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public GetCustomerMembershipsQueryHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<List<CustomerMembershipDto>>> Handle(
            GetCustomerMembershipsQuery request,
            CancellationToken cancellationToken)
        {
            var customerExists = await _unitOfWork.Customers
                .Query()
                .AnyAsync(
                    x => x.Id == request.CustomerId && !x.IsDeleted,
                    cancellationToken);

            if (!customerExists)
            {
                return Result<List<CustomerMembershipDto>>.Failure(
                    ResultStatus.NotFound,
                    _localizer["CustomerNotFound"].Value);
            }

            var isArabic =
                CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

            var memberships = await _unitOfWork.CustomerMemberships
                .Query()
                .AsNoTracking()
                .Where(
                    x => x.CustomerId == request.CustomerId &&
                         !x.IsDeleted)
                .OrderByDescending(x => x.StartDate)
                .Select(x => new CustomerMembershipDto
                {
                    Id = x.Id,
                    MembershipId = x.MembershipId,

                    MembershipName = isArabic
                        ? x.Membership.NameAr
                        : x.Membership.NameEn,

                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Status = x.Status,
                    PricePaid = x.PricePaid
                })
                .ToListAsync(cancellationToken);

            return Result<List<CustomerMembershipDto>>
                .Success(memberships);
        }
    }
}
