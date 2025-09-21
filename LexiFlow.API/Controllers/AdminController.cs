using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý h? th?ng dành cho Admin
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// L?y dashboard t?ng quan cho admin
        /// </summary>
        [HttpGet("dashboard")]
        public async Task<ActionResult<AdminDashboardDto>> GetDashboard()
        {
            try
            {
                var dashboard = new AdminDashboardDto
                {
                    SystemStats = new SystemStatsDto
                    {
                        TotalUsers = 1251,
                        ActiveUsers = 890,
                        NewUsersToday = 15,
                        TotalContent = 5500,
                        TotalVocabulary = 3200,
                        TotalKanji = 1500,
                        TotalGrammar = 800,
                        SystemUptime = "15 days, 6 hours",
                        DatabaseSize = "2.5 GB",
                        LastBackup = DateTime.UtcNow.AddHours(-6)
                    },
                    UserActivity = new UserActivityDto
                    {
                        DailyActiveUsers = 320,
                        WeeklyActiveUsers = 650,
                        MonthlyActiveUsers = 890,
                        AverageSessionTime = 45,
                        TotalSessionsToday = 180,
                        TopUserActivities = new List<string> { "Vocabulary Study", "Kanji Practice", "Grammar Review" }
                    },
                    ContentStats = new ContentStatsDto
                    {
                        PendingApprovals = 25,
                        RecentSubmissions = 40,
                        ContentByLevel = new Dictionary<string, int>
                        {
                            { "N5", 1200 },
                            { "N4", 1100 },
                            { "N3", 1000 },
                            { "N2", 800 },
                            { "N1", 400 }
                        },
                        TopCategories = new Dictionary<string, int>
                        {
                            { "Daily Conversation", 450 },
                            { "Business", 320 },
                            { "Travel", 280 },
                            { "Academic", 250 }
                        }
                    },
                    SystemHealth = new SystemHealthDto
                    {
                        CpuUsage = 45.2f,
                        MemoryUsage = 68.7f,
                        DiskUsage = 72.3f,
                        DatabaseResponseTime = 120,
                        ApiResponseTime = 95,
                        ErrorRate = 0.02f,
                        ActiveConnections = 145,
                        Status = "Healthy"
                    },
                    RecentAlerts = new List<AlertDto>
                    {
                        new AlertDto
                        {
                            AlertId = 1,
                            Type = "Warning",
                            Message = "High memory usage detected",
                            Timestamp = DateTime.UtcNow.AddMinutes(-15),
                            IsResolved = false
                        },
                        new AlertDto
                        {
                            AlertId = 2,
                            Type = "Info",
                            Message = "Database backup completed successfully",
                            Timestamp = DateTime.UtcNow.AddHours(-6),
                            IsResolved = true
                        }
                    }
                };

                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting admin dashboard");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Quản lý người dung
        /// </summary>
        [HttpGet("users")]
        public async Task<ActionResult<PaginatedResultDto<AdminUserDto>>> GetUsers(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? search = null,
            [FromQuery] string? role = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] DateTime? registeredAfter = null)
        {
            try
            {
                var users = new List<AdminUserDto>
                {
                    new AdminUserDto
                    {
                        UserId = 1,
                        Username = "user001",
                        Email = "user001@example.com",
                        DisplayName = "John Doe",
                        Roles = new List<string> { "User" },
                        IsActive = true,
                        RegistrationDate = DateTime.UtcNow.AddDays(-90),
                        LastLoginAt = DateTime.UtcNow.AddHours(-2),
                        StudyDays = 45,
                        TotalStudyTime = 2700,
                        VocabLearned = 320,
                        CurrentLevel = "N4"
                    },
                    new AdminUserDto
                    {
                        UserId = 2,
                        Username = "premium_user",
                        Email = "premium@example.com",
                        DisplayName = "Jane Smith",
                        Roles = new List<string> { "User", "Premium" },
                        IsActive = true,
                        RegistrationDate = DateTime.UtcNow.AddDays(-180),
                        LastLoginAt = DateTime.UtcNow.AddMinutes(-30),
                        StudyDays = 120,
                        TotalStudyTime = 7200,
                        VocabLearned = 850,
                        CurrentLevel = "N2"
                    }
                };

                // Apply filters
                if (!string.IsNullOrEmpty(search))
                {
                    users = users.Where(u => 
                        u.Username.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        u.Email.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        u.DisplayName.Contains(search, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (!string.IsNullOrEmpty(role))
                {
                    users = users.Where(u => u.Roles.Contains(role, StringComparer.OrdinalIgnoreCase))
                                 .ToList();
                }

                if (isActive.HasValue)
                {
                    users = users.Where(u => u.IsActive == isActive.Value).ToList();
                }

                if (registeredAfter.HasValue)
                {
                    users = users.Where(u => u.RegistrationDate >= registeredAfter.Value).ToList();
                }

                var totalCount = users.Count;
                var items = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<AdminUserDto>
                {
                    Data = items,
                    TotalCount = totalCount,
                    PageNumber = page,
                    PageSize = pageSize
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users for admin");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Cập nhật trạng thái người dùng
        /// </summary>
        [HttpPut("users/{userId}/status")]
        public async Task<ActionResult> UpdateUserStatus(int userId, [FromBody] UpdateUserStatusDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // TODO: Implement actual user status update
                
                var result = new
                {
                    Success = true,
                    Message = $"User status updated to {updateDto.Status}",
                    UserId = userId,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = GetCurrentUserId()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user status for user {UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Quản lý vai trò người dùng
        /// </summary>
        [HttpPost("users/{userId}/roles")]
        public async Task<ActionResult> AssignRole(int userId, [FromBody] AssignRoleDto assignDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // TODO: Implement actual role assignment
                
                var result = new
                {
                    Success = true,
                    Message = $"Role '{assignDto.RoleName}' assigned to user {userId}",
                    UserId = userId,
                    RoleName = assignDto.RoleName,
                    AssignedAt = DateTime.UtcNow,
                    AssignedBy = GetCurrentUserId()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role to user {UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Quản lý nội dung chờ duyệt
        /// </summary>
        [HttpGet("content/pending")]
        public async Task<ActionResult<PaginatedResultDto<PendingContentDto>>> GetPendingContent(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? contentType = null,
            [FromQuery] string? level = null)
        {
            try
            {
                var pendingContent = new List<PendingContentDto>
                {
                    new PendingContentDto
                    {
                        ContentId = 1,
                        ContentType = "Vocabulary",
                        Title = "?????",
                        SubmittedBy = "user001",
                        SubmittedAt = DateTime.UtcNow.AddHours(-2),
                        Level = "N5",
                        Status = "Pending",
                        Priority = "Normal",
                        ReviewComments = "",
                        PreviewData = "{ \"word\": \"?????\", \"meaning\": \"Good evening\" }"
                    },
                    new PendingContentDto
                    {
                        ContentId = 2,
                        ContentType = "Grammar",
                        Title = "???? pattern",
                        SubmittedBy = "premium_user",
                        SubmittedAt = DateTime.UtcNow.AddHours(-4),
                        Level = "N3",
                        Status = "In Review",
                        Priority = "High",
                        ReviewComments = "Needs more examples",
                        PreviewData = "{ \"pattern\": \"????\", \"meaning\": \"while doing\" }"
                    }
                };

                // Apply filters
                if (!string.IsNullOrEmpty(contentType))
                {
                    pendingContent = pendingContent.Where(pc => pc.ContentType.Equals(contentType, StringComparison.OrdinalIgnoreCase))
                                                  .ToList();
                }

                if (!string.IsNullOrEmpty(level))
                {
                    pendingContent = pendingContent.Where(pc => pc.Level.Equals(level, StringComparison.OrdinalIgnoreCase))
                                                  .ToList();
                }

                var totalCount = pendingContent.Count;
                var items = pendingContent.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<PendingContentDto>
                {
                    Data = items,
                    TotalCount = totalCount,
                    PageNumber = page,
                    PageSize = pageSize
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending content");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Duyệt hoặc từ chối nội dung
        /// </summary>
        [HttpPost("content/{contentId}/review")]
        public async Task<ActionResult> ReviewContent(int contentId, [FromBody] ContentReviewDto reviewDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // TODO: Implement actual content review
                
                var result = new
                {
                    Success = true,
                    Message = $"Content {reviewDto.Action.ToLower()}ed successfully",
                    ContentId = contentId,
                    Action = reviewDto.Action,
                    Comments = reviewDto.Comments,
                    ReviewedAt = DateTime.UtcNow,
                    ReviewedBy = GetCurrentUserId()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reviewing content {ContentId}", contentId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Báo cáo hệ thống
        /// </summary>
        [HttpGet("reports/system")]
        public async Task<ActionResult<SystemReportDto>> GetSystemReport(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var start = startDate ?? DateTime.UtcNow.AddDays(-30);
                var end = endDate ?? DateTime.UtcNow;

                var report = new SystemReportDto
                {
                    Period = new PeriodDto { StartDate = start, EndDate = end },
                    UserMetrics = new UserMetricsDto
                    {
                        NewRegistrations = 85,
                        ActiveUsers = 650,
                        RetentionRate = 78.5f,
                        ChurnRate = 21.5f,
                        AverageSessionTime = 42
                    },
                    ContentMetrics = new ContentMetricsDto
                    {
                        ContentAdded = 120,
                        ContentApproved = 95,
                        ContentRejected = 18,
                        UserSubmissions = 75,
                        AdminSubmissions = 45
                    },
                    PerformanceMetrics = new PerformanceMetricsDto
                    {
                        AverageResponseTime = 185,
                        ErrorRate = 0.03f,
                        Uptime = 99.7f,
                        PeakConcurrentUsers = 245,
                        DatabaseSize = 2.8f
                    },
                    TopIssues = new List<IssueDto>
                    {
                        new IssueDto
                        {
                            IssueType = "Performance",
                            Description = "Slow query on vocabulary search",
                            Frequency = 15,
                            Severity = "Medium",
                            Status = "In Progress"
                        }
                    }
                };

                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating system report");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Quản lý cài đặt hệ thống
        /// </summary>
        [HttpGet("settings")]
        public async Task<ActionResult<List<SystemSettingDto>>> GetSystemSettings([FromQuery] string? category = null)
        {
            try
            {
                var settings = new List<SystemSettingDto>
                {
                    new SystemSettingDto
                    {
                        SettingId = 1,
                        Key = "MaxFileUploadSize",
                        Value = "10MB",
                        Category = "Upload",
                        Description = "Maximum file size for media uploads",
                        DataType = "String",
                        IsEditable = true,
                        IsVisible = true
                    },
                    new SystemSettingDto
                    {
                        SettingId = 2,
                        Key = "DefaultStudySessionTime",
                        Value = "30",
                        Category = "Learning",
                        Description = "Default study session time in minutes",
                        DataType = "Integer",
                        IsEditable = true,
                        IsVisible = true
                    },
                    new SystemSettingDto
                    {
                        SettingId = 3,
                        Key = "MaintenanceMode",
                        Value = "false",
                        Category = "System",
                        Description = "Enable maintenance mode",
                        DataType = "Boolean",
                        IsEditable = true,
                        IsVisible = true
                    }
                };

                if (!string.IsNullOrEmpty(category))
                {
                    settings = settings.Where(s => s.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                                      .ToList();
                }

                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting system settings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Cập nhật cài đặt hệ thống
        /// </summary>
        [HttpPut("settings/{settingId}")]
        public async Task<ActionResult> UpdateSystemSetting(int settingId, [FromBody] UpdateSettingDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // TODO: Implement actual setting update with validation
                
                var result = new
                {
                    Success = true,
                    Message = "System setting updated successfully",
                    SettingId = settingId,
                    NewValue = updateDto.Value,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = GetCurrentUserId()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating system setting {SettingId}", settingId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Logs hệ thống
        /// </summary>
        [HttpGet("logs")]
        public async Task<ActionResult<PaginatedResultDto<SystemLogDto>>> GetSystemLogs(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? level = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? source = null)
        {
            try
            {
                var logs = new List<SystemLogDto>
                {
                    new SystemLogDto
                    {
                        LogId = 1,
                        Timestamp = DateTime.UtcNow.AddMinutes(-5),
                        Level = "Warning",
                        Source = "Database",
                        Message = "High memory usage detected",
                        Details = "Memory usage: 85%. Consider optimizing queries.",
                        UserId = null,
                        RequestId = "req_123456"
                    },
                    new SystemLogDto
                    {
                        LogId = 2,
                        Timestamp = DateTime.UtcNow.AddMinutes(-10),
                        Level = "Info",
                        Source = "Authentication",
                        Message = "User login successful",
                        Details = "User: user001, IP: 192.168.1.100",
                        UserId = 1,
                        RequestId = "req_123455"
                    },
                    new SystemLogDto
                    {
                        LogId = 3,
                        Timestamp = DateTime.UtcNow.AddMinutes(-15),
                        Level = "Error",
                        Source = "API",
                        Message = "Failed to process request",
                        Details = "Timeout occurred while accessing external service",
                        UserId = null,
                        RequestId = "req_123454"
                    }
                };

                // Apply filters
                if (!string.IsNullOrEmpty(level))
                {
                    logs = logs.Where(l => l.Level.Equals(level, StringComparison.OrdinalIgnoreCase))
                              .ToList();
                }

                if (startDate.HasValue)
                {
                    logs = logs.Where(l => l.Timestamp >= startDate.Value).ToList();
                }

                if (endDate.HasValue)
                {
                    logs = logs.Where(l => l.Timestamp <= endDate.Value).ToList();
                }

                if (!string.IsNullOrEmpty(source))
                {
                    logs = logs.Where(l => l.Source.Contains(source, StringComparison.OrdinalIgnoreCase))
                              .ToList();
                }

                var totalCount = logs.Count;
                var items = logs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<SystemLogDto>
                {
                    Data = items,
                    TotalCount = totalCount,
                    PageNumber = page,
                    PageSize = pageSize
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting system logs");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Backup dữ liệu
        /// </summary>
        [HttpPost("backup")]
        public async Task<ActionResult> CreateBackup([FromBody] BackupRequestDto backupDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // TODO: Implement actual backup creation
                
                var result = new
                {
                    Success = true,
                    Message = "Backup created successfully",
                    BackupId = Guid.NewGuid().ToString(),
                    BackupType = backupDto.BackupType,
                    IncludeMedia = backupDto.IncludeMedia,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = GetCurrentUserId(),
                    EstimatedSize = "1.2 GB",
                    EstimatedTime = "15 minutes"
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating backup");
                return StatusCode(500, "Internal server error");
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return 1;
        }
    }

    #region DTOs

    public class AdminDashboardDto
    {
        public SystemStatsDto SystemStats { get; set; } = new SystemStatsDto();
        public UserActivityDto UserActivity { get; set; } = new UserActivityDto();
        public ContentStatsDto ContentStats { get; set; } = new ContentStatsDto();
        public SystemHealthDto SystemHealth { get; set; } = new SystemHealthDto();
        public List<AlertDto> RecentAlerts { get; set; } = new List<AlertDto>();
    }

    public class SystemStatsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int NewUsersToday { get; set; }
        public int TotalContent { get; set; }
        public int TotalVocabulary { get; set; }
        public int TotalKanji { get; set; }
        public int TotalGrammar { get; set; }
        public string SystemUptime { get; set; } = string.Empty;
        public string DatabaseSize { get; set; } = string.Empty;
        public DateTime LastBackup { get; set; }
    }

    public class UserActivityDto
    {
        public int DailyActiveUsers { get; set; }
        public int WeeklyActiveUsers { get; set; }
        public int MonthlyActiveUsers { get; set; }
        public int AverageSessionTime { get; set; }
        public int TotalSessionsToday { get; set; }
        public List<string> TopUserActivities { get; set; } = new List<string>();
    }

    public class ContentStatsDto
    {
        public int PendingApprovals { get; set; }
        public int RecentSubmissions { get; set; }
        public Dictionary<string, int> ContentByLevel { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> TopCategories { get; set; } = new Dictionary<string, int>();
    }

    public class SystemHealthDto
    {
        public float CpuUsage { get; set; }
        public float MemoryUsage { get; set; }
        public float DiskUsage { get; set; }
        public int DatabaseResponseTime { get; set; }
        public int ApiResponseTime { get; set; }
        public float ErrorRate { get; set; }
        public int ActiveConnections { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class AlertDto
    {
        public int AlertId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public bool IsResolved { get; set; }
    }

    public class AdminUserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public bool IsActive { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int StudyDays { get; set; }
        public int TotalStudyTime { get; set; }
        public int VocabLearned { get; set; }
        public string CurrentLevel { get; set; } = string.Empty;
    }

    public class UpdateUserStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }

    public class AssignRoleDto
    {
        [Required]
        public string RoleName { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class PendingContentDto
    {
        public int ContentId { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string SubmittedBy { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string ReviewComments { get; set; } = string.Empty;
        public string PreviewData { get; set; } = string.Empty;
    }

    public class ContentReviewDto
    {
        [Required]
        public string Action { get; set; } = string.Empty; // Approve, Reject, RequestChanges
        public string Comments { get; set; } = string.Empty;
        public int? ReviewScore { get; set; }
    }

    public class SystemReportDto
    {
        public PeriodDto Period { get; set; } = new PeriodDto();
        public UserMetricsDto UserMetrics { get; set; } = new UserMetricsDto();
        public ContentMetricsDto ContentMetrics { get; set; } = new ContentMetricsDto();
        public PerformanceMetricsDto PerformanceMetrics { get; set; } = new PerformanceMetricsDto();
        public List<IssueDto> TopIssues { get; set; } = new List<IssueDto>();
    }

    public class PeriodDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class UserMetricsDto
    {
        public int NewRegistrations { get; set; }
        public int ActiveUsers { get; set; }
        public float RetentionRate { get; set; }
        public float ChurnRate { get; set; }
        public int AverageSessionTime { get; set; }
    }

    public class ContentMetricsDto
    {
        public int ContentAdded { get; set; }
        public int ContentApproved { get; set; }
        public int ContentRejected { get; set; }
        public int UserSubmissions { get; set; }
        public int AdminSubmissions { get; set; }
    }

    public class PerformanceMetricsDto
    {
        public int AverageResponseTime { get; set; }
        public float ErrorRate { get; set; }
        public float Uptime { get; set; }
        public int PeakConcurrentUsers { get; set; }
        public float DatabaseSize { get; set; }
    }

    public class IssueDto
    {
        public string IssueType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Frequency { get; set; }
        public string Severity { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class SystemSettingDto
    {
        public int SettingId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public bool IsEditable { get; set; }
        public bool IsVisible { get; set; }
    }

    public class UpdateSettingDto
    {
        [Required]
        public string Value { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }

    public class SystemLogDto
    {
        public int LogId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public string RequestId { get; set; } = string.Empty;
    }

    public class BackupRequestDto
    {
        [Required]
        public string BackupType { get; set; } = string.Empty; // Full, Incremental, Differential
        public bool IncludeMedia { get; set; } = true;
        public string Description { get; set; } = string.Empty;
    }

    #endregion
}