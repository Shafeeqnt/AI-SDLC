namespace GMS.Application.Features.Users.Models;

public sealed record UserDto(
    int UserId,
    string Username,
    string Email,
    int UserTypeId,
    bool IsActive,
    IReadOnlyCollection<int> RoleIds,
    IReadOnlyCollection<string> Roles);
