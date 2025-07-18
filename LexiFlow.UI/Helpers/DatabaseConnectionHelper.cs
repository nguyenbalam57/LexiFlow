using Microsoft.Data.SqlClient;

namespace LexiFlow.UI.Helpers
{
    public static class DatabaseConnectionHelper
    {
        public static string GetConnectionString(string serverName = ".\\SQLEXPRESS",
                                               string databaseName = "LexiFlow",
                                               bool useWindowsAuth = true,
                                               string? username = null,
                                               string? password = null)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = serverName,
                InitialCatalog = databaseName,
                TrustServerCertificate = true,
                MultipleActiveResultSets = true,
                ConnectTimeout = 30
            };

            if (useWindowsAuth)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.IntegratedSecurity = false;
                builder.UserID = username ?? "sa";
                builder.Password = password ?? "";
            }

            return builder.ConnectionString;
        }

        public static async Task<(bool Success, string Message)> TestConnectionAsync(string connectionString)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                // Test with a simple query
                using var command = new SqlCommand("SELECT 1", connection);
                await command.ExecuteScalarAsync();

                return (true, "Kết nối thành công!");
            }
            catch (SqlException sqlEx)
            {
                return (false, $"Lỗi SQL: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        public static async Task<List<string>> GetAvailableSqlServersAsync()
        {
            var servers = new List<string>();

            try
            {
                // Add common SQL Server instances
                servers.Add(".\\SQLEXPRESS");
                servers.Add("(local)");
                servers.Add(".");
                servers.Add(Environment.MachineName);
                servers.Add($"{Environment.MachineName}\\SQLEXPRESS");

                // Try to discover SQL Server instances on the network
                var instances = await DiscoverSqlServersAsync();
                servers.AddRange(instances);

                // Remove duplicates
                servers = servers.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error discovering SQL servers: {ex.Message}");
            }

            return servers;
        }

        private static Task<List<string>> DiscoverSqlServersAsync()
        {
            return Task.Run(() =>
            {
                var servers = new List<string>();

                try
                {
                    // This uses SQL Server Browser service
                    var factory = SqlClientFactory.Instance;
                    if (factory.CanCreateDataSourceEnumerator)
                    {
                        var enumerator = factory.CreateDataSourceEnumerator();
                        var dataTable = enumerator?.GetDataSources();

                        if (dataTable != null)
                        {
                            foreach (System.Data.DataRow row in dataTable.Rows)
                            {
                                var serverName = row["ServerName"]?.ToString();
                                var instanceName = row["InstanceName"]?.ToString();

                                if (!string.IsNullOrEmpty(serverName))
                                {
                                    var fullName = string.IsNullOrEmpty(instanceName)
                                        ? serverName
                                        : $"{serverName}\\{instanceName}";

                                    servers.Add(fullName);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error enumerating SQL servers: {ex.Message}");
                }

                return servers;
            });
        }

        public static bool IsSqlServerServiceRunning()
        {
            try
            {
                // Check common SQL Server service names
                string[] serviceNames = {
                    "MSSQL$SQLEXPRESS",
                    "MSSQLSERVER",
                    "SQL Server (SQLEXPRESS)",
                    "SQL Server (MSSQLSERVER)"
                };

                foreach (var serviceName in serviceNames)
                {
                    var service = System.ServiceProcess.ServiceController.GetServices()
                        .FirstOrDefault(s => s.ServiceName.Equals(serviceName, StringComparison.OrdinalIgnoreCase));

                    if (service != null && service.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public static string GetDatabaseConfigurationGuide()
        {
            return @"HƯỚNG DẪN CẤU HÌNH CƠ SỞ DỮ LIỆU:

1. Kiểm tra SQL Server Express đã được cài đặt:
   - Mở Services.msc
   - Tìm 'SQL Server (SQLEXPRESS)'
   - Đảm bảo service đang chạy (Running)

2. Nếu chưa cài đặt SQL Server Express:
   - Tải từ: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - Chọn 'Express' edition (miễn phí)
   - Cài đặt với cấu hình mặc định

3. Cấu hình SQL Server:
   - Bật SQL Server Browser service
   - Cho phép TCP/IP connections
   - Kiểm tra Windows Firewall

4. Cập nhật file appsettings.json:
   - Server: .\SQLEXPRESS hoặc (local)\SQLEXPRESS
   - Database: LexiFlow
   - Trusted_Connection: True (cho Windows Authentication)

5. Nếu dùng SQL Authentication:
   - Bật Mixed Mode Authentication trong SQL Server
   - Tạo user login với quyền dbcreator";
        }
    }
}
