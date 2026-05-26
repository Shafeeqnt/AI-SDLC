using FluentValidation;
using GMS.Application.Common.Interfaces;
using GMS.Application.Common.Models;
using GMS.Application.Features.Auth.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GMS.Application.Features.Auth.Commands.ForgotPassword;

public sealed record ForgotPasswordCommand(string UsernameOrEmail) : IRequest<ApiResponse<ForgotPasswordResultDto>>;

public sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.UsernameOrEmail).NotEmpty().MaximumLength(200);
    }
}

public sealed class ForgotPasswordCommandHandler(
    IApplicationDbContext dbContext,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider,
    IEmailService emailService) : IRequestHandler<ForgotPasswordCommand, ApiResponse<ForgotPasswordResultDto>>
{
    public async Task<ApiResponse<ForgotPasswordResultDto>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(
            x => x.Username == request.UsernameOrEmail || x.Email == request.UsernameOrEmail,
            cancellationToken);

        if (user is not null && user.IsActive)
        {
            var token = Guid.NewGuid().ToString("N");
            user.PasswordResetTokenHash = passwordHasher.Hash(token);
            user.PasswordResetTokenExpiresAtUtc = dateTimeProvider.UtcNow.AddHours(1);
            await dbContext.SaveChangesAsync(cancellationToken);
            await emailService.SendPasswordResetAsync(user.Email, token, cancellationToken);
        }

        return ApiResponse<ForgotPasswordResultDto>.Ok(new ForgotPasswordResultDto(true, "email"));
    }
}
