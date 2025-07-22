using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LexiFlow.API.Models
{
    /// <summary>
    /// Generic API response
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Success flag
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Response message
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Additional data
        /// </summary>
        public object? Data { get; set; }
    }

    /// <summary>
    /// Paginated result for collections
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class PaginatedResultDto<T>
    {
        /// <summary>
        /// Collection of items
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Total count of items
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Current page
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Total pages
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>
        /// Flag indicating if there is a previous page
        /// </summary>
        public bool HasPreviousPage => CurrentPage > 1;

        /// <summary>
        /// Flag indicating if there is a next page
        /// </summary>
        public bool HasNextPage => CurrentPage < TotalPages;
    }

    #region Dashboard DTOs

    /// <summary>
    /// Dashboard statistics
    /// </summary>
    public class DashboardStatisticsDto
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
        public List<UserActivityDto> RecentUserActivities { get; set; } = new List<UserActivityDto>();
        public List<MonthlyStatsDto> MonthlyUserStats { get; set; } = new List<MonthlyStatsDto>();
        public Dictionary<string, int> ContentByCategory { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> UsersByRole { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, double> SystemPerformance { get; set; } = new Dictionary<string, double>();
    }

    /// <summary>
    /// Monthly statistics
    /// </summary>
    public class MonthlyStatsDto
    {
        public DateTime Month { get; set; }
        public int Value { get; set; }
        public string Label { get; set; } = string.Empty;
    }

    /// <summary>
    /// User management statistics
    /// </summary>
    public class UserManagementStatsDto
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
        public List<UserActivityDto> RecentUserRegistrations { get; set; } = new List<UserActivityDto>();
    }

    #endregion

    #region User DTOs

    /// <summary>
    /// User data transfer object
    /// </summary>
    public class UserDto
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

        public string? RowVersionString { get; set; }

        [JsonIgnore]
        public string FullName => $"{FirstName} {LastName}".Trim();
    }

    /// <summary>
    /// Create user request
    /// </summary>
    public class CreateUserDto
    {
        [Required]
        public UserDto User { get; set; } = new UserDto();

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Reset password request
    /// </summary>
    public class ResetPasswordDto
    {
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string NewPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// Assign role request
    /// </summary>
    public class AssignRoleDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoleId { get; set; }
    }

    #endregion

    #region Role DTOs

    /// <summary>
    /// Role data transfer object
    /// </summary>
    public class RoleDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        public bool IsSystemRole { get; set; } = false;

        public List<string> Permissions { get; set; } = new List<string>();

        public int UsersCount { get; set; }
    }

    #endregion

    #region Activity DTOs

    /// <summary>
    /// User activity data transfer object
    /// </summary>
    public class UserActivityDto
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

    #endregion

    #region Settings DTOs

    /// <summary>
    /// System settings data transfer object
    /// </summary>
    public class SystemSettingsDto
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

    #endregion

    #region Database DTOs

    /// <summary>
    /// Backup request
    /// </summary>
    public class BackupRequestDto
    {
        [Required]
        public string Path { get; set; } = string.Empty;
    }

    /// <summary>
    /// Restore request
    /// </summary>
    public class RestoreRequestDto
    {
        [Required]
        public string BackupPath { get; set; } = string.Empty;
    }

    /// <summary>
    /// Connection test request
    /// </summary>
    public class ConnectionTestDto
    {
        [Required]
        public string ConnectionString { get; set; } = string.Empty;
    }

    #endregion

    #region Log DTOs

    /// <summary>
    /// Clear logs request
    /// </summary>
    public class ClearLogsDto
    {
        [Required]
        public DateTime Before { get; set; }
    }

    #endregion

    #region Filter DTOs

    /// <summary>
    /// User filter
    /// </summary>
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

    /// <summary>
    /// Activity log filter
    /// </summary>
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

    #endregion
}