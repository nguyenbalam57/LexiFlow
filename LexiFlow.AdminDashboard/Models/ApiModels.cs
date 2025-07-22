using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LexiFlow.AdminDashboard.Models
{
    // Models related to Dashboard

    public class DashboardStatistics
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int TotalContent { get; set; }
        public int ContentAddedThisMonth { get; set; }
        public int TotalExercises { get; set; }
        public int TotalLessons { get; set; }
        public int TotalCourses { get; set; }
        public int TotalQuizzes { get; set; }
        public int CompletedQuizzes { get; set; }
        public int StudentsActive { get; set; }
        public double AverageScore { get; set; }
        public List<UserActivity> RecentUserActivities { get; set; } = new List<UserActivity>();
        public List<MonthlyStats> MonthlyUserStats { get; set; } = new List<MonthlyStats>();
        public Dictionary<string, int> ContentByCategory { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> UsersByRole { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, double> SystemPerformance { get; set; } = new Dictionary<string, double>();
    }

    public class MonthlyStats
    {
        public DateTime Month { get; set; }
        public int Value { get; set; }
        public string Label { get; set; } = string.Empty;
    }

    public class UserActivity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Details { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
    }

    public class UserManagementStats
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int AdminUsers { get; set; }
        public int TeacherUsers { get; set; }
        public int StudentUsers { get; set; }
        public int NewUsersToday { get; set; }
        public int NewUsersThisWeek { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int LockedAccounts { get; set; }
        public int PendingApprovals { get; set; }
        public Dictionary<string, int> UsersByCountry { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> UsersByRole { get; set; } = new Dictionary<string, int>();
        public List<UserActivity> RecentUserRegistrations { get; set; } = new List<UserActivity>();
    }

    // Models related to User Management

    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public bool IsEmailVerified { get; set; } = false;

        public bool IsTwoFactorEnabled { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? LastLoginAt { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public string ProfileImageUrl { get; set; } = string.Empty;

        public string TimeZone { get; set; } = "UTC";

        public string Language { get; set; } = "en-US";

        public List<int> RoleIds { get; set; } = new List<int>();

        public List<string> RoleNames { get; set; } = new List<string>();

        [JsonIgnore]
        public string FullName => $"{FirstName} {LastName}".Trim();
    }

    public class Role
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        public bool IsSystemRole { get; set; } = false;

        public List<string> Permissions { get; set; } = new List<string>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int UsersCount { get; set; }
    }

    // Models related to System Settings

    public class SystemSettings
    {
        // General settings
        public string ApplicationName { get; set; } = "LexiFlow";
        public string CompanyName { get; set; } = string.Empty;
        public string SupportEmail { get; set; } = string.Empty;
        public string SupportPhone { get; set; } = string.Empty;
        public string DefaultLanguage { get; set; } = "en-US";
        public string DefaultTimeZone { get; set; } = "UTC";

        // Email settings
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 587;
        public string SmtpUsername { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public bool SmtpUseSsl { get; set; } = true;
        public string EmailSender { get; set; } = string.Empty;
        public string EmailSenderName { get; set; } = string.Empty;

        // Security settings
        public bool EnableRegistration { get; set; } = true;
        public bool RequireEmailVerification { get; set; } = true;
        public bool AllowPasswordReset { get; set; } = true;
        public int MinimumPasswordLength { get; set; } = 8;
        public bool RequirePasswordComplexity { get; set; } = true;
        public int PasswordExpiryDays { get; set; } = 90;
        public int MaxFailedLoginAttempts { get; set; } = 5;
        public int AccountLockoutMinutes { get; set; } = 30;
        public bool EnableTwoFactorAuth { get; set; } = false;

        // Advanced settings
        public int SessionTimeoutMinutes { get; set; } = 30;
        public string FileUploadPath { get; set; } = string.Empty;
        public List<string> AllowedFileExtensions { get; set; } = new List<string>();
        public int MaxUploadSizeMB { get; set; } = 10;
        public bool EnableAuditLogging { get; set; } = true;
        public int AuditLogRetentionDays { get; set; } = 90;
        public bool EnablePerformanceMonitoring { get; set; } = true;
    }

    // Models related to API responses

    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }

    public class ApiError
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public List<ValidationError>? ValidationErrors { get; set; }
    }

    public class ValidationError
    {
        public string Field { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    // DTOs for filtering and searching

    public class UserFilterDto
    {
        public string? SearchTerm { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public string? Role { get; set; }
        public string? SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class ActivityLogFilterDto
    {
        public DateTime From { get; set; } = DateTime.Now.AddDays(-7);
        public DateTime To { get; set; } = DateTime.Now;
        public string? Module { get; set; }
        public string? Action { get; set; }
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string? IpAddress { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}