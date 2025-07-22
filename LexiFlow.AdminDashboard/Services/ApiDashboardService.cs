using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LexiFlow.AdminDashboard.Models;
using LexiFlow.Core.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LexiFlow.AdminDashboard.Services
{
    public class ApiEndpoints
    {
        public string Dashboard { get; set; } = "api/admin/dashboard";
        public string Users { get; set; } = "api/admin/users";
        public string Roles { get; set; } = "api/admin/roles";
        public string Activities { get; set; } = "api/admin/activities";
        public string Settings { get; set; } = "api/admin/settings";
        public string Database { get; set; } = "api/admin/database";
        public string Logs { get; set; } = "api/admin/logs";
    }

    public class ApiDashboardService : IAdminDashboardService
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<ApiDashboardService> _logger;
        private readonly ApiEndpoints _endpoints;

        public ApiDashboardService(
            IApiClient apiClient,
            IOptions<ApiEndpoints> endpointsOptions,
            ILogger<ApiDashboardService> logger)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _endpoints = endpointsOptions.Value;
        }

        public async Task<DashboardStatistics> GetDashboardStatisticsAsync()
        {
            try
            {
                _logger.LogInformation("Getting dashboard statistics");
                var result = await _apiClient.GetAsync<DashboardStatistics>($"{_endpoints.Dashboard}/statistics");
                return result ?? new DashboardStatistics();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard statistics");
                throw;
            }
        }

        public async Task<UserManagementStats> GetUserManagementStatsAsync()
        {
            try
            {
                _logger.LogInformation("Getting user management statistics");
                var result = await _apiClient.GetAsync<UserManagementStats>($"{_endpoints.Dashboard}/user-stats");
                return result ?? new UserManagementStats();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user management statistics");
                throw;
            }
        }

        public async Task<List<User>> GetAllUsersAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                _logger.LogInformation("Getting all users: page {Page}, pageSize {PageSize}", page, pageSize);

                var queryParams = new Dictionary<string, string>
                {
                    ["page"] = page.ToString(),
                    ["pageSize"] = pageSize.ToString()
                };

                var result = await _apiClient.GetAsync<List<User>>(_endpoints.Users, queryParams);
                return result ?? new List<User>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                throw;
            }
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            try
            {
                _logger.LogInformation("Getting all roles");
                var result = await _apiClient.GetAsync<List<Role>>(_endpoints.Roles);
                return result ?? new List<Role>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all roles");
                throw;
            }
        }

        public async Task<bool> CreateUserAsync(User user, string password)
        {
            try
            {
                _logger.LogInformation("Creating user {Username}", user.Username);

                var request = new CreateUserRequest
                {
                    User = user,
                    Password = password
                };

                var result = await _apiClient.PostAsync<CreateUserRequest, ApiResponse>(_endpoints.Users, request);
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                _logger.LogInformation("Updating user {Username} (ID: {UserId})", user.Username, user.Id);
                var result = await _apiClient.PutAsync<User, ApiResponse>($"{_endpoints.Users}/{user.Id}", user);
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Deleting user {UserId}", userId);
                return await _apiClient.DeleteAsync(_endpoints.Users, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                throw;
            }
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Deactivating user {UserId}", userId);
                var result = await _apiClient.PostAsync<object, ApiResponse>(
                    $"{_endpoints.Users}/{userId}/deactivate",
                    new { });

                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating user");
                throw;
            }
        }

        public async Task<bool> ActivateUserAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Activating user {UserId}", userId);
                var result = await _apiClient.PostAsync<object, ApiResponse>(
                    $"{_endpoints.Users}/{userId}/activate",
                    new { });

                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating user");
                throw;
            }
        }

        public async Task<bool> ResetUserPasswordAsync(int userId, string newPassword)
        {
            try
            {
                _logger.LogInformation("Resetting password for user {UserId}", userId);

                var request = new ResetPasswordRequest
                {
                    NewPassword = newPassword
                };

                var result = await _apiClient.PostAsync<ResetPasswordRequest, ApiResponse>(
                    $"{_endpoints.Users}/{userId}/reset-password",
                    request);

                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting user password");
                throw;
            }
        }

        public async Task<bool> AssignUserToRoleAsync(int userId, int roleId)
        {
            try
            {
                _logger.LogInformation("Assigning user {UserId} to role {RoleId}", userId, roleId);

                var request = new AssignRoleRequest
                {
                    UserId = userId,
                    RoleId = roleId
                };

                var result = await _apiClient.PostAsync<AssignRoleRequest, ApiResponse>(
                    $"{_endpoints.Users}/assign-role",
                    request);

                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning user to role");
                throw;
            }
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Getting user {UserId}", userId);
                return await _apiClient.GetAsync<User>($"{_endpoints.Users}/{userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID");
                throw;
            }
        }

        public async Task<List<UserActivity>> GetRecentUserActivitiesAsync(int count = 50)
        {
            try
            {
                _logger.LogInformation("Getting recent user activities (count: {Count})", count);

                var queryParams = new Dictionary<string, string>
                {
                    ["count"] = count.ToString()
                };

                var result = await _apiClient.GetAsync<List<UserActivity>>(_endpoints.Activities, queryParams);
                return result ?? new List<UserActivity>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent user activities");
                throw;
            }
        }

        public async Task<SystemSettings> GetSystemSettingsAsync()
        {
            try
            {
                _logger.LogInformation("Getting system settings");
                var result = await _apiClient.GetAsync<SystemSettings>(_endpoints.Settings);
                return result ?? new SystemSettings();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting system settings");
                throw;
            }
        }

        public async Task<bool> UpdateSystemSettingsAsync(SystemSettings settings)
        {
            try
            {
                _logger.LogInformation("Updating system settings");
                var result = await _apiClient.PutAsync<SystemSettings, ApiResponse>(_endpoints.Settings, settings);
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating system settings");
                throw;
            }
        }

        public async Task<bool> BackupDatabaseAsync(string path)
        {
            try
            {
                _logger.LogInformation("Creating database backup to {Path}", path);

                var request = new BackupRequest
                {
                    Path = path
                };

                var result = await _apiClient.PostAsync<BackupRequest, ApiResponse>(
                    $"{_endpoints.Database}/backup",
                    request);

                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating database backup");
                throw;
            }
        }

        public async Task<bool> RestoreDatabaseAsync(string backupPath)
        {
            try
            {
                _logger.LogInformation("Restoring database from {BackupPath}", backupPath);

                var request = new RestoreRequest
                {
                    BackupPath = backupPath
                };

                var result = await _apiClient.PostAsync<RestoreRequest, ApiResponse>(
                    $"{_endpoints.Database}/restore",
                    request);

                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restoring database");
                throw;
            }
        }

        public async Task<List<string>> GetDatabaseBackupsAsync()
        {
            try
            {
                _logger.LogInformation("Getting database backups");
                var result = await _apiClient.GetAsync<List<string>>($"{_endpoints.Database}/backups");
                return result ?? new List<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting database backups");
                throw;
            }
        }

        public async Task<List<string>> GetActivityLogsAsync(DateTime from, DateTime to, string? module = null, string? action = null)
        {
            try
            {
                _logger.LogInformation("Getting activity logs from {From} to {To}", from, to);

                var queryParams = new Dictionary<string, string>
                {
                    ["from"] = from.ToString("o"),
                    ["to"] = to.ToString("o")
                };

                if (!string.IsNullOrEmpty(module))
                {
                    queryParams.Add("module", module);
                }

                if (!string.IsNullOrEmpty(action))
                {
                    queryParams.Add("action", action);
                }

                var result = await _apiClient.GetAsync<List<string>>(_endpoints.Logs, queryParams);
                return result ?? new List<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting activity logs");
                throw;
            }
        }

        public async Task ClearActivityLogsAsync(DateTime before)
        {
            try
            {
                _logger.LogInformation("Clearing activity logs before {Before}", before);

                var request = new ClearLogsRequest
                {
                    Before = before
                };

                await _apiClient.PostAsync<ClearLogsRequest, ApiResponse>(
                    $"{_endpoints.Logs}/clear",
                    request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing activity logs");
                throw;
            }
        }

        public async Task<bool> TestEmailSettingsAsync(string smtpServer, int port, string username, string password, bool useSsl)
        {
            try
            {
                _logger.LogInformation("Testing email settings for server {SmtpServer}", smtpServer);

                var request = new EmailSettingsRequest
                {
                    SmtpServer = smtpServer,
                    Port = port,
                    Username = username,
                    Password = password,
                    UseSsl = useSsl
                };

                var result = await _apiClient.PostAsync<EmailSettingsRequest, ApiResponse>(
                    $"{_endpoints.Settings}/test-email",
                    request);

                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error testing email settings");
                throw;
            }
        }

        public async Task<bool> TestDatabaseConnectionAsync(string connectionString)
        {
            try
            {
                _logger.LogInformation("Testing database connection");

                var request = new ConnectionTestRequest
                {
                    ConnectionString = connectionString
                };

                var result = await _apiClient.PostAsync<ConnectionTestRequest, ApiResponse>(
                    $"{_endpoints.Database}/test-connection",
                    request);

                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error testing database connection");
                throw;
            }
        }
    }

    // Request/Response models for the API
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }

    public class CreateUserRequest
    {
        public User User { get; set; } = new User();
        public string Password { get; set; } = string.Empty;
    }

    public class ResetPasswordRequest
    {
        public string NewPassword { get; set; } = string.Empty;
    }

    public class AssignRoleRequest
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

    public class BackupRequest
    {
        public string Path { get; set; } = string.Empty;
    }

    public class RestoreRequest
    {
        public string BackupPath { get; set; } = string.Empty;
    }

    public class ClearLogsRequest
    {
        public DateTime Before { get; set; }
    }

    public class EmailSettingsRequest
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool UseSsl { get; set; }
    }

    public class ConnectionTestRequest
    {
        public string ConnectionString { get; set; } = string.Empty;
    }
}