using GMS.Application.Common.Interfaces;
using GMS.Application.Common.Mapping;
using GMS.Application.Common.Models;
using GMS.Application.Features.Grievances.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GMS.Application.Features.Grievances.Queries.ListGrievances;

public sealed record ListGrievancesQuery(string? ReferenceNumber, string? ProjectId, int? StatusId, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<PagedResult<GrievanceDto>>>;

public sealed class ListGrievancesQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<ListGrievancesQuery, ApiResponse<PagedResult<GrievanceDto>>>
{
    public async Task<ApiResponse<PagedResult<GrievanceDto>>> Handle(ListGrievancesQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Grievances.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.ReferenceNumber))
        {
            query = query.Where(x => x.ReferenceNumber.Contains(request.ReferenceNumber));
        }

        if (!string.IsNullOrWhiteSpace(request.ProjectId))
        {
            query = query.Where(x => x.ProjectId == request.ProjectId);
        }

        if (request.StatusId.HasValue)
        {
            query = query.Where(x => x.StatusId == request.StatusId.Value);
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
