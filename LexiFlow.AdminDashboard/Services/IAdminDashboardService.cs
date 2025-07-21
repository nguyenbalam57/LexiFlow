using LexiFlow.AdminDashboard.Models;
using LexiFlow.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.AdminDashboard.Services
{
    public interface IAdminDashboardService
    {
        Task<DashboardStatistics> GetDashboardStatisticsAsync();
        Task<UserManagementStats> GetUserManagementStatsAsync();
        Task<List<User>> GetAllUsersAsync(int page = 1, int pageSize = 20);
        Task<List<Role>> GetAllRolesAsync();
        Task<bool> CreateUserAsync(User user, string password);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> DeactivateUserAsync(int userId);
        Task<bool> ActivateUserAsync(int userId);
        Task<bool> ResetUserPasswordAsync(int userId, string newPassword);
        Task<bool> AssignUserToRoleAsync(int userId, int roleId);
        Task<User?> GetUserByIdAsync(int userId);
        Task<List<UserActivity>> GetRecentUserActivitiesAsync(int count = 50);
        Task<SystemSettings> GetSystemSettingsAsync();
        Task<bool> UpdateSystemSettingsAsync(SystemSettings settings);
        Task<bool> BackupDatabaseAsync(string path);
        Task<bool> RestoreDatabaseAsync(string backupPath);
        Task<List<string>> GetDatabaseBackupsAsync();
        Task<List<string>> GetActivityLogsAsync(DateTime from, DateTime to, string? module = null, string? action = null);
        Task ClearActivityLogsAsync(DateTime before);
        Task<bool> TestEmailSettingsAsync(string smtpServer, int port, string username, string password, bool useSsl);
        Task<bool> TestDatabaseConnectionAsync(string connectionString);
    }
}
