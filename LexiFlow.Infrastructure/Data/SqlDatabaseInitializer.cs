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

        public async Task InitializeDatabaseAsync()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

                // Tạo chuỗi kết nối tới master database để có thể tạo database mới
                var builder = new SqlConnectionStringBuilder(connectionString);
                string databaseName = builder.InitialCatalog;

                _logger.LogInformation($"Checking database '{databaseName}'...");
                // Chỉ kiểm tra xem database có tồn tại không
                if (await DatabaseExistsAsync(connectionString))
                {
                    _logger.LogInformation($"Database '{databaseName}' already exists. Skipping creation.");

                    // Kiểm tra xem có tables không
                    if (!await TablesExistAsync(connectionString))
                    {
                        _logger.LogInformation("Tables not found. Creating tables...");
                        await ExecuteSqlScriptAsync(connectionString);
                    }
                    else
                    {
                        _logger.LogInformation("Tables already exist. Database is ready.");
                    }
                }
                else
                {
                    _logger.LogWarning($"Database '{databaseName}' not found!");
                    throw new Exception($"Database '{databaseName}' does not exist. Please create it first in SQL Server Management Studio.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }

        private async Task<bool> DatabaseExistsAsync(string connectionString)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        private async Task<bool> TablesExistAsync(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Kiểm tra xem bảng Users có tồn tại không
            string query = @"
                SELECT COUNT(*) 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_TYPE = 'BASE TABLE' 
                AND TABLE_NAME = 'Users'";

            using var command = new SqlCommand(query, connection);
            var count = (int)await command.ExecuteScalarAsync();
            return count > 0;
        }

        private async Task ExecuteSqlScriptAsync(string connectionString)
        {
            // Đọc nội dung script SQL từ file
            string scriptContent = await ReadSqlScriptFromResourceAsync();

            // Chia script thành các lệnh riêng biệt theo dấu hiệu "GO"
            string[] commandStrings = scriptContent.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                foreach (string commandString in commandStrings)
                {
                    if (string.IsNullOrWhiteSpace(commandString))
                        continue;

                    using (var command = new SqlCommand(commandString, connection))
                    {
                        try
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error executing SQL command: {commandString.Substring(0, Math.Min(100, commandString.Length))}...");
                            throw;
                        }
                    }
                }
            }
        }

        private async Task<string> ReadSqlScriptFromResourceAsync()
        {
            // Đọc từ file bên ngoài
            string sqlScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SQLQueryLexiFlow.sql");

            if (File.Exists(sqlScriptPath))
            {
                return await File.ReadAllTextAsync(sqlScriptPath);
            }

            // Nếu không tìm thấy file, cố gắng đọc từ embedded resource
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "LexiFlow.Infrastructure.Data.SQLQueryLexiFlow.sql";

            using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"SQL script not found. Please ensure the file 'SQLQueryLexiFlow.sql' is in the application directory or embedded as a resource.");
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}
