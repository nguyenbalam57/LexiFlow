using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Infrastructure.Data
{
    public class SqlScriptInitializer
    {
        private readonly ILogger<SqlScriptInitializer> _logger;

        public SqlScriptInitializer(ILogger<SqlScriptInitializer> logger)
        {
            _logger = logger;
        }

        public void CopySqlScriptToOutputDirectory()
        {
            try
            {
                string sourceScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "SQLQueryLexiFlow.sql");
                string destScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SQLQueryLexiFlow.sql");

                // Nếu file đã tồn tại thì không cần copy
                if (File.Exists(destScriptPath))
                {
                    _logger.LogInformation("SQL script already exists in the output directory.");
                    return;
                }

                // Chuẩn hóa đường dẫn
                sourceScriptPath = Path.GetFullPath(sourceScriptPath);

                if (File.Exists(sourceScriptPath))
                {
                    File.Copy(sourceScriptPath, destScriptPath, true);
                    _logger.LogInformation($"SQL script copied to the output directory: {destScriptPath}");
                }
                else
                {
                    _logger.LogWarning($"SQL script not found at path: {sourceScriptPath}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error copying SQL script to output directory.");
            }
        }
    }
}
