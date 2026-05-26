using GMS.Domain.Common;

namespace GMS.Domain.Entities;

public sealed class Grievance : BaseEntity
{
    public string ReferenceNumber { get; set; } = string.Empty;
    public string? ComplainerName { get; set; }
    public string? OrganizationName { get; set; }
    public string ContactNumber { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string GrievanceDescription { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string ProjectId { get; set; } = string.Empty;
    public int StatusId { get; set; }
    public int CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;
    public DateTime CreatedDateUtc { get; set; }
    public DateTime? UpdatedDateUtc { get; set; }
    public DateTime? ClosedDateUtc { get; set; }
    public ICollection<GrievanceStatusHistory> StatusHistory { get; set; } = new List<GrievanceStatusHistory>();
}
