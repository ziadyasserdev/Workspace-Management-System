using MediatR;
using Workspace_Management_System.Application.Features.Checkout.Dtos;

namespace Workspace_Management_System.Application.Features.Checkout.Commands.CheckoutSession;

public class CheckoutSessionCommand : IRequest<CheckoutResponseDto>
{
    public int SessionId { get; set; }

    public CheckoutRequestDto Request { get; set; } = new();
}