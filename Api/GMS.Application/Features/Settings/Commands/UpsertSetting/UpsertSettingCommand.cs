using FluentValidation;
using GMS.Application.Common.Interfaces;
using GMS.Application.Common.Mapping;
using GMS.Application.Common.Models;
using GMS.Application.Features.Settings.Models;
using GMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GMS.Application.Features.Settings.Commands.UpsertSetting;

public sealed record UpsertSettingCommand(string SettingKey, string SettingValue, int UpdatedByUserId)
    : IRequest<ApiResponse<SettingDto>>;

public sealed class UpsertSettingCommandValidator : AbstractValidator<UpsertSettingCommand>
{
    public UpsertSettingCommandValidator()
    {
        RuleFor(x => x.SettingKey).NotEmpty().MaximumLength(100);
        RuleFor(x => x.SettingValue).NotEmpty().MaximumLength(500);
        RuleFor(x => x.UpdatedByUserId).GreaterThan(0);
    }
}

public sealed class UpsertSettingCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<UpsertSettingCommand, ApiResponse<SettingDto>>
{
    public async Task<ApiResponse<SettingDto>> Handle(UpsertSettingCommand request, CancellationToken cancellationToken)
    {
        var setting = await dbContext.SystemSettings.SingleOrDefaultAsync(x => x.SettingKey == request.SettingKey, cancellationToken);
        if (setting is null)
        {
            setting = new SystemSetting
            {
                SettingKey = request.SettingKey,
                SettingValue = request.SettingValue,
                UpdatedByUserId = request.UpdatedByUserId,
                UpdatedDateUtc = dateTimeProvider.UtcNow
            };
            dbContext.SystemSettings.Add(setting);
        }
        else
        {
            setting.SettingValue = request.SettingValue;
            setting.UpdatedByUserId = request.UpdatedByUserId;
            setting.UpdatedDateUtc = dateTimeProvider.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return ApiResponse<SettingDto>.Ok(setting.ToDto());
    }
}
