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
        /// Seed database
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
                logger.LogInformation("Migrating database...");
                await context.Database.MigrateAsync();

                var seeder = new DatabaseSeeder(logger, context);
                await seeder.SeedAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                throw;
            }
        }
    }
}
