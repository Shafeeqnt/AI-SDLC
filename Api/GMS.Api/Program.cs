using System.Security.Claims;
using GMS.Api.Extensions;
using GMS.Api.Middleware;
using GMS.Application;
using GMS.Application.Common.Interfaces;
using GMS.Application.Features.Auth.Commands.ForgotPassword;
using GMS.Application.Features.Auth.Commands.Login;
using GMS.Application.Features.Auth.Commands.ResetPassword;
using GMS.Application.Features.Auth.Commands.ResetPasswordByRecovery;
using GMS.Application.Features.Grievances.Commands.SubmitGrievance;
using GMS.Application.Features.Grievances.Queries.GetGrievanceDetail;
using GMS.Application.Features.Grievances.Queries.ListGrievances;
using GMS.Application.Features.Grievances.Queries.ListMyGrievances;
using GMS.Application.Features.Notifications.Queries.ListNotifications;
using GMS.Application.Features.Settings.Commands.UpsertSetting;
using GMS.Application.Features.Settings.Queries.ListSettings;
using GMS.Application.Features.Users.Commands.CreateUser;
using GMS.Application.Features.Users.Commands.UpdateUser;
using GMS.Application.Features.Users.Queries.ListUsers;
using GMS.Infrastructure;
using GMS.Infrastructure.Persistence;
using GMS.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("ConfiguredOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseSerilogRequestLogging();
app.UseCors("ConfiguredOrigins");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GmsDbContext>();
    await dbContext.Database.MigrateAsync();
    await SeedData.InitializeAsync(dbContext, scope.ServiceProvider.GetRequiredService<IPasswordHasher>(), CancellationToken.None);
}

var auth = app.MapGroup("/api/auth");
auth.MapPost("/login", async (LoginCommand command, ISender sender, CancellationToken cancellationToken) =>
    (await sender.Send(command, cancellationToken)).ToHttpResult())
    .AllowAnonymous()
    .Accepts<LoginCommand>("application/json")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized);

auth.MapPost("/forgot-password", async (ForgotPasswordCommand command, ISender sender, CancellationToken cancellationToken) =>
    (await sender.Send(command, cancellationToken)).ToHttpResult())
    .AllowAnonymous()
    .Accepts<ForgotPasswordCommand>("application/json");

auth.MapPost("/reset-password", async (ResetPasswordCommand command, ISender sender, CancellationToken cancellationToken) =>
    (await sender.Send(command, cancellationToken)).ToHttpResult())
    .AllowAnonymous()
    .Accepts<ResetPasswordCommand>("application/json");

auth.MapPost("/reset-password/recovery", async (ResetPasswordByRecoveryCommand command, ISender sender, CancellationToken cancellationToken) =>
    (await sender.Send(command, cancellationToken)).ToHttpResult())
    .AllowAnonymous()
    .Accepts<ResetPasswordByRecoveryCommand>("application/json");

var users = app.MapGroup("/api/users").RequireAuthorization("AdminOnly");
users.MapGet("/", async (string? role, int? userTypeId, bool? isActive, int page, int pageSize, ISender sender, CancellationToken cancellationToken) =>
    (await sender.Send(new ListUsersQuery(role, userTypeId, isActive, page <= 0 ? 1 : page, pageSize <= 0 ? 20 : pageSize), cancellationToken)).ToHttpResult());

users.MapPost("/", async (CreateUserCommand command, ISender sender, CancellationToken cancellationToken) =>
    (await sender.Send(command, cancellationToken)).ToHttpResult())
    .Accepts<CreateUserCommand>("application/json");

users.MapPut("/{userId:int}", async (int userId, UpdateUserRequest request, ISender sender, CancellationToken cancellationToken) =>
    (await sender.Send(new UpdateUserCommand(userId, request.Email, request.UserTypeId, request.IsActive, request.RoleIds), cancellationToken)).ToHttpResult())
    .Accepts<UpdateUserRequest>("application/json");

var grievances = app.MapGroup("/api/grievances").RequireAuthorization();
grievances.MapPost("/", async (SubmitGrievanceRequest request, ClaimsPrincipal user, ISender sender, CancellationToken cancellationToken) =>
    (await sender.Send(
        new SubmitGrievanceCommand(user.GetCurrentUserId(), request.ComplainerName, request.OrganizationName, request.ContactNumber, request.EmailAddress, request.GrievanceDescription, request.ProjectName, request.ProjectId),
        cancellationToken)).ToHttpResult())
    .Accepts<SubmitGrievanceRequest>("application/json");

grievances.MapGet("/my", async (ClaimsPrincipal user, int? statusId, bool includeClosed, int page, int pageSize, ISender sender, CancellationToken cancellationToken) =>
    (await sender.Send(new ListMyGrievancesQuery(user.GetCurrentUserId(), statusId, includeClosed, page <= 0 ? 1 : page, pageSize <= 0 ? 20 : pageSize), cancellationToken)).ToHttpResult());

grievances.MapGet("/{grievanceId:int}", async (int grievanceId, ClaimsPrincipal user, ISender sender, CancellationToken cancellationToken) =>
    (await sender.Send(
        new GetGrievanceDetailQuery(grievanceId, user.GetCurrentUserId(), user.HasRole("SystemAdministrator", "StaffMember")),
        cancellationToken)).ToHttpResult());

grievances.MapGet("/", async (string? referenceNumber, string? projectId, int? statusId, int page, int pageSize, ClaimsPrincipal user, ISender sender, CancellationToken cancellationToken) =>
{
    if (!user.HasRole("SystemAdministrator", "StaffMember"))
    {
        return Results.Json(GMS.Application.Common.Models.ApiResponse<object>.Fail("Forbidden.", statusCode: StatusCodes.Status403Forbidden), statusCode: StatusCodes.Status403Forbidden);
    }

    return (await sender.Send(new ListGrievancesQuery(referenceNumber, projectId, statusId, page <= 0 ? 1 : page, pageSize <= 0 ? 20 : pageSize), cancellationToken)).ToHttpResult();
});

app.MapGet("/api/notifications", async (bool unreadOnly, int page, int pageSize, ClaimsPrincipal user, ISender sender, CancellationToken cancellationToken) =>
    (await sender.Send(new ListNotificationsQuery(user.GetCurrentUserId(), unreadOnly, page <= 0 ? 1 : page, pageSize <= 0 ? 20 : pageSize), cancellationToken)).ToHttpResult())
    .RequireAuthorization();

var settings = app.MapGroup("/api/settings").RequireAuthorization("AdminOnly");
settings.MapGet("/", async (ISender sender, CancellationToken cancellationToken) =>
    (await sender.Send(new ListSettingsQuery(), cancellationToken)).ToHttpResult());
settings.MapPut("/{settingKey}", async (string settingKey, UpsertSettingRequest request, ClaimsPrincipal user, ISender sender, CancellationToken cancellationToken) =>
    (await sender.Send(new UpsertSettingCommand(settingKey, request.SettingValue, user.GetCurrentUserId()), cancellationToken)).ToHttpResult())
    .Accepts<UpsertSettingRequest>("application/json");

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();

public sealed record UpdateUserRequest(string Email, int UserTypeId, bool IsActive, IReadOnlyCollection<int> RoleIds);
public sealed record SubmitGrievanceRequest(string? ComplainerName, string? OrganizationName, string ContactNumber, string EmailAddress, string GrievanceDescription, string ProjectName, string ProjectId);
public sealed record UpsertSettingRequest(string SettingValue);
