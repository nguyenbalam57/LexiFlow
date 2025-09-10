using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Infrastructure.Data.Seed
{
    /// <summary>
    /// Extension method for using the database seeder
    /// </summary>
    public static class DatabaseSeederExtension
    {
        /// <summary>
        /// Seed database with improved error handling for model changes
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<DatabaseSeeder>>();
            var context = services.GetRequiredService<LexiFlowContext>();

            try
            {
                logger.LogInformation("Checking database status...");
                
                // Check if database exists first
                var canConnect = await context.Database.CanConnectAsync();
                if (!canConnect)
                {
                    logger.LogInformation("Database not found. Creating database...");
                    await context.Database.EnsureCreatedAsync();
                    logger.LogInformation("Database created successfully.");
                }
                else
                {
                    logger.LogInformation("Database connection established. Checking for migrations...");
                    
                    // Check for pending migrations
                    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                    if (pendingMigrations.Any())
                    {
                        logger.LogWarning("Phát hiện {Count} migration chưa áp dụng: {Migrations}", 
                            pendingMigrations.Count(), string.Join(", ", pendingMigrations));
                        
                        // Try to apply pending migrations
                        try
                        {
                            logger.LogInformation("Áp dụng migrations...");
                            await context.Database.MigrateAsync();
                            logger.LogInformation("Migrations áp dụng thành công.");
                        }
                        catch (InvalidOperationException ex) when (ex.Message.Contains("pending changes"))
                        {
                            logger.LogWarning("Có thay đổi model chưa được migrate. Ứng dụng sẽ tiếp tục với cảnh báo. " +
                                "Vui lòng chạy 'dotnet ef migrations add [MigrationName]' để tạo migration mới.");
                            
                            // Continue without migration - the warning is suppressed in OnConfiguring
                        }
                    }
                    else
                    {
                        logger.LogInformation("Database đã được cập nhật.");
                    }
                }

                // Proceed with seeding
                logger.LogInformation("Bắt đầu seed data...");
                var seeder = new DatabaseSeeder(logger, context);
                await seeder.SeedAsync();
                logger.LogInformation("Seed data hoàn thành thành công.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Đã xảy ra lỗi khi migrate hoặc seed database.");
                
                // If it's a pending model changes error, provide helpful guidance
                if (ex.Message.Contains("pending changes"))
                {
                    logger.LogError("=== HƯỚNG DẪN KHẮC PHỤC ===");
                    logger.LogError("Bạn đã thay đổi model Entity Framework nhưng chưa tạo migration.");
                    logger.LogError("Để khắc phục, hãy chạy lệnh sau:");
                    logger.LogError("dotnet ef migrations add FixNavigationProperties --project LexiFlow.Infrastructure --startup-project LexiFlow.API");
                    logger.LogError("Sau đó chạy: dotnet ef database update --project LexiFlow.Infrastructure --startup-project LexiFlow.API");
                    logger.LogError("========================");
                }
                
                throw;
            }
        }
    }
}
