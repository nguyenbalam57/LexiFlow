using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LexiFlow.Infrastructure.Migrations
{
    /// <summary>
    /// Manages database migrations and version control
    /// </summary>
    public class DbMigrationManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DbMigrationManager> _logger;

        public DbMigrationManager(
            IServiceProvider serviceProvider,
            ILogger<DbMigrationManager> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Checks if the database needs migration
        /// </summary>
        public async Task<bool> NeedsMigrationAsync()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Check if any migrations are pending
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

                return pendingMigrations.Any();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking for pending migrations");
                throw;
            }
        }

        /// <summary>
        /// Migrates the database to the latest version
        /// </summary>
        public async Task MigrateAsync()
        {
            try
            {
                _logger.LogInformation("Starting database migration");

                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Apply migrations
                await dbContext.Database.MigrateAsync();

                _logger.LogInformation("Database migration completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during database migration");
                throw;
            }
        }

        /// <summary>
        /// Gets migration history
        /// </summary>
        public async Task<List<MigrationInfo>> GetMigrationHistoryAsync()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Get applied migrations
                var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();

                // Get all available migrations
                var availableMigrations = dbContext.Database.GetMigrations();

                // Get pending migrations
                var pendingMigrations = availableMigrations.Except(appliedMigrations);

                // Create migration info list
                var migrationHistory = new List<MigrationInfo>();

                // Add applied migrations
                foreach (var migration in appliedMigrations)
                {
                    migrationHistory.Add(new MigrationInfo
                    {
                        Id = migration,
                        Name = GetMigrationName(migration),
                        Status = MigrationStatus.Applied
                    });
                }

                // Add pending migrations
                foreach (var migration in pendingMigrations)
                {
                    migrationHistory.Add(new MigrationInfo
                    {
                        Id = migration,
                        Name = GetMigrationName(migration),
                        Status = MigrationStatus.Pending
                    });
                }

                return migrationHistory.OrderBy(m => m.Id).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting migration history");
                throw;
            }
        }

        /// <summary>
        /// Gets the database version
        /// </summary>
        public async Task<string> GetDatabaseVersionAsync()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Check if database exists
                if (!await dbContext.Database.CanConnectAsync())
                {
                    return "Database not found";
                }

                // Get applied migrations
                var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();

                // Return the latest applied migration as version
                return appliedMigrations.Any()
                    ? GetMigrationName(appliedMigrations.Last())
                    : "No migrations applied";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting database version");
                throw;
            }
        }

        /// <summary>
        /// Gets a user-friendly name from migration ID
        /// </summary>
        private string GetMigrationName(string migrationId)
        {
            // Migration IDs are typically in the format "YYYYMMDDHHMMSS_MigrationName"
            // Extract the timestamp and name
            var parts = migrationId.Split('_', 2);

            if (parts.Length < 2)
            {
                return migrationId;
            }

            // Parse timestamp
            if (DateTime.TryParseExact(parts[0], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var timestamp))
            {
                // Format timestamp and combine with name
                return $"{timestamp:yyyy-MM-dd HH:mm:ss} - {FormatMigrationName(parts[1])}";
            }

            // Format name only if timestamp parsing fails
            return FormatMigrationName(parts[1]);
        }

        /// <summary>
        /// Formats a migration name to be more user-friendly
        /// </summary>
        private string FormatMigrationName(string name)
        {
            // Replace camel/pascal case with spaces
            var formatted = System.Text.RegularExpressions.Regex.Replace(name, "([a-z])([A-Z])", "$1 $2");

            // Replace underscores with spaces
            formatted = formatted.Replace('_', ' ');

            // Capitalize first letter
            if (!string.IsNullOrEmpty(formatted))
            {
                formatted = char.ToUpper(formatted[0]) + formatted.Substring(1);
            }

            return formatted;
        }
    }

    /// <summary>
    /// Represents a database migration
    /// </summary>
    public class MigrationInfo
    {
        /// <summary>
        /// Migration ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// User-friendly name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Migration status
        /// </summary>
        public MigrationStatus Status { get; set; }

        /// <summary>
        /// Applied timestamp (if available)
        /// </summary>
        public DateTime? AppliedAt { get; set; }
    }

    /// <summary>
    /// Migration status
    /// </summary>
    public enum MigrationStatus
    {
        /// <summary>
        /// Migration is pending
        /// </summary>
        Pending,

        /// <summary>
        /// Migration has been applied
        /// </summary>
        Applied
    }
}