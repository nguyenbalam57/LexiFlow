using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LexiFlow.AdminDashboard.Models;
using LexiFlow.Core.Entities;
using Microsoft.Extensions.Logging;

namespace LexiFlow.AdminDashboard.Services
{
    /// <summary>
    /// Decorator for IAdminDashboardService that adds caching capabilities
    /// Uses the Decorator pattern to wrap an existing implementation
    /// </summary>
    public class CachingDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardService _dashboardService;
        private readonly IApiCacheService _cacheService;
        private readonly ILogger<CachingDashboardService> _logger;

        public CachingDashboardService(
            IAdminDashboardService dashboardService,
            IApiCacheService cacheService,
            ILogger<CachingDashboardService> logger)
        {
            _dashboardService = dashboardService;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<DashboardStatistics> GetDashboardStatisticsAsync()
        {
            string cacheKey = ApiCacheServiceExtensions.GenerateCacheKey<DashboardStatistics>("GetDashboardStatistics");

            return await _cacheService.GetOrAddAsync(
                cacheKey,
                () => _dashboardService.GetDashboardStatisticsAsync(),
                TimeSpan.FromMinutes(5));
        }

        public async Task<UserManagementStats> GetUserManagementStatsAsync()
        {
            string cacheKey = ApiCacheServiceExtensions.GenerateCacheKey<UserManagementStats>("GetUserManagementStats");

            return await _cacheService.GetOrAddAsync(
                cacheKey,
                () => _dashboardService.GetUserManagementStatsAsync(),
                TimeSpan.FromMinutes(5));
        }

        public async Task<List<User>> GetAllUsersAsync(int page = 1, int pageSize = 20)
        {
            string cacheKey = ApiCacheServiceExtensions.GenerateCacheKey<List<User>>("GetAllUsers", page, pageSize);

            return await _cacheService.GetOrAddAsync(
                cacheKey,
                () => _dashboardService.GetAllUsersAsync(page, pageSize),
                TimeSpan.FromMinutes(5));
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            string cacheKey = ApiCacheServiceExtensions.GenerateCacheKey<List<Role>>("GetAllRoles");

            return await _cacheService.GetOrAddAsync(
                cacheKey,
                () => _dashboardService.GetAllRolesAsync(),
                TimeSpan.FromMinutes(10)); // Roles change less frequently
        }

        public async Task<bool> CreateUserAsync(User user, string password)
        {
            // After successful creation, invalidate user-related caches
            bool result = await _dashboardService.CreateUserAsync(user, password);

            if (result)
            {
                InvalidateUserCaches();
            }

            return result;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            // After successful update, invalidate user-related caches
            bool result = await _dashboardService.UpdateUserAsync(user);

            if (result)
            {
                InvalidateUserCaches();
                _cacheService.Remove(ApiCacheServiceExtensions.GenerateCacheKey<User>("GetUserById", user.Id));
            }

            return result;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            // After successful deletion, invalidate user-related caches
            bool result = await _dashboardService.DeleteUserAsync(userId);

            if (result)
            {
                InvalidateUserCaches();
                _cacheService.Remove(ApiCacheServiceExtensions.GenerateCacheKey<User>("GetUserById", userId));
            }

            return result;
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            // After successful deactivation, invalidate user-related caches
            bool result = await _dashboardService.DeactivateUserAsync(userId);

            if (result)
            {
                InvalidateUserCaches();
                _cacheService.Remove(ApiCacheServiceExtensions.GenerateCacheKey<User>("GetUserById", userId));
            }

            return result;
        }

        public async Task<bool> ActivateUserAsync(int userId)
        {
            // After successful activation, invalidate user-related caches
            bool result = await _dashboardService.ActivateUserAsync(userId);

            if (result)
            {
                InvalidateUserCaches();
                _cacheService.Remove(ApiCacheServiceExtensions.GenerateCacheKey<User>("GetUserById", userId));
            }

            return result;
        }

        public async Task<bool> ResetUserPasswordAsync(int userId, string newPassword)
        {
            // Password resets don't affect user listings, but we should invalidate the specific user
            bool result = await _dashboardService.ResetUserPasswordAsync(userId, newPassword);

            if (result)
            {
                _cacheService.Remove(ApiCacheServiceExtensions.GenerateCacheKey<User>("GetUserById", userId));
            }

            return result;
        }

        public async Task<bool> AssignUserToRoleAsync(int userId, int roleId)
        {
            // After role assignment, invalidate user and role caches
            bool result = await _dashboardService.AssignUserToRoleAsync(userId, roleId);

            if (result)
            {
                InvalidateUserCaches();
                InvalidateRoleCaches();
                _cacheService.Remove(ApiCacheServiceExtensions.GenerateCacheKey<User>("GetUserById", userId));
            }

            return result;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            string cacheKey = ApiCacheServiceExtensions.GenerateCacheKey<User>("GetUserById", userId);

            return await _cacheService.GetOrAddAsync(
                cacheKey,
                () => _dashboardService.GetUserByIdAsync(userId),
                TimeSpan.FromMinutes(5));
        }

        public async Task<List<UserActivity>> GetRecentUserActivitiesAsync(int count = 50)
        {
            // User activities change frequently, so short cache time
            string cacheKey = ApiCacheServiceExtensions.GenerateCacheKey<List<UserActivity>>("GetRecentUserActivities", count);

            return await _cacheService.GetOrAddAsync(
                cacheKey,
                () => _dashboardService.GetRecentUserActivitiesAsync(count),
                TimeSpan.FromMinutes(1));
        }

        public async Task<SystemSettings> GetSystemSettingsAsync()
        {
            string cacheKey = ApiCacheServiceExtensions.GenerateCacheKey<SystemSettings>("GetSystemSettings");

            return await _cacheService.GetOrAddAsync(
                cacheKey,
                () => _dashboardService.GetSystemSettingsAsync(),
                TimeSpan.FromMinutes(15)); // Settings change infrequently
        }

        public async Task<bool> UpdateSystemSettingsAsync(SystemSettings settings)
        {
            // After updating settings, invalidate settings cache
            bool result = await _dashboardService.UpdateSystemSettingsAsync(settings);

            if (result)
            {
                _cacheService.Remove(ApiCacheServiceExtensions.GenerateCacheKey<SystemSettings>("GetSystemSettings"));
            }

            return result;
        }

        public Task<bool> BackupDatabaseAsync(string path)
        {
            // No caching needed for this operation
            return _dashboardService.BackupDatabaseAsync(path);
        }

        public Task<bool> RestoreDatabaseAsync(string backupPath)
        {
            // After database restore, clear all caches
            var result = _dashboardService.RestoreDatabaseAsync(backupPath);
            _cacheService.Clear();
            return result;
        }

        public async Task<List<string>> GetDatabaseBackupsAsync()
        {
            string cacheKey = ApiCacheServiceExtensions.GenerateCacheKey<List<string>>("GetDatabaseBackups");

            return await _cacheService.GetOrAddAsync(
                cacheKey,
                () => _dashboardService.GetDatabaseBackupsAsync(),
                TimeSpan.FromMinutes(5));
        }

        public Task<List<string>> GetActivityLogsAsync(DateTime from, DateTime to, string? module = null, string? action = null)
        {
            // Activity logs are typically queried with different parameters, so caching is less effective
            return _dashboardService.GetActivityLogsAsync(from, to, module, action);
        }

        public Task ClearActivityLogsAsync(DateTime before)
        {
            // After clearing logs, invalidate activity-related caches
            var result = _dashboardService.ClearActivityLogsAsync(before);

            InvalidateActivityCaches();

            return result;
        }

        public Task<bool> TestEmailSettingsAsync(string smtpServer, int port, string username, string password, bool useSsl)
        {
            // No caching needed for this operation
            return _dashboardService.TestEmailSettingsAsync(smtpServer, port, username, password, useSsl);
        }

        public Task<bool> TestDatabaseConnectionAsync(string connectionString)
        {
            // No caching needed for this operation
            return _dashboardService.TestDatabaseConnectionAsync(connectionString);
        }

        // Helper methods to invalidate related caches

        private void InvalidateUserCaches()
        {
            _logger.LogDebug("Invalidating user-related caches");

            // Clear dashboard statistics (includes user counts)
            _cacheService.Remove(ApiCacheServiceExtensions.GenerateCacheKey<DashboardStatistics>("GetDashboardStatistics"));

            // Clear user management statistics
            _cacheService.Remove(ApiCacheServiceExtensions.GenerateCacheKey<UserManagementStats>("GetUserManagementStats"));

            // Clear user list caches - since we don't know which pages are cached, we clear all caches
            _cacheService.Clear();
        }

        private void InvalidateRoleCaches()
        {
            _logger.LogDebug("Invalidating role-related caches");

            // Clear roles list
            _cacheService.Remove(ApiCacheServiceExtensions.GenerateCacheKey<List<Role>>("GetAllRoles"));
        }

        private void InvalidateActivityCaches()
        {
            _logger.LogDebug("Invalidating activity-related caches");

            // Clear recent activities
            for (int i = 1; i <= 100; i++)
            {
                _cacheService.Remove(ApiCacheServiceExtensions.GenerateCacheKey<List<UserActivity>>("GetRecentUserActivities", i));
            }
        }
    }
}