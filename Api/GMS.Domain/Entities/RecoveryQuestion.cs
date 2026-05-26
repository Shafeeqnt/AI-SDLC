using GMS.Domain.Common;

namespace GMS.Domain.Entities;

public sealed class RecoveryQuestion : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string QuestionText { get; set; } = string.Empty;
    public string AnswerHash { get; set; } = string.Empty;
}
