using LexiFlow.AdminDashboard.Models;
using LexiFlow.AdminDashboard.Services;
using LexiFlow.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace LexiFlow.AdminDashboard
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAdminDashboardServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register options
            services.Configure<ApiClientOptions>(
                configuration.GetSection("ApiClient"));

            services.Configure<ApiEndpoints>(
                configuration.GetSection("ApiEndpoints"));

            // Register API client
            services.AddSingleton<IApiClient, ApiClient>();

            // Register service implementations
            services.AddScoped<IAdminDashboardService, ApiDashboardService>();

            // Register ViewModels
            services.AddViewModels();

            return services;
        }

        private static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            // Register all ViewModels here
            // This helps manage ViewModels in a centralized way

            // Example (add your actual ViewModels):
            // services.AddTransient<AdminDashboardViewModel>();
            // services.AddTransient<UserManagementViewModel>();
            // services.AddTransient<SystemSettingsViewModel>();

            return services;
        }

        /// <summary>
        /// Adds offline mode services (for development/testing without API connection)
        /// </summary>
        public static IServiceCollection AddAdminDashboardOfflineServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register mock service implementation
            services.AddScoped<IAdminDashboardService, MockDashboardService>();

            // Register ViewModels
            services.AddViewModels();

            return services;
        }
    }

    /// <summary>
    /// Mock implementation of IAdminDashboardService for offline/testing use
    /// </summary>
    public class MockDashboardService : IAdminDashboardService
    {
        private readonly ILogger<MockDashboardService> _logger;

        public MockDashboardService(ILogger<MockDashboardService> logger)
        {
            _logger = logger;
        }

        // Implementation of all interface methods with mock data
        // Implement these methods as needed for testing/development

        public Task<DashboardStatistics> GetDashboardStatisticsAsync()
        {
            var stats = new DashboardStatistics
            {
                TotalUsers = 150,
                ActiveUsers = 120,
                NewUsersThisMonth = 15,
                TotalContent = 2500,
                RecentUserActivities = GetMockUserActivities(10),
                // Add other mock statistics
            };

            return Task.FromResult(stats);
        }

        public Task<UserManagementStats> GetUserManagementStatsAsync()
        {
            var stats = new UserManagementStats
            {
                TotalUsers = 150,
                ActiveUsers = 120,
                InactiveUsers = 30,
                AdminUsers = 5
                // Add other mock statistics
            };

            return Task.FromResult(stats);
        }

        public Task<List<User>> GetAllUsersAsync(int page = 1, int pageSize = 20)
        {
            var users = new List<User>();

            // Create some mock users
            for (int i = 1; i <= pageSize; i++)
            {
                int userId = (page - 1) * pageSize + i;
                users.Add(new User
                {
                    Id = userId,
                    Username = $"user{userId}",
                    Email = $"user{userId}@example.com",
                    FirstName = $"First{userId}",
                    LastName = $"Last{userId}",
                    IsActive = userId % 5 != 0, // Every 5th user is inactive
                    CreatedAt = DateTime.Now.AddDays(-userId)
                });
            }

            return Task.FromResult(users);
        }

        // Implement other methods with mock data...

        // Helper method to generate mock user activities
        private List<UserActivity> GetMockUserActivities(int count)
        {
            var activities = new List<UserActivity>();

            for (int i = 1; i <= count; i++)
            {
                activities.Add(new UserActivity
                {
                    Id = i,
                    UserId = i % 10 + 1,
                    Username = $"user{i % 10 + 1}",
                    Action = GetRandomAction(),
                    Module = GetRandomModule(),
                    Timestamp = DateTime.Now.AddHours(-i),
                    Details = $"Mock activity details {i}"
                });
            }

            return activities;
        }

        private string GetRandomAction()
        {
            string[] actions = { "Login", "Logout", "Create", "Update", "Delete", "Export", "Import", "View" };
            return actions[new Random().Next(actions.Length)];
        }

        private string GetRandomModule()
        {
            string[] modules = { "User", "Content", "Settings", "Dashboard", "Report", "System" };
            return modules[new Random().Next(modules.Length)];
        }

        // Implement all remaining methods from IAdminDashboardService with mock data
        public Task<List<Role>> GetAllRolesAsync() =>
            Task.FromResult(new List<Role>());

        public Task<bool> CreateUserAsync(User user, string password) =>
            Task.FromResult(true);

        public Task<bool> UpdateUserAsync(User user) =>
            Task.FromResult(true);

        public Task<bool> DeleteUserAsync(int userId) =>
            Task.FromResult(true);

        public Task<bool> DeactivateUserAsync(int userId) =>
            Task.FromResult(true);

        public Task<bool> ActivateUserAsync(int userId) =>
            Task.FromResult(true);

        public Task<bool> ResetUserPasswordAsync(int userId, string newPassword) =>
            Task.FromResult(true);

        public Task<bool> AssignUserToRoleAsync(int userId, int roleId) =>
            Task.FromResult(true);

        public Task<User?> GetUserByIdAsync(int userId) =>
            Task.FromResult<User?>(new User { Id = userId, Username = $"user{userId}" });

        public Task<List<UserActivity>> GetRecentUserActivitiesAsync(int count = 50) =>
            Task.FromResult(GetMockUserActivities(count));

        public Task<SystemSettings> GetSystemSettingsAsync() =>
            Task.FromResult(new SystemSettings());

        public Task<bool> UpdateSystemSettingsAsync(SystemSettings settings) =>
            Task.FromResult(true);

        public Task<bool> BackupDatabaseAsync(string path) =>
            Task.FromResult(true);

        public Task<bool> RestoreDatabaseAsync(string backupPath) =>
            Task.FromResult(true);

        public Task<List<string>> GetDatabaseBackupsAsync() =>
            Task.FromResult(new List<string> { "backup1.bak", "backup2.bak" });

        public Task<List<string>> GetActivityLogsAsync(DateTime from, DateTime to, string? module = null, string? action = null) =>
            Task.FromResult(new List<string> { "Log entry 1", "Log entry 2" });

        public Task ClearActivityLogsAsync(DateTime before) =>
            Task.CompletedTask;

        public Task<bool> TestEmailSettingsAsync(string smtpServer, int port, string username, string password, bool useSsl) =>
            Task.FromResult(true);

        public Task<bool> TestDatabaseConnectionAsync(string connectionString) =>
            Task.FromResult(true);
    }
}