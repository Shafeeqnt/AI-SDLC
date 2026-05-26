using GMS.Domain.Entities;
using GMS.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GMS.Infrastructure.Services;

public static class SeedData
{
    public static async Task InitializeAsync(Persistence.GmsDbContext dbContext, IPasswordHasher passwordHasher, CancellationToken cancellationToken)
    {
        var sysadmin = await dbContext.Users.SingleAsync(x => x.Id == 1, cancellationToken);
        if (sysadmin.PasswordHash == "SEED_AT_RUNTIME")
        {
            sysadmin.PasswordHash = passwordHasher.Hash("Assyst@123");
            sysadmin.PasswordLastChangedDateUtc = DateTime.UtcNow;
        }

        var hasRecoveryQuestions = await dbContext.RecoveryQuestions.AnyAsync(x => x.UserId == 1, cancellationToken);
        if (!hasRecoveryQuestions)
        {
            dbContext.RecoveryQuestions.AddRange(
                new RecoveryQuestion { UserId = 1, QuestionText = "What is your default admin recovery code?", AnswerHash = passwordHasher.Hash("Assyst@123") },
                new RecoveryQuestion { UserId = 1, QuestionText = "What system is this account for?", AnswerHash = passwordHasher.Hash("GMS") });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
