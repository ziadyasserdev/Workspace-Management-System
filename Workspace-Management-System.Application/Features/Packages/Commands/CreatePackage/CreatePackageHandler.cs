using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Packages.Commands.CreatePackage;

public class CreatePackageCommandHandler
    : IRequestHandler<CreatePackageCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreatePackageCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> Handle(
        CreatePackageCommand request,
        CancellationToken cancellationToken)
    {
        await using var transaction =
            await _unitOfWork.BeginTransactionAsync();

        try
        {
            var package = new Package
            {
                NameEn = request.NameEn.Trim(),
                NameAr = request.NameAr.Trim(),

                DescriptionEn =
                    string.IsNullOrWhiteSpace(request.DescriptionEn)
                        ? null
                        : request.DescriptionEn.Trim(),

                DescriptionAr =
                    string.IsNullOrWhiteSpace(request.DescriptionAr)
                        ? null
                        : request.DescriptionAr.Trim(),

                PackageType = request.PackageType,
                TotalHours = request.TotalHours,
                DurationDays = request.DurationDays,
                Price = request.Price,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUser.UserId
            };

            await _unitOfWork.Packages.AddAsync(package);

            await _unitOfWork.SaveAsync();

            await transaction.CommitAsync(cancellationToken);

            return Result<int>.Success(
                package.Id,
                "Package created successfully.");
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}