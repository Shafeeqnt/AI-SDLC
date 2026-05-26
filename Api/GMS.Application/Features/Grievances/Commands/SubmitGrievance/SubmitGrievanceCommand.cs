using FluentValidation;
using GMS.Application.Common.Interfaces;
using GMS.Application.Common.Mapping;
using GMS.Application.Common.Models;
using GMS.Application.Features.Grievances.Models;
using GMS.Domain.Entities;
using GMS.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GMS.Application.Features.Grievances.Commands.SubmitGrievance;

public sealed record SubmitGrievanceCommand(
    int CreatedByUserId,
    string? ComplainerName,
    string? OrganizationName,
    string ContactNumber,
    string EmailAddress,
    string GrievanceDescription,
    string ProjectName,
    string ProjectId) : IRequest<ApiResponse<GrievanceDto>>;

public sealed class SubmitGrievanceCommandValidator : AbstractValidator<SubmitGrievanceCommand>
{
    public SubmitGrievanceCommandValidator()
    {
        RuleFor(x => x.CreatedByUserId).GreaterThan(0);
        RuleFor(x => x.ContactNumber).NotEmpty().Matches("^[0-9]{7,15}$");
        RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress();
        RuleFor(x => x.GrievanceDescription).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.ProjectName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ProjectId).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ComplainerName).MaximumLength(200);
        RuleFor(x => x.OrganizationName).MaximumLength(200);
    }
}

public sealed class SubmitGrievanceCommandHandler(
    IApplicationDbContext dbContext,
    IReferenceNumberGenerator referenceNumberGenerator,
    IDateTimeProvider dateTimeProvider,
    INotificationDispatcher notificationDispatcher) : IRequestHandler<SubmitGrievanceCommand, ApiResponse<GrievanceDto>>
{
    public async Task<ApiResponse<GrievanceDto>> Handle(SubmitGrievanceCommand request, CancellationToken cancellationToken)
    {
        var userExists = await dbContext.Users.AnyAsync(x => x.Id == request.CreatedByUserId && x.IsActive, cancellationToken);
        if (!userExists)
        {
            return ApiResponse<GrievanceDto>.Fail("Submitting user not found.", statusCode: 404);
        }

        string referenceNumber;
        do
        {
            referenceNumber = referenceNumberGenerator.Generate();
        } while (await dbContext.Grievances.AnyAsync(x => x.ReferenceNumber == referenceNumber, cancellationToken));

        var grievance = new Grievance
        {
            ReferenceNumber = referenceNumber,
            ComplainerName = request.ComplainerName,
            OrganizationName = request.OrganizationName,
            ContactNumber = request.ContactNumber,
            EmailAddress = request.EmailAddress,
            GrievanceDescription = request.GrievanceDescription,
            ProjectName = request.ProjectName,
            ProjectId = request.ProjectId,
            StatusId = (int)GrievanceStatus.Open,
            CreatedByUserId = request.CreatedByUserId,
            CreatedDateUtc = dateTimeProvider.UtcNow
        };

        dbContext.Grievances.Add(grievance);
        dbContext.GrievanceStatusHistories.Add(new GrievanceStatusHistory
        {
            Grievance = grievance,
            FromStatusId = null,
            ToStatusId = (int)GrievanceStatus.Open,
            ChangedByUserId = request.CreatedByUserId,
            ChangedDateUtc = dateTimeProvider.UtcNow,
            Comment = "Grievance created"
        });

        await dbContext.SaveChangesAsync(cancellationToken);
        await notificationDispatcher.NotifyRelevantStaffAsync(grievance.Id, $"New grievance submitted: {grievance.ReferenceNumber}", cancellationToken);

        return ApiResponse<GrievanceDto>.Ok(grievance.ToDto(), statusCode: 201);
    }
}
