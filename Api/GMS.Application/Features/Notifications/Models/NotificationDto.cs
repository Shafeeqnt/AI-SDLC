namespace GMS.Application.Features.Notifications.Models;

public sealed record NotificationDto(
    int NotificationId,
    int? GrievanceId,
    string Message,
    bool IsRead,
    DateTime CreatedDateUtc);
