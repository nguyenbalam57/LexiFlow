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
                builder.InitialCatalog = "master";
                string masterConnectionString = builder.ConnectionString;

                // Kiểm tra và tạo database nếu chưa tồn tại
                if (!await DatabaseExistsAsync(masterConnectionString, databaseName))
                {
                    await CreateDatabaseAsync(masterConnectionString, databaseName);
                }

                // Đọc và thực thi script SQL
                await ExecuteSqlScriptAsync(connectionString);

                _logger.LogInformation("Database initialized successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }

        private async Task<bool> DatabaseExistsAsync(string masterConnectionString, string databaseName)
        {
            using (var connection = new SqlConnection(masterConnectionString))
            {
                await connection.OpenAsync();

                string query = $"SELECT COUNT(*) FROM sys.databases WHERE name = '{databaseName}'";
                using (var command = new SqlCommand(query, connection))
                {
                    object? result = await command.ExecuteScalarAsync();
                    int count = Convert.ToInt32(result);
                    return count > 0;
                }
            }
        }

        private async Task CreateDatabaseAsync(string masterConnectionString, string databaseName)
        {
            using (var connection = new SqlConnection(masterConnectionString))
            {
                await connection.OpenAsync();

                string query = $"CREATE DATABASE [{databaseName}]";
                using (var command = new SqlCommand(query, connection))
                {
                    await command.ExecuteNonQueryAsync();
                    _logger.LogInformation($"Database '{databaseName}' created successfully.");
                }
            }
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
