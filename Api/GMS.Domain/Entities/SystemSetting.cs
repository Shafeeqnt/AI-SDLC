using GMS.Domain.Common;

namespace GMS.Domain.Entities;

public sealed class SystemSetting : BaseEntity
{
    public string SettingKey { get; set; } = string.Empty;
    public string SettingValue { get; set; } = string.Empty;
    public int? UpdatedByUserId { get; set; }
    public DateTime? UpdatedDateUtc { get; set; }
}
