using LexiFlow.AdminDashboard.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LexiFlow.AdminDashboard.Services
{
    public class AdminConfigService
    {
        private readonly ILogger<AdminConfigService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _configFilePath;

        public AdminConfigService(ILogger<AdminConfigService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _configFilePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "admin_config.json");
        }

        public async Task<SystemSettings> LoadConfigAsync()
        {
            try
            {
                if (File.Exists(_configFilePath))
                {
                    string json = await File.ReadAllTextAsync(_configFilePath);
                    var settings = JsonSerializer.Deserialize<SystemSettings>(json);
                    return settings ?? new SystemSettings();
                }

                return new SystemSettings();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading admin configuration");
                return new SystemSettings();
            }
        }

        public async Task SaveConfigAsync(SystemSettings settings)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(settings, options);
                await File.WriteAllTextAsync(_configFilePath, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving admin configuration");
                throw;
            }
        }

        public async Task<bool> UpdateDatabaseConnectionAsync(string connectionString)
        {
            try
            {
                // Test connection first
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                }

                // Update configuration in app settings
                var settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                if (File.Exists(settingsPath))
                {
                    string json = await File.ReadAllTextAsync(settingsPath);
                    var config = JsonSerializer.Deserialize<JsonDocument>(json);

                    // Create a new settings object
                    using var stream = new MemoryStream();
                    using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
                    writer.WriteStartObject();

                    // Copy all properties except ConnectionStrings
                    using var document = JsonDocument.Parse(json);
                    foreach (var property in document.RootElement.EnumerateObject())
                    {
                        if (property.Name == "ConnectionStrings")
                        {
                            // Write updated ConnectionStrings
                            writer.WritePropertyName("ConnectionStrings");
                            writer.WriteStartObject();
                            writer.WriteString("DefaultConnection", connectionString);
                            writer.WriteEndObject();
                        }
                        else
                        {
                            // Copy other properties as is
                            property.WriteTo(writer);
                        }
                    }

                    writer.WriteEndObject();
                    writer.Flush();

                    string updatedJson = Encoding.UTF8.GetString(stream.ToArray());
                    await File.WriteAllTextAsync(settingsPath, updatedJson);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating database connection");
                return false;
            }
        }
    }
}
