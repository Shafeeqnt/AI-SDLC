using System.Security.Claims;
using GMS.Application.Common.Models;

namespace GMS.Api.Extensions;

public static class EndpointExtensions
{
    public static IResult ToHttpResult<T>(this ApiResponse<T> response) =>
        Results.Json(response, statusCode: response.StatusCode);

    public static int GetCurrentUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst("userId")?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var userId) ? userId : 0;
    }

    public static bool HasRole(this ClaimsPrincipal user, params string[] roles) =>
        roles.Any(user.IsInRole);
}
