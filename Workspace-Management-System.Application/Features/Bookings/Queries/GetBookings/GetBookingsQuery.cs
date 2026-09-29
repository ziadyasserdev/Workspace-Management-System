using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Bookings.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Bookings.Queries.GetBookings
{
    public class GetBookingsQuery
    : IRequest<Result<PaginatedResult<BookingListDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public BookingStatus? Status { get; set; }

        public int? WorkspaceId { get; set; }

        public int? CustomerId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool SortDescending { get; set; } = false;
    }
}
