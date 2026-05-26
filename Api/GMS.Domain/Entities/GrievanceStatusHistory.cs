using GMS.Domain.Common;

namespace GMS.Domain.Entities;

public sealed class GrievanceStatusHistory : BaseEntity
{
    public int GrievanceId { get; set; }
    public Grievance Grievance { get; set; } = null!;
    public int? FromStatusId { get; set; }
    public int ToStatusId { get; set; }
    public int ChangedByUserId { get; set; }
    public DateTime ChangedDateUtc { get; set; }
    public string? Comment { get; set; }
}
