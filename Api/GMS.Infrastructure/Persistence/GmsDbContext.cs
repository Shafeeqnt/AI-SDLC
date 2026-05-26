using GMS.Application.Common.Interfaces;
using GMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GMS.Infrastructure.Persistence;

public sealed class GmsDbContext(DbContextOptions<GmsDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RecoveryQuestion> RecoveryQuestions => Set<RecoveryQuestion>();
    public DbSet<Grievance> Grievances => Set<Grievance>();
    public DbSet<GrievanceStatusHistory> GrievanceStatusHistories => Set<GrievanceStatusHistory>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();
    public new EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class => base.Entry(entity);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(x => x.Username).IsUnique();
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.Username).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(200).IsRequired();
            entity.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();
            entity.Property(x => x.PasswordResetTokenHash).HasMaxLength(500);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(x => x.Name).IsUnique();
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.HasData(
                new Role { Id = 1, Name = "SystemAdministrator", Description = "Full system access" },
                new Role { Id = 2, Name = "StaffMember", Description = "Staff grievance access" },
                new Role { Id = 3, Name = "EndUser", Description = "End user grievance access" });
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasIndex(x => new { x.UserId, x.RoleId }).IsUnique();
        });

        modelBuilder.Entity<RecoveryQuestion>(entity =>
        {
            entity.Property(x => x.QuestionText).HasMaxLength(200).IsRequired();
            entity.Property(x => x.AnswerHash).HasMaxLength(500).IsRequired();
        });

        modelBuilder.Entity<Grievance>(entity =>
        {
            entity.HasIndex(x => x.ReferenceNumber).IsUnique();
            entity.HasIndex(x => x.StatusId);
            entity.HasIndex(x => x.ProjectId);
            entity.Property(x => x.ReferenceNumber).HasMaxLength(50).IsRequired();
            entity.Property(x => x.ComplainerName).HasMaxLength(200);
            entity.Property(x => x.OrganizationName).HasMaxLength(200);
            entity.Property(x => x.ContactNumber).HasMaxLength(20).IsRequired();
            entity.Property(x => x.EmailAddress).HasMaxLength(200).IsRequired();
            entity.Property(x => x.ProjectName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.ProjectId).HasMaxLength(100).IsRequired();
            entity.Property(x => x.GrievanceDescription).HasMaxLength(4000).IsRequired();
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasIndex(x => x.RecipientUserId);
            entity.Property(x => x.Message).HasMaxLength(500).IsRequired();
        });

        modelBuilder.Entity<SystemSetting>(entity =>
        {
            entity.HasIndex(x => x.SettingKey).IsUnique();
            entity.Property(x => x.SettingKey).HasMaxLength(100).IsRequired();
            entity.Property(x => x.SettingValue).HasMaxLength(500).IsRequired();
            entity.HasData(
                new SystemSetting { Id = 1, SettingKey = "PasswordExpiryDays", SettingValue = "90" },
                new SystemSetting { Id = 2, SettingKey = "NotificationChannel", SettingValue = "InAppAndEmail" });
        });

        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Username = "sysadmin",
            Email = "sysadmin@gms.local",
            UserTypeId = 1,
            IsActive = true,
            PasswordHash = "SEED_AT_RUNTIME",
            PasswordLastChangedDateUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });

        modelBuilder.Entity<UserRole>().HasData(new UserRole { Id = 1, UserId = 1, RoleId = 1 });
    }
}
