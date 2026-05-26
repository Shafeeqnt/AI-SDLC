using FluentValidation;
using GMS.Application.Common.Interfaces;
using GMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GMS.Application.Features.Auth.Commands.ResetPasswordByRecovery;

public sealed record ResetPasswordByRecoveryCommand(string Username, IReadOnlyCollection<string> RecoveryAnswers, string NewPassword)
    : IRequest<ApiResponse<object>>;

public sealed class ResetPasswordByRecoveryCommandValidator : AbstractValidator<ResetPasswordByRecoveryCommand>
{
    public ResetPasswordByRecoveryCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8);
        RuleFor(x => x.RecoveryAnswers).NotNull().Must(x => x.Count > 0);
    }
}

public sealed class ResetPasswordByRecoveryCommandHandler(
    IApplicationDbContext dbContext,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<ResetPasswordByRecoveryCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(ResetPasswordByRecoveryCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Include(x => x.RecoveryQuestions)
            .SingleOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

        if (user is null || user.RecoveryQuestions.Count == 0 || user.RecoveryQuestions.Count != request.RecoveryAnswers.Count)
        {
            return ApiResponse<object>.Fail("Recovery answers are invalid.", statusCode: 400);
        }

        var matched = user.RecoveryQuestions.Zip(request.RecoveryAnswers, (question, answer) =>
            passwordHasher.Verify(question.AnswerHash, answer)).All(x => x);

        if (!matched)
        {
            return ApiResponse<object>.Fail("Recovery answers are invalid.", statusCode: 400);
        }

        user.PasswordHash = passwordHasher.Hash(request.NewPassword);
        user.PasswordLastChangedDateUtc = dateTimeProvider.UtcNow;
        user.PasswordResetTokenHash = null;
        user.PasswordResetTokenExpiresAtUtc = null;
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<object>.Ok(new { resetSuccess = true });
    }
}
