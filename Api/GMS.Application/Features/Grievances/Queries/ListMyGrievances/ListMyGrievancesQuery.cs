using GMS.Application.Common.Interfaces;
using GMS.Application.Common.Mapping;
using GMS.Application.Common.Models;
using GMS.Application.Features.Grievances.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GMS.Application.Features.Grievances.Queries.ListMyGrievances;

public sealed record ListMyGrievancesQuery(int UserId, int? StatusId, bool IncludeClosed, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<PagedResult<GrievanceDto>>>;

public sealed class ListMyGrievancesQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<ListMyGrievancesQuery, ApiResponse<PagedResult<GrievanceDto>>>
{
    public async Task<ApiResponse<PagedResult<GrievanceDto>>> Handle(ListMyGrievancesQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Grievances.AsNoTracking().Where(x => x.CreatedByUserId == request.UserId);

        if (request.StatusId.HasValue)
        {
            query = query.Where(x => x.StatusId == request.StatusId.Value);
        }
        else if (!request.IncludeClosed)
        {
            query = query.Where(x => x.StatusId != 5);
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(x => x.CreatedDateUtc)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return ApiResponse<PagedResult<GrievanceDto>>.Ok(new PagedResult<GrievanceDto>
        {
            Items = items.Select(x => x.ToDto()).ToArray(),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
