using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using LexiFlow.API.DTOs.Analytics;
using LexiFlow.API.Services;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller phân tích và báo cáo h?c t?p v?i real-time data integration
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AnalyticsController : ControllerBase
    {
        private readonly ILogger<AnalyticsController> _logger;
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(ILogger<AnalyticsController> logger, IAnalyticsService analyticsService)
        {
            _logger = logger;
            _analyticsService = analyticsService;
        }

        /// <summary>
        /// L?y dashboard phân tích h?c t?p v?i real-time data
        /// </summary>
        [HttpGet("dashboard")]
        public async Task<ActionResult<LearningAnalyticsDashboardDto>> GetLearningDashboard(
            [FromQuery] int? days = 30,
            [FromQuery] bool useCache = true)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("Fetching analytics dashboard for user {UserId} with {Days} days", userId, days);

                if (!useCache)
                {
                    await _analyticsService.InvalidateCacheAsync(userId);
                }

                var dashboard = await _analyticsService.GetLearningDashboardAsync(userId, days ?? 30);
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting learning dashboard for user");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// T?o báo cáo chi ti?t v?i advanced analytics
        /// </summary>
        [HttpPost("generate-report")]
        public async Task<ActionResult<DetailedReportDto>> GenerateReport(
            [FromBody] GenerateReportRequestDto request)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("Generating detailed report for user {UserId}, type: {ReportType}", 
                    userId, request.ReportType);

                var report = await _analyticsService.GenerateDetailedReportAsync(userId, request);
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating report");
                return StatusCode(500, new { message = "Error generating report", error = ex.Message });
            }
        }

        /// <summary>
        /// L?y xu h??ng hi?u su?t chi ti?t
        /// </summary>
        [HttpGet("performance-trends")]
        public async Task<ActionResult<List<PerformanceTrendDto>>> GetPerformanceTrends(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int? days = 30)
        {
            try
            {
                var userId = GetCurrentUserId();
                var end = endDate ?? DateTime.UtcNow;
                var start = startDate ?? end.AddDays(-(days ?? 30));

                var trends = await _analyticsService.GetPerformanceTrendsAsync(userId, start, end);
                return Ok(trends);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting performance trends");
                return StatusCode(500, new { message = "Error getting performance trends", error = ex.Message });
            }
        }

        /// <summary>
        /// L?y patterns h?c t?p chi ti?t
        /// </summary>
        [HttpGet("study-patterns")]
        public async Task<ActionResult<List<DailyStudyDto>>> GetStudyPatterns(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int? days = 30)
        {
            try
            {
                var userId = GetCurrentUserId();
                var end = endDate ?? DateTime.UtcNow;
                var start = startDate ?? end.AddDays(-(days ?? 30));

                var patterns = await _analyticsService.GetStudyPatternsAsync(userId, start, end);
                return Ok(patterns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting study patterns");
                return StatusCode(500, new { message = "Error getting study patterns", error = ex.Message });
            }
        }

        /// <summary>
        /// L?y metrics nâng cao
        /// </summary>
        [HttpGet("advanced-metrics")]
        public async Task<ActionResult<Dictionary<string, object>>> GetAdvancedMetrics(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int? days = 30)
        {
            try
            {
                var userId = GetCurrentUserId();
                var end = endDate ?? DateTime.UtcNow;
                var start = startDate ?? end.AddDays(-(days ?? 30));

                var metrics = await _analyticsService.GetAdvancedMetricsAsync(userId, start, end);
                return Ok(metrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting advanced metrics");
                return StatusCode(500, new { message = "Error getting advanced metrics", error = ex.Message });
            }
        }

        /// <summary>
        /// L?y ?? xu?t cá nhân hóa
        /// </summary>
        [HttpGet("recommendations")]
        public async Task<ActionResult<List<string>>> GetPersonalizedRecommendations()
        {
            try
            {
                var userId = GetCurrentUserId();
                var recommendations = await _analyticsService.GeneratePersonalizedRecommendationsAsync(userId);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating recommendations");
                return StatusCode(500, new { message = "Error generating recommendations", error = ex.Message });
            }
        }

        /// <summary>
        /// Xu?t d? li?u v?i formats m? r?ng
        /// </summary>
        [HttpPost("export")]
        public async Task<IActionResult> ExportData([FromBody] ExportDataRequestDto request)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("Exporting data for user {UserId}, format: {Format}, type: {DataType}", 
                    userId, request.Format, request.DataType);
                
                var exportData = await GenerateExportDataAsync(userId, request);
                
                var fileName = $"lexiflow_export_{request.DataType}_{DateTime.Now:yyyyMMdd_HHmmss}.{request.Format.ToLower()}";
                var contentType = GetContentType(request.Format);
                
                return File(exportData, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting data");
                return StatusCode(500, new { message = "Error exporting data", error = ex.Message });
            }
        }

        /// <summary>
        /// Xóa cache analytics cho user
        /// </summary>
        [HttpPost("clear-cache")]
        public async Task<IActionResult> ClearCache()
        {
            try
            {
                var userId = GetCurrentUserId();
                await _analyticsService.InvalidateCacheAsync(userId);
                return Ok(new { message = "Cache cleared successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cache");
                return StatusCode(500, new { message = "Error clearing cache", error = ex.Message });
            }
        }

        /// <summary>
        /// Endpoint cho real-time analytics updates
        /// </summary>
        [HttpGet("real-time-stats")]
        public async Task<ActionResult<object>> GetRealTimeStats()
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // Get today's statistics
                var today = DateTime.UtcNow.Date;
                var tomorrow = today.AddDays(1);
                
                var todayMetrics = await _analyticsService.GetAdvancedMetricsAsync(userId, today, tomorrow);
                
                return Ok(new
                {
                    timestamp = DateTime.UtcNow,
                    userId = userId,
                    todayStats = todayMetrics,
                    status = "active"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting real-time stats");
                return StatusCode(500, new { message = "Error getting real-time stats", error = ex.Message });
            }
        }

        /// <summary>
        /// Endpoint ?? validate data integrity
        /// </summary>
        [HttpGet("validate-data")]
        public async Task<ActionResult<object>> ValidateData(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                var end = endDate ?? DateTime.UtcNow;
                var start = startDate ?? end.AddDays(-30);

                // TODO: Implement data validation logic
                var validation = new
                {
                    userId = userId,
                    period = new { start, end },
                    isValid = true,
                    issues = new string[0],
                    lastValidated = DateTime.UtcNow
                };

                return Ok(validation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating data");
                return StatusCode(500, new { message = "Error validating data", error = ex.Message });
            }
        }

        // Helper methods
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 1; // Default to 1 for testing
        }

        private async Task<byte[]> GenerateExportDataAsync(int userId, ExportDataRequestDto request)
        {
            var data = await _analyticsService.GetAdvancedMetricsAsync(userId, request.StartDate, request.EndDate);
            
            return request.Format.ToLower() switch
            {
                "json" => System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(data, new System.Text.Json.JsonSerializerOptions 
                { 
                    WriteIndented = true 
                }),
                "csv" => GenerateCsvData(data, request.DataType),
                "xlsx" => GenerateExcelData(data, request.DataType),
                _ => System.Text.Encoding.UTF8.GetBytes("Unsupported format")
            };
        }

        private byte[] GenerateCsvData(Dictionary<string, object> data, string dataType)
        {
            var csv = new System.Text.StringBuilder();
            csv.AppendLine("Metric,Value,Type");
            
            foreach (var kvp in data)
            {
                csv.AppendLine($"{kvp.Key},{kvp.Value},{dataType}");
            }
            
            return System.Text.Encoding.UTF8.GetBytes(csv.ToString());
        }

        private byte[] GenerateExcelData(Dictionary<string, object> data, string dataType)
        {
            // TODO: Implement Excel generation using a library like EPPlus
            // For now, return CSV format
            return GenerateCsvData(data, dataType);
        }

        private string GetContentType(string format)
        {
            return format.ToLower() switch
            {
                "csv" => "text/csv",
                "json" => "application/json",
                "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "pdf" => "application/pdf",
                _ => "application/octet-stream"
            };
        }
    }

    /// <summary>
    /// DTO cho yêu c?u t?o báo cáo v?i enhanced options
    /// </summary>
    public class GenerateReportRequestDto
    {
        public string ReportType { get; set; } = "overview";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> IncludeSections { get; set; } = new();
        public bool IncludeCharts { get; set; } = true;
        public bool IncludeRecommendations { get; set; } = true;
        public string Format { get; set; } = "json";
        public Dictionary<string, object> CustomFilters { get; set; } = new();
    }

    /// <summary>
    /// DTO cho yêu c?u xu?t d? li?u v?i enhanced options
    /// </summary>
    public class ExportDataRequestDto
    {
        public string DataType { get; set; } = "studyProgress";
        public string Format { get; set; } = "csv";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> IncludeFields { get; set; } = new();
        public bool IncludeMetadata { get; set; } = true;
        public string Compression { get; set; } = "none"; // none, zip, gzip
        public int MaxRecords { get; set; } = 10000;
    }
}