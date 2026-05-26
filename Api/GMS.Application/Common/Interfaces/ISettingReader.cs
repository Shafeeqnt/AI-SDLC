namespace GMS.Application.Common.Interfaces;

public interface ISettingReader
{
    Task<int> GetPasswordExpiryDaysAsync(CancellationToken cancellationToken);
}
