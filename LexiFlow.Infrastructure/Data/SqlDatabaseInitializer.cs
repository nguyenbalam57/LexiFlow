using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace LexiFlow.Infrastructure.Data
{
    public class SqlDatabaseInitializer
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SqlDatabaseInitializer> _logger;

        public SqlDatabaseInitializer(IConfiguration configuration, ILogger<SqlDatabaseInitializer> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> InitializeDatabaseAsync()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

                // First check if SQL Server is accessible
                if (!await CheckSqlServerConnectionAsync(connectionString))
                {
                    _logger.LogError("Cannot connect to SQL Server. Please ensure SQL Server Express is installed and running.");
                    return false;
                }

                // Tạo chuỗi kết nối tới master database để có thể tạo database mới
                var builder = new SqlConnectionStringBuilder(connectionString);
                string databaseName = builder.InitialCatalog;
                builder.InitialCatalog = "master";
                string masterConnectionString = builder.ConnectionString;

                // Check and create database if it doesn't exist
                bool databaseExists = await DatabaseExistsAsync(masterConnectionString, databaseName);

                if (!databaseExists)
                {
                    _logger.LogInformation($"Database '{databaseName}' does not exist. Creating...");
                    await CreateDatabaseAsync(masterConnectionString, databaseName);
                    _logger.LogInformation($"Database '{databaseName}' created successfully.");
                }
                else
                {
                    _logger.LogInformation($"Database '{databaseName}' already exists.");
                }

                // Check if tables exist
                bool tablesExist = await CheckTablesExistAsync(connectionString);

                if (!tablesExist)
                {
                    _logger.LogInformation("Tables do not exist. Creating database schema...");
                    await ExecuteSqlScriptAsync(connectionString);
                    _logger.LogInformation("Database schema created successfully.");
                }
                else
                {
                    _logger.LogInformation("Database tables already exist.");
                }

                // Insert or update initial data
                await EnsureInitialDataAsync(connectionString);

                _logger.LogInformation("Database initialized successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database.");
                return false;
            }
        }

        private async Task<bool> CheckSqlServerConnectionAsync(string connectionString)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString)
                {
                    InitialCatalog = "master",
                    ConnectTimeout = 5
                };

                using var connection = new SqlConnection(builder.ConnectionString);
                await connection.OpenAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to SQL Server");
                return false;
            }
        }

        private async Task<bool> DatabaseExistsAsync(string masterConnectionString, string databaseName)
        {
            using var connection = new SqlConnection(masterConnectionString);
            await connection.OpenAsync();

            string query = "SELECT COUNT(*) FROM sys.databases WHERE name = @dbName";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@dbName", databaseName);

            int count = Convert.ToInt32(await command.ExecuteScalarAsync());
            return count > 0;
        }

        private async Task CreateDatabaseAsync(string masterConnectionString, string databaseName)
        {
            using var connection = new SqlConnection(masterConnectionString);
            await connection.OpenAsync();

            string query = $"CREATE DATABASE [{databaseName}]";
            using var command = new SqlCommand(query, connection);
            await command.ExecuteNonQueryAsync();

            // Wait a moment for database creation
            await Task.Delay(1000);
        }

        private async Task<bool> CheckTablesExistAsync(string connectionString)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                string query = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_TYPE = 'BASE TABLE' 
                    AND TABLE_NAME IN ('Users', 'Roles', 'Permissions')";

                using var command = new SqlCommand(query, connection);
                int tableCount = Convert.ToInt32(await command.ExecuteScalarAsync());

                return tableCount >= 3; // At least the core tables should exist
            }
            catch
            {
                return false;
            }
        }

        private async Task ExecuteSqlScriptAsync(string connectionString)
        {
            string scriptContent = await ReadSqlScriptAsync();

            // Split by GO statements
            string[] commandStrings = scriptContent.Split(
                new[] { "GO\r\n", "GO\n", "GO\r" },
                StringSplitOptions.RemoveEmptyEntries);

            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            int commandNumber = 0;
            foreach (string commandString in commandStrings)
            {
                if (string.IsNullOrWhiteSpace(commandString))
                    continue;

                commandNumber++;

                try
                {
                    using var command = new SqlCommand(commandString, connection);
                    command.CommandTimeout = 60; // 60 seconds timeout
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error executing SQL command #{commandNumber}");

                    // Log the problematic SQL for debugging
                    if (commandString.Length > 100)
                    {
                        _logger.LogError($"Failed SQL (first 100 chars): {commandString.Substring(0, 100)}...");
                    }
                    else
                    {
                        _logger.LogError($"Failed SQL: {commandString}");
                    }

                    throw;
                }
            }
        }

        private async Task<string> ReadSqlScriptAsync()
        {
            // Try multiple locations for the SQL script
            string[] possiblePaths = new[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SQLQueryLexiFlow.sql"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "SQLQueryLexiFlow.sql"),
                Path.Combine(Directory.GetCurrentDirectory(), "SQLQueryLexiFlow.sql"),
                Path.GetFullPath("SQLQueryLexiFlow.sql")
            };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    _logger.LogInformation($"Found SQL script at: {path}");
                    return await File.ReadAllTextAsync(path);
                }
            }

            // If not found as file, try embedded resource
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "LexiFlow.Infrastructure.Data.SQLQueryLexiFlow.sql";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                return await reader.ReadToEndAsync();
            }

            throw new FileNotFoundException(
                "SQL script not found. Please ensure 'SQLQueryLexiFlow.sql' is in the application directory or embedded as a resource.");
        }

        private async Task EnsureInitialDataAsync(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Check if we have the initial admin user
            string checkAdminQuery = "SELECT COUNT(*) FROM Users WHERE Username = 'admin'";
            using var checkCommand = new SqlCommand(checkAdminQuery, connection);
            int adminCount = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());

            if (adminCount == 0)
            {
                _logger.LogInformation("Creating initial admin user...");

                // Insert admin user with BCrypt hashed password (Admin@123)
                string insertAdminQuery = @"
                    INSERT INTO Users (Username, PasswordHash, FullName, Email, IsActive, CreatedAt)
                    VALUES ('admin', '$2a$11$rBdhmS0dk3m5yaJ5NvT6OeNqGjJF6aZCKQH5xrBz1ZFJ.uDp8yW2.', 
                            'System Administrator', 'admin@lexiflow.com', 1, GETDATE())";

                using var insertCommand = new SqlCommand(insertAdminQuery, connection);
                await insertCommand.ExecuteNonQueryAsync();

                // Assign admin role
                string assignRoleQuery = @"
                    INSERT INTO UserRoles (UserID, RoleID, AssignedAt)
                    SELECT u.UserID, r.RoleID, GETDATE()
                    FROM Users u, Roles r
                    WHERE u.Username = 'admin' AND r.RoleName = 'Admin'";

                using var roleCommand = new SqlCommand(assignRoleQuery, connection);
                await roleCommand.ExecuteNonQueryAsync();

                _logger.LogInformation("Admin user created successfully.");
            }

            // Insert additional demo users if needed
            await InsertDemoUsersAsync(connection);
        }

        private async Task InsertDemoUsersAsync(SqlConnection connection)
        {
            var demoUsers = new[]
            {
                new { Username = "teacher", Password = "$2a$11$uijeaU3q7TQbz1UiQD0vJeGVNYXNFGEgrzM4QmAFgO9NvnS2BhE7S",
                      FullName = "Demo Teacher", Email = "teacher@lexiflow.com", Role = "Teacher" },
                new { Username = "leader", Password = "$2a$11$5YJZqJ3K7TQbz1UiQD0vJeRTNYXNFGEgrzM4QmAFgO9NvnS2BhE7H",
                      FullName = "Team Leader", Email = "leader@lexiflow.com", Role = "Leader" },
                new { Username = "manager", Password = "$2a$11$8HGFqJ3K7TQbz1UiQD0vJeQWNYXNFGEgrzM4QmAFgO9NvnS2BhE7J",
                      FullName = "Department Manager", Email = "manager@lexiflow.com", Role = "Manager" },
                new { Username = "student", Password = "$2a$11$ICU0BfFyuWDK0SLYnIv.s.dMPh/NHgNuBdMwAj0fCB2ZFZ9X8Ylnu",
                      FullName = "Demo Student", Email = "student@lexiflow.com", Role = "Student" }
            };

            foreach (var user in demoUsers)
            {
                try
                {
                    // Check if user exists
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                    using var checkCmd = new SqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@username", user.Username);

                    int count = Convert.ToInt32(await checkCmd.ExecuteScalarAsync());
                    if (count > 0) continue;

                    // Insert user
                    string insertQuery = @"
                        INSERT INTO Users (Username, PasswordHash, FullName, Email, IsActive, CreatedAt)
                        VALUES (@username, @password, @fullname, @email, 1, GETDATE())";

                    using var insertCmd = new SqlCommand(insertQuery, connection);
                    insertCmd.Parameters.AddWithValue("@username", user.Username);
                    insertCmd.Parameters.AddWithValue("@password", user.Password);
                    insertCmd.Parameters.AddWithValue("@fullname", user.FullName);
                    insertCmd.Parameters.AddWithValue("@email", user.Email);

                    await insertCmd.ExecuteNonQueryAsync();

                    // Assign role
                    string roleQuery = @"
                        INSERT INTO UserRoles (UserID, RoleID, AssignedAt)
                        SELECT u.UserID, r.RoleID, GETDATE()
                        FROM Users u, Roles r
                        WHERE u.Username = @username AND r.RoleName = @role";

                    using var roleCmd = new SqlCommand(roleQuery, connection);
                    roleCmd.Parameters.AddWithValue("@username", user.Username);
                    roleCmd.Parameters.AddWithValue("@role", user.Role);

                    await roleCmd.ExecuteNonQueryAsync();

                    _logger.LogInformation($"Demo user '{user.Username}' created successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"Failed to create demo user '{user.Username}'");
                }
            }
        }
    }
}
