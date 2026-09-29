using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Bookings.Dtos;
using Workspace_Management_System.Application.Features.Bookings.Queries.GetBookings;

public class GetBookingsQueryHandler
    : IRequestHandler<
        GetBookingsQuery,
        Result<PaginatedResult<BookingListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBookingsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginatedResult<BookingListDto>>> Handle(
        GetBookingsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Bookings
            .Query()
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

      
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x =>
                x.BookingNumber.Contains(search) ||

                x.Customer.FullName.Contains(search) ||
            

                x.Workspace.Name.Contains(search) ||
                x.Workspace.Code.Contains(search));
        }

    
        if (request.Status.HasValue)
        {
            query = query.Where(x =>
                x.Status == request.Status.Value);
        }

      
        if (request.WorkspaceId.HasValue)
        {
            query = query.Where(x =>
                x.WorkspaceId == request.WorkspaceId.Value);
        }

    
        if (request.CustomerId.HasValue)
        {
            query = query.Where(x =>
                x.CustomerId == request.CustomerId.Value);
        }

     
        if (request.FromDate.HasValue)
        {
            var fromDate = request.FromDate.Value.Date;

            query = query.Where(x =>
                x.BookingDate >= fromDate);
        }

        if (request.ToDate.HasValue)
        {
            var toDate = request.ToDate.Value.Date.AddDays(1);

            query = query.Where(x =>
                x.BookingDate < toDate);
        }

      
        var totalCount =
            await query.CountAsync(cancellationToken);

     
        if (request.SortDescending)
        {
            query = query.OrderByDescending(x => x.StartTime);
        }
        else
        {
            query = query.OrderBy(x => x.StartTime);
        }

       
        var bookings = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new BookingListDto
            {
                Id = x.Id,

                BookingNumber = x.BookingNumber,

                CustomerId = x.CustomerId,
                CustomerName =
                    x.Customer.FullName,

                WorkspaceId = x.WorkspaceId,
                WorkspaceName = x.Workspace.Name,
                WorkspaceCode = x.Workspace.Code,

                BookingDate = x.BookingDate,

                StartTime = x.StartTime,
                ExpectedEndTime = x.ExpectedEndTime,

                NumberOfPeople = x.NumberOfPeople,

                Status = x.Status,

                Notes = x.Notes,

                HasSession = x.Session != null
            })
            .ToListAsync(cancellationToken);

        var result =
            new PaginatedResult<BookingListDto>(
                bookings,
                totalCount,
                request.PageNumber,
                request.PageSize);

        return Result<PaginatedResult<BookingListDto>>
            .Success(result);
    }
}