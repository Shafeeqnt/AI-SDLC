using FluentValidation;
using GMS.Application.Common.Interfaces;
using GMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GMS.Application.Features.Auth.Commands.ResetPassword;

public sealed record ResetPasswordCommand(string ResetToken, string NewPassword) : IRequest<ApiResponse<object>>;

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.ResetToken).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8);
    }
}

public sealed class ResetPasswordCommandHandler(
    IApplicationDbContext dbContext,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<ResetPasswordCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var candidates = await dbContext.Users
            .Where(x => x.PasswordResetTokenHash != null && x.PasswordResetTokenExpiresAtUtc != null)
            .ToListAsync(cancellationToken);

        var user = candidates.SingleOrDefault(x =>
            x.PasswordResetTokenExpiresAtUtc >= dateTimeProvider.UtcNow &&
            x.PasswordResetTokenHash is not null &&
            passwordHasher.Verify(x.PasswordResetTokenHash, request.ResetToken));

        if (user is null)
        {
            return ApiResponse<object>.Fail("Reset token is invalid or expired.", statusCode: 400);
        }

        user.PasswordHash = passwordHasher.Hash(request.NewPassword);
        user.PasswordLastChangedDateUtc = dateTimeProvider.UtcNow;
        user.PasswordResetTokenHash = null;
        user.PasswordResetTokenExpiresAtUtc = null;
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<object>.Ok(new { resetSuccess = true });
    }
}
