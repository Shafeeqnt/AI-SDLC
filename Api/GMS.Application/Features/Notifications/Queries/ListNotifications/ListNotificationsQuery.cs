using GMS.Application.Common.Interfaces;
using GMS.Application.Common.Mapping;
using GMS.Application.Common.Models;
using GMS.Application.Features.Notifications.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GMS.Application.Features.Notifications.Queries.ListNotifications;

public sealed record ListNotificationsQuery(int UserId, bool UnreadOnly, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<PagedResult<NotificationDto>>>;

public sealed class ListNotificationsQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<ListNotificationsQuery, ApiResponse<PagedResult<NotificationDto>>>
{
    public async Task<ApiResponse<PagedResult<NotificationDto>>> Handle(ListNotificationsQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Notifications.AsNoTracking().Where(x => x.RecipientUserId == request.UserId);

        if (request.UnreadOnly)
        {
            query = query.Where(x => !x.IsRead);
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(x => x.CreatedDateUtc)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return ApiResponse<PagedResult<NotificationDto>>.Ok(new PagedResult<NotificationDto>
        {
            Items = items.Select(x => x.ToDto()).ToArray(),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
