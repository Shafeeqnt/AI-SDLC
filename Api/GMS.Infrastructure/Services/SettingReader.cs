using GMS.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GMS.Infrastructure.Services;

public sealed class SettingReader(IApplicationDbContext dbContext) : ISettingReader
{
    public async Task<int> GetPasswordExpiryDaysAsync(CancellationToken cancellationToken)
    {
        var setting = await dbContext.SystemSettings.AsNoTracking()
            .SingleOrDefaultAsync(x => x.SettingKey == "PasswordExpiryDays", cancellationToken);

        return setting is not null && int.TryParse(setting.SettingValue, out var days) ? days : 90;
    }
}
