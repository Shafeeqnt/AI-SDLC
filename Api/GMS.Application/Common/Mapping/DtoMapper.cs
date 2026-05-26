using GMS.Application.Features.Grievances.Models;
using GMS.Application.Features.Notifications.Models;
using GMS.Application.Features.Settings.Models;
using GMS.Application.Features.Users.Models;
using GMS.Domain.Entities;

namespace GMS.Application.Common.Mapping;

public static class DtoMapper
{
    public static UserDto ToDto(this User user) =>
        new(
            user.Id,
            user.Username,
            user.Email,
            user.UserTypeId,
            user.IsActive,
            user.UserRoles.Select(x => x.RoleId).ToArray(),
            user.UserRoles.Select(x => x.Role.Name).ToArray());

    public static GrievanceDto ToDto(this Grievance grievance) =>
        new(
            grievance.Id,
            grievance.ReferenceNumber,
            grievance.ComplainerName,
            grievance.OrganizationName,
            grievance.ContactNumber,
            grievance.EmailAddress,
            grievance.GrievanceDescription,
            grievance.ProjectName,
            grievance.ProjectId,
            grievance.StatusId,
            grievance.CreatedDateUtc,
            grievance.UpdatedDateUtc,
            grievance.ClosedDateUtc);

    public static NotificationDto ToDto(this Notification notification) =>
        new(
            notification.Id,
            notification.GrievanceId,
            notification.Message,
            notification.IsRead,
            notification.CreatedDateUtc);

    public static SettingDto ToDto(this SystemSetting setting) =>
        new(setting.Id, setting.SettingKey, setting.SettingValue, setting.UpdatedDateUtc);
}
