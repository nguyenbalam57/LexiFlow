using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý thông báo
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ILogger<NotificationController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// L?y danh sách thông báo c?a user
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<NotificationDto>>> GetNotifications(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? type = null,
            [FromQuery] bool? isRead = null,
            [FromQuery] string? priority = null)
        {
            try
            {
                var userId = GetCurrentUserId();

                var notifications = new List<NotificationDto>
                {
                    new NotificationDto
                    {
                        NotificationId = 1,
                        Title = "Study Reminder",
                        Message = "Don't forget your daily Japanese study session!",
                        Type = "Study",
                        Priority = "Medium",
                        IsRead = false,
                        IsDelivered = true,
                        CreatedAt = DateTime.UtcNow.AddHours(-2),
                        ActionUrl = "/study/dashboard",
                        Icon = "??",
                        Category = "Learning"
                    },
                    new NotificationDto
                    {
                        NotificationId = 2,
                        Title = "Achievement Unlocked!",
                        Message = "Congratulations! You've earned the 'Vocabulary Master' badge.",
                        Type = "Achievement",
                        Priority = "High",
                        IsRead = false,
                        IsDelivered = true,
                        CreatedAt = DateTime.UtcNow.AddHours(-1),
                        ActionUrl = "/achievements",
                        Icon = "??",
                        Category = "Gamification"
                    },
                    new NotificationDto
                    {
                        NotificationId = 3,
                        Title = "Test Results Available",
                        Message = "Your JLPT N4 practice test results are ready for review.",
                        Type = "Test",
                        Priority = "Medium",
                        IsRead = true,
                        IsDelivered = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-1),
                        ReadAt = DateTime.UtcNow.AddHours(-12),
                        ActionUrl = "/tests/results/123",
                        Icon = "??",
                        Category = "Assessment"
                    }
                };

                // Apply filters
                if (!string.IsNullOrEmpty(type))
                {
                    notifications = notifications.Where(n => n.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                                                .ToList();
                }

                if (isRead.HasValue)
                {
                    notifications = notifications.Where(n => n.IsRead == isRead.Value).ToList();
                }

                if (!string.IsNullOrEmpty(priority))
                {
                    notifications = notifications.Where(n => n.Priority.Equals(priority, StringComparison.OrdinalIgnoreCase))
                                                .ToList();
                }

                var totalCount = notifications.Count;
                var items = notifications.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<NotificationDto>
                {
                    Data = items,
                    TotalCount = totalCount,
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notifications");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// ?ánh d?u thông báo ?ã ??c
        /// </summary>
        [HttpPut("{notificationId}/read")]
        public async Task<ActionResult> MarkAsRead(int notificationId)
        {
            try
            {
                var userId = GetCurrentUserId();

                var result = new
                {
                    Success = true,
                    Message = "Notification marked as read",
                    NotificationId = notificationId,
                    ReadAt = DateTime.UtcNow
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification {NotificationId} as read", notificationId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// ?ánh d?u t?t c? thông báo ?ã ??c
        /// </summary>
        [HttpPut("mark-all-read")]
        public async Task<ActionResult> MarkAllAsRead()
        {
            try
            {
                var userId = GetCurrentUserId();

                var result = new
                {
                    Success = true,
                    Message = "All notifications marked as read",
                    MarkedCount = 15,
                    MarkedAt = DateTime.UtcNow
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking all notifications as read");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Xóa thông báo
        /// </summary>
        [HttpDelete("{notificationId}")]
        public async Task<ActionResult> DeleteNotification(int notificationId)
        {
            try
            {
                var userId = GetCurrentUserId();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting notification {NotificationId}", notificationId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y cài ??t thông báo
        /// </summary>
        [HttpGet("settings")]
        public async Task<ActionResult<object>> GetNotificationSettings()
        {
            try
            {
                var userId = GetCurrentUserId();

                // S? d?ng anonymous object thay vì NotificationSettingsDto ?? tránh xung ??t
                var settings = new
                {
                    EmailNotifications = true,
                    PushNotifications = true,
                    StudyReminders = true,
                    AchievementNotifications = true,
                    TestResultNotifications = true,
                    ChallengeNotifications = false,
                    SystemNotifications = true,
                    QuietHours = new
                    {
                        Enabled = true,
                        StartTime = "22:00",
                        EndTime = "08:00",
                        Timezone = "Asia/Ho_Chi_Minh"
                    },
                    StudyReminderSettings = new
                    {
                        Enabled = true,
                        DailyReminderTime = "19:00",
                        WeeklyGoalReminder = true,
                        StreakWarning = true
                    }
                };

                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notification settings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t cài ??t thông báo
        /// </summary>
        [HttpPut("settings")]
        public async Task<ActionResult> UpdateNotificationSettings([FromBody] UpdateNotificationSettingsDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                var result = new
                {
                    Success = true,
                    Message = "Notification settings updated successfully",
                    UpdatedAt = DateTime.UtcNow
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating notification settings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// G?i thông báo test
        /// </summary>
        [HttpPost("test")]
        public async Task<ActionResult> SendTestNotification([FromBody] SendTestNotificationDto testDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                var result = new
                {
                    Success = true,
                    Message = "Test notification sent successfully",
                    NotificationId = new Random().Next(1000, 9999),
                    SentAt = DateTime.UtcNow,
                    Channel = testDto.Channel
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending test notification");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y th?ng kê thông báo
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetNotificationStatistics()
        {
            try
            {
                var userId = GetCurrentUserId();

                var statistics = new
                {
                    TotalNotifications = 45,
                    UnreadNotifications = 5,
                    ReadNotifications = 40,
                    NotificationsByType = new Dictionary<string, int>
                    {
                        { "Study", 20 },
                        { "Achievement", 8 },
                        { "Test", 10 },
                        { "System", 7 }
                    },
                    EngagementRate = 78.5f,
                    AverageReadTime = 2.5f
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notification statistics");
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

    #region Notification DTOs

    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public bool IsDelivered { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public string? ActionUrl { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }

    public class UpdateNotificationSettingsDto
    {
        public bool? EmailNotifications { get; set; }
        public bool? PushNotifications { get; set; }
        public bool? StudyReminders { get; set; }
        public bool? AchievementNotifications { get; set; }
        public bool? TestResultNotifications { get; set; }
        public bool? ChallengeNotifications { get; set; }
        public bool? SystemNotifications { get; set; }
    }

    public class SendTestNotificationDto
    {
        [Required]
        public string Channel { get; set; } = string.Empty;
        public string? Message { get; set; }
    }

    #endregion
}