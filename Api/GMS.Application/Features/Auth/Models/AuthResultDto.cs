namespace GMS.Application.Features.Auth.Models;

public sealed record AuthResultDto(
    string AccessToken,
    int UserId,
    string Username,
    string Email,
    IReadOnlyCollection<int> RoleIds,
    IReadOnlyCollection<string> Roles,
    bool PasswordChangeRequired);
