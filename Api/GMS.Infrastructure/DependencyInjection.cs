using System.Text;
using GMS.Application.Common.Interfaces;
using GMS.Infrastructure.Auth;
using GMS.Infrastructure.Persistence;
using GMS.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace GMS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is not configured.");
        var serverVersionValue = configuration["Database:ServerVersion"] ?? "9.0.0";
        var serverVersion = Version.Parse(serverVersionValue);

        services.AddDbContext<GmsDbContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(serverVersion)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<GmsDbContext>());
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IReferenceNumberGenerator, ReferenceNumberGenerator>();
        services.AddScoped<INotificationDispatcher, NotificationDispatcher>();
        services.AddScoped<ISettingReader, SettingReader>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        var key = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured.");
        var issuer = configuration["Jwt:Issuer"] ?? "GMS";
        var audience = configuration["Jwt:Audience"] ?? "GMS.Client";

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly", policy => policy.RequireRole("SystemAdministrator"))
            .AddPolicy("StaffOrAdmin", policy => policy.RequireRole("SystemAdministrator", "StaffMember"));

        return services;
    }
}
