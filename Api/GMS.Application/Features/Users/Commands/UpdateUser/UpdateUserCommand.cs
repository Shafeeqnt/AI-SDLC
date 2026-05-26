using FluentValidation;
using GMS.Application.Common.Interfaces;
using GMS.Application.Common.Mapping;
using GMS.Application.Common.Models;
using GMS.Application.Features.Users.Models;
using GMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GMS.Application.Features.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(int UserId, string Email, int UserTypeId, bool IsActive, IReadOnlyCollection<int> RoleIds)
    : IRequest<ApiResponse<UserDto>>;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.RoleIds).NotEmpty();
        RuleFor(x => x.UserTypeId).InclusiveBetween(1, 2);
    }
}

public sealed class UpdateUserCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateUserCommand, ApiResponse<UserDto>>
{
    public async Task<ApiResponse<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            return ApiResponse<UserDto>.Fail("User not found.", statusCode: 404);
        }

        if (await dbContext.Users.AnyAsync(x => x.Email == request.Email && x.Id != request.UserId, cancellationToken))
        {
            return ApiResponse<UserDto>.Fail("Email already exists.");
        }

        var roles = await dbContext.Roles.Where(x => request.RoleIds.Contains(x.Id)).ToListAsync(cancellationToken);
        if (roles.Count != request.RoleIds.Count)
        {
            return ApiResponse<UserDto>.Fail("One or more roles are invalid.");
        }

        user.Email = request.Email;
        user.UserTypeId = request.UserTypeId;
        user.IsActive = request.IsActive;

        var existingRoles = user.UserRoles.ToList();
        dbContext.UserRoles.RemoveRange(existingRoles);
        user.UserRoles = roles.Select(x => new UserRole { UserId = user.Id, RoleId = x.Id }).ToList();

        await dbContext.SaveChangesAsync(cancellationToken);
        await dbContext.Entry(user).Collection(x => x.UserRoles).Query().Include(x => x.Role).LoadAsync(cancellationToken);

        return ApiResponse<UserDto>.Ok(user.ToDto());
    }
}
