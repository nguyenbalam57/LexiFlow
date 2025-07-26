using System;
using System.Collections.Generic;

namespace LexiFlow.API.Models
{

    /// <summary>
    /// Paginated result DTO
    /// </summary>
    public class PaginatedResultDto<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }


    /// <summary>
    /// Role DTO
    /// </summary>
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSystemRole { get; set; }
        public List<string> Permissions { get; set; } = new List<string>();
        public int UsersCount { get; set; }
    }

    /// <summary>
    /// User activity DTO
    /// </summary>
    public class UserActivityDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }
        public string IpAddress { get; set; }
    }

    /// <summary>
    /// Reset password DTO
    /// </summary>
    public class ResetPasswordDto
    {
        public string NewPassword { get; set; }
    }

    /// <summary>
    /// Assign role DTO
    /// </summary>
    public class AssignRoleDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

    /// <summary>
    /// Monthly stats DTO
    /// </summary>
    public class MonthlyStatsDto
    {
        public DateTime Month { get; set; }
        public int Value { get; set; }
        public string Label { get; set; }
    }

    /// <summary>
    /// Dashboard statistics DTO
    /// </summary>
    public class DashboardStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int TotalContent { get; set; }
        public int ContentAddedThisMonth { get; set; }
        public List<MonthlyStatsDto> MonthlyUserStats { get; set; } = new List<MonthlyStatsDto>();
        public Dictionary<string, int> ContentByCategory { get; set; } = new Dictionary<string, int>();
        public List<UserActivityDto> RecentUserActivities { get; set; } = new List<UserActivityDto>();
    }

    /// <summary>
    /// User management stats DTO
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
    }

    /// <summary>
    /// System settings DTO
    /// </summary>
    public class SystemSettingsDto
    {
        public string ApplicationName { get; set; }
        public string CompanyName { get; set; }
        public string SupportEmail { get; set; }
        public string DefaultLanguage { get; set; }
        public string DefaultTimeZone { get; set; }
        public bool EnableRegistration { get; set; }
        public bool RequireEmailVerification { get; set; }
        public int MinimumPasswordLength { get; set; }
        public bool RequirePasswordComplexity { get; set; }
        public int SessionTimeoutMinutes { get; set; }
    }
}