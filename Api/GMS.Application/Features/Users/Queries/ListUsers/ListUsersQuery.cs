using GMS.Application.Common.Interfaces;
using GMS.Application.Common.Mapping;
using GMS.Application.Common.Models;
using GMS.Application.Features.Users.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GMS.Application.Features.Users.Queries.ListUsers;

public sealed record ListUsersQuery(string? Role, int? UserTypeId, bool? IsActive, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<PagedResult<UserDto>>>;

public sealed class ListUsersQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<ListUsersQuery, ApiResponse<PagedResult<UserDto>>>
{
    public async Task<ApiResponse<PagedResult<UserDto>>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Users
            .AsNoTracking()
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            query = query.Where(x => x.UserRoles.Any(r => r.Role.Name == request.Role));
        }

        if (request.UserTypeId.HasValue)
        {
            query = query.Where(x => x.UserTypeId == request.UserTypeId.Value);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(x => x.Username)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var result = new PagedResult<UserDto>
        {
            Items = items.Select(x => x.ToDto()).ToArray(),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return ApiResponse<PagedResult<UserDto>>.Ok(result);
    }
}
