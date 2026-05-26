namespace GMS.Application.Common.Interfaces;

public interface INotificationDispatcher
{
    Task NotifyRelevantStaffAsync(int grievanceId, string message, CancellationToken cancellationToken);
}
