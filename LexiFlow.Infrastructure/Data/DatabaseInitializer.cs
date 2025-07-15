using LexiFlow.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Infrastructure.Data
{
    public class DatabaseInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var logger = services.GetRequiredService<ILogger<DatabaseInitializer>>();

                // Apply migrations if they haven't been applied
                //await context.Database.MigrateAsync();

                // Thay vào đó, chỉ kiểm tra kết nối
                var canConnect = await context.Database.CanConnectAsync();
                if (!canConnect)
                {
                    throw new Exception("Cannot connect to database. Please ensure database 'LexiFlow' exists.");
                }

                logger.LogInformation("Successfully connected to existing database.");

                // Seed additional data if needed
                await SeedDataAsync(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<DatabaseInitializer>>();
                logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }

        private static async Task SeedDataAsync(ApplicationDbContext context)
        {
            // Check if the database is already seeded with users beyond the admin
            if (await context.Users.CountAsync() > 1)
            {
                return; // Additional data already seeded
            }

            // Seed Groups if none exist
            if (!await context.Groups.AnyAsync())
            {
                var groups = new List<Group>
            {
                new Group { Name = "Administrators", Description = "System administrators with full access" },
                new Group { Name = "Teachers", Description = "Content creators and teachers" },
                new Group { Name = "Students", Description = "Regular users learning Japanese" }
            };

                await context.Groups.AddRangeAsync(groups);
                await context.SaveChangesAsync();
            }

            // Seed additional users if needed
            var users = new List<User>
        {
            new User
            {
                Username = "teacher",
                // Pre-hashed password: Teacher@123
                PasswordHash = "$2a$11$uijeaU3q7TQbz1UiQD0vJeGVNYXNFGEgrzM4QmAFgO9NvnS2BhE7S",
                Email = "teacher@lexiflow.com",
                FullName = "Test Teacher",
                IsActive = true,
                CreatedAt = DateTime.Now,
                RoleId = 2 // User role
            },
            new User
            {
                Username = "student",
                // Pre-hashed password: Student@123
                PasswordHash = "$2a$11$ICU0BfFyuWDK0SLYnIv.s.dMPh/NHgNuBdMwAj0fCB2ZFZ9X8Ylnu",
                Email = "student@lexiflow.com",
                FullName = "Test Student",
                IsActive = true,
                CreatedAt = DateTime.Now,
                RoleId = 2 // User role
            }
        };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            // Associate users with groups
            var userGroups = new List<UserGroup>
        {
            new UserGroup { UserId = 1, GroupId = 1 }, // admin - Administrators
            new UserGroup { UserId = 2, GroupId = 2 }, // teacher - Teachers
            new UserGroup { UserId = 3, GroupId = 3 }  // student - Students
        };

            await context.UserGroups.AddRangeAsync(userGroups);
            await context.SaveChangesAsync();
        }
    }
}
