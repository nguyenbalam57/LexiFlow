using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LexiFlow.Infrastructure.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<LexiFlowContext>
    {
        public LexiFlowContext CreateDbContext(string[] args)
        {
            // Đọc cấu hình từ appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
                .Build();

            // Lấy chuỗi kết nối từ cấu hình
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Tạo các options cho DbContext
            var optionsBuilder = new DbContextOptionsBuilder<LexiFlowContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Tạo logger factory giả cho design-time
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                // Không sử dụng AddConsole() vì không cần console logging trong design-time
            });
            var logger = loggerFactory.CreateLogger<LexiFlowContext>();

            return new LexiFlowContext(optionsBuilder.Options, logger);
        }
    }
}
