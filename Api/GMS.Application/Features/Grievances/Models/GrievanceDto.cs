namespace GMS.Application.Features.Grievances.Models;

public sealed record GrievanceDto(
    int GrievanceId,
    string ReferenceNumber,
    string? ComplainerName,
    string? OrganizationName,
    string ContactNumber,
    string EmailAddress,
    string GrievanceDescription,
    string ProjectName,
    string ProjectId,
    int StatusId,
    DateTime CreatedDateUtc,
    DateTime? UpdatedDateUtc,
    DateTime? ClosedDateUtc);
