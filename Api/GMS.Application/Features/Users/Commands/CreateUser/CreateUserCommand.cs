using FluentValidation;
using GMS.Application.Common.Interfaces;
using GMS.Application.Common.Mapping;
using GMS.Application.Common.Models;
using GMS.Application.Features.Users.Models;
using GMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GMS.Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string Username,
    string Email,
    int UserTypeId,
    bool IsActive,
    string Password,
    IReadOnlyCollection<int> RoleIds) : IRequest<ApiResponse<UserDto>>;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.RoleIds).NotEmpty();
        RuleFor(x => x.UserTypeId).InclusiveBetween(1, 2);
    }
}

public sealed class CreateUserCommandHandler(
    IApplicationDbContext dbContext,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<CreateUserCommand, ApiResponse<UserDto>>
{
    public async Task<ApiResponse<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await dbContext.Users.AnyAsync(x => x.Username == request.Username, cancellationToken))
        {
            return ApiResponse<UserDto>.Fail("Username already exists.");
        }

        if (await dbContext.Users.AnyAsync(x => x.Email == request.Email, cancellationToken))
        {
            return ApiResponse<UserDto>.Fail("Email already exists.");
        }

        var roles = await dbContext.Roles.Where(x => request.RoleIds.Contains(x.Id)).ToListAsync(cancellationToken);
        if (roles.Count != request.RoleIds.Count)
        {
            return ApiResponse<UserDto>.Fail("One or more roles are invalid.");
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            UserTypeId = request.UserTypeId,
            IsActive = request.IsActive,
            PasswordHash = passwordHasher.Hash(request.Password),
            PasswordLastChangedDateUtc = dateTimeProvider.UtcNow,
            UserRoles = roles.Select(x => new UserRole { RoleId = x.Id }).ToList()
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        await dbContext.Entry(user).Collection(x => x.UserRoles).Query().Include(x => x.Role).LoadAsync(cancellationToken);
        return ApiResponse<UserDto>.Ok(user.ToDto(), statusCode: 201);
    }
}
