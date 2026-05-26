using GMS.Application.Common.Interfaces;

namespace GMS.Infrastructure.Services;

public sealed class ReferenceNumberGenerator : IReferenceNumberGenerator
{
    public string Generate() => $"GRV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..25];
}
