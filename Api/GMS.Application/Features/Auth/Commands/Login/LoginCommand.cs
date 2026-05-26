using FluentValidation;
using GMS.Application.Common.Interfaces;
using GMS.Application.Common.Models;
using GMS.Application.Features.Auth.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GMS.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string Username, string Password) : IRequest<ApiResponse<AuthResultDto>>;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(200);
    }
}

public sealed class LoginCommandHandler(
    IApplicationDbContext dbContext,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    ISettingReader settingReader,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<LoginCommand, ApiResponse<AuthResultDto>>
{
    public async Task<ApiResponse<AuthResultDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .SingleOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

        if (user is null || !user.IsActive || !passwordHasher.Verify(user.PasswordHash, request.Password))
        {
            return ApiResponse<AuthResultDto>.Fail("Invalid credentials.", statusCode: 401);
        }

        var roles = user.UserRoles.Select(x => x.Role).ToArray();
        var expiryDays = await settingReader.GetPasswordExpiryDaysAsync(cancellationToken);
        var passwordChangeRequired = !roles.Any(x => x.Name == "SystemAdministrator")
            && (!user.PasswordLastChangedDateUtc.HasValue || user.PasswordLastChangedDateUtc.Value.AddDays(expiryDays) <= dateTimeProvider.UtcNow);

        var result = new AuthResultDto(
            jwtTokenGenerator.GenerateToken(user, roles),
            user.Id,
            user.Username,
            user.Email,
            roles.Select(x => x.Id).ToArray(),
            roles.Select(x => x.Name).ToArray(),
            passwordChangeRequired);

        return ApiResponse<AuthResultDto>.Ok(result);
    }
}
