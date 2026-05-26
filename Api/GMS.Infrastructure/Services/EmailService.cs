using GMS.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace GMS.Infrastructure.Services;

public sealed class EmailService(ILogger<EmailService> logger) : IEmailService
{
    public Task SendPasswordResetAsync(string email, string token, CancellationToken cancellationToken)
    {
        logger.LogInformation("Password reset requested for {Email}. Reset token generated for email delivery.", email);
        return Task.CompletedTask;
    }
}
