namespace GMS.Application.Features.Settings.Models;

public sealed record SettingDto(int SettingId, string SettingKey, string SettingValue, DateTime? UpdatedDateUtc);
