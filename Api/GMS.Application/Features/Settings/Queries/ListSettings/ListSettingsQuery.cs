using GMS.Application.Common.Interfaces;
using GMS.Application.Common.Mapping;
using GMS.Application.Common.Models;
using GMS.Application.Features.Settings.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GMS.Application.Features.Settings.Queries.ListSettings;

public sealed record ListSettingsQuery() : IRequest<ApiResponse<IReadOnlyCollection<SettingDto>>>;

public sealed class ListSettingsQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<ListSettingsQuery, ApiResponse<IReadOnlyCollection<SettingDto>>>
{
    public async Task<ApiResponse<IReadOnlyCollection<SettingDto>>> Handle(ListSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = await dbContext.SystemSettings.AsNoTracking()
            .OrderBy(x => x.SettingKey)
            .ToListAsync(cancellationToken);

        return ApiResponse<IReadOnlyCollection<SettingDto>>.Ok(settings.Select(x => x.ToDto()).ToArray());
    }
}
