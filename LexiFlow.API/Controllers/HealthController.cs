using LexiFlow.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LexiFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly LexiFlowContext _context;
        private readonly ILogger<HealthController> _logger;

        public HealthController(LexiFlowContext context, ILogger<HealthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Ki?m tra tr?ng thái k?t n?i database
        /// </summary>
        [HttpGet("database")]
        public async Task<IActionResult> CheckDatabase()
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                
                // Test connection
                var canConnect = await _context.Database.CanConnectAsync();
                stopwatch.Stop();

                if (!canConnect)
                {
                    return StatusCode(503, new
                    {
                        status = "Unhealthy",
                        message = "Không th? k?t n?i ??n database",
                        responseTime = stopwatch.ElapsedMilliseconds
                    });
                }

                // Get database info
                var connectionString = _context.Database.GetConnectionString();
                var databaseName = _context.Database.GetDbConnection().Database;

                // Count some basic entities to verify data
                var userCount = await _context.Users.CountAsync();
                var vocabularyCount = await _context.Vocabularies.CountAsync();
                var kanjiCount = await _context.Kanjis.CountAsync();

                return Ok(new
                {
                    status = "Healthy",
                    message = "Database connection thành công",
                    responseTime = stopwatch.ElapsedMilliseconds,
                    databaseInfo = new
                    {
                        name = databaseName,
                        connectionString = MaskConnectionString(connectionString),
                        statistics = new
                        {
                            users = userCount,
                            vocabularies = vocabularyCount,
                            kanjis = kanjiCount
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database health check failed");
                return StatusCode(503, new
                {
                    status = "Unhealthy",
                    message = $"Database error: {ex.Message}",
                    error = ex.GetType().Name
                });
            }
        }

        /// <summary>
        /// Ki?m tra t?ng quát API
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> HealthCheck()
        {
            try
            {
                var dbHealthy = await _context.Database.CanConnectAsync();

                return Ok(new
                {
                    status = dbHealthy ? "Healthy" : "Degraded",
                    timestamp = DateTime.UtcNow,
                    version = "1.0.0",
                    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                    services = new
                    {
                        database = dbHealthy ? "Connected" : "Disconnected",
                        api = "Running"
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return StatusCode(503, new
                {
                    status = "Unhealthy",
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// L?y thông tin chi ti?t v? database schema
        /// </summary>
        [HttpGet("database/schema")]
        public async Task<IActionResult> GetDatabaseSchema()
        {
            try
            {
                var tables = await _context.Database.SqlQueryRaw<string>(
                    "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME")
                    .ToListAsync();

                var migrations = await _context.Database.SqlQueryRaw<string>(
                    "SELECT MigrationId FROM __EFMigrationsHistory ORDER BY MigrationId")
                    .ToListAsync();

                return Ok(new
                {
                    status = "Success",
                    tableCount = tables.Count,
                    tables = tables,
                    migrationCount = migrations.Count,
                    lastMigration = migrations.LastOrDefault()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get database schema");
                return StatusCode(500, new
                {
                    status = "Error",
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Test t?o và xóa d? li?u m?u
        /// </summary>
        [HttpPost("database/test-crud")]
        public async Task<IActionResult> TestCrudOperations()
        {
            try
            {
                var testCategory = new Models.Learning.Vocabulary.Category
                {
                    CategoryName = "Test Category " + Guid.NewGuid().ToString("N")[..8],
                    Description = "Test category for health check",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Create
                _context.Categories.Add(testCategory);
                await _context.SaveChangesAsync();

                // Read
                var created = await _context.Categories.FindAsync(testCategory.CategoryId);
                if (created == null)
                {
                    return StatusCode(500, new { status = "Error", message = "Failed to read created data" });
                }

                // Update
                created.Description = "Updated description";
                await _context.SaveChangesAsync();

                // Delete
                _context.Categories.Remove(created);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    status = "Success",
                    message = "CRUD operations completed successfully",
                    testData = new
                    {
                        id = testCategory.CategoryId,
                        name = testCategory.CategoryName,
                        operations = new[] { "Create", "Read", "Update", "Delete" }
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CRUD test failed");
                return StatusCode(500, new
                {
                    status = "Error",
                    message = ex.Message
                });
            }
        }

        private static string MaskConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return "";

            // Mask sensitive information
            var parts = connectionString.Split(';');
            var masked = new List<string>();

            foreach (var part in parts)
            {
                if (part.ToLower().Contains("password") || part.ToLower().Contains("pwd"))
                {
                    var keyValue = part.Split('=');
                    if (keyValue.Length == 2)
                    {
                        masked.Add($"{keyValue[0]}=***");
                    }
                    else
                    {
                        masked.Add("***");
                    }
                }
                else
                {
                    masked.Add(part);
                }
            }

            return string.Join(";", masked);
        }
    }
}