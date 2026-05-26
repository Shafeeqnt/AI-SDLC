using GMS.Application.Common.Interfaces;
using GMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GMS.Infrastructure.Services;

public sealed class NotificationDispatcher(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider,
    ILogger<NotificationDispatcher> logger) : INotificationDispatcher
{
    public async Task NotifyRelevantStaffAsync(int grievanceId, string message, CancellationToken cancellationToken)
    {
        var staffUserIds = await dbContext.UserRoles
            .AsNoTracking()
            .Where(x => x.RoleId == 1 || x.RoleId == 2)
            .Select(x => x.UserId)
            .Distinct()
            .ToListAsync(cancellationToken);

        if (staffUserIds.Count == 0)
        {
            logger.LogWarning("No staff recipients found for grievance {GrievanceId}.", grievanceId);
            return;
        }

        foreach (var userId in staffUserIds)
        {
            dbContext.Notifications.Add(new Notification
            {
                RecipientUserId = userId,
                GrievanceId = grievanceId,
                Message = message,
                IsRead = false,
                CreatedDateUtc = dateTimeProvider.UtcNow
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
