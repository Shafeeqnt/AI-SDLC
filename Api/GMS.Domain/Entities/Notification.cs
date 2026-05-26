using GMS.Domain.Common;

namespace GMS.Domain.Entities;

public sealed class Notification : BaseEntity
{
    public int RecipientUserId { get; set; }
    public User RecipientUser { get; set; } = null!;
    public int? GrievanceId { get; set; }
    public Grievance? Grievance { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedDateUtc { get; set; }
}
