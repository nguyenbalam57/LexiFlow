using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý cài ??t ng??i dùng và h? th?ng
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SettingsController : ControllerBase
    {
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(ILogger<SettingsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// L?y t?t c? cài ??t ng??i dùng
        /// </summary>
        [HttpGet("user")]
        public async Task<ActionResult<UserSettingsDto>> GetUserSettings()
        {
            try
            {
                var userId = GetCurrentUserId();

                var settings = new UserSettingsDto
                {
                    UserId = userId,
                    GeneralSettings = new GeneralSettingsDto
                    {
                        Language = "vi",
                        Theme = "light",
                        TimeZone = "Asia/Ho_Chi_Minh",
                        DateFormat = "dd/MM/yyyy",
                        TimeFormat = "24h",
                        StartOfWeek = "Monday"
                    },
                    NotificationSettings = new NotificationSettingsDto
                    {
                        EmailNotifications = true,
                        PushNotifications = true,
                        StudyReminders = true,
                        GoalReminders = true,
                        StreakReminders = true,
                        AchievementNotifications = true,
                        WeeklyReports = true,
                        ReminderTime = "09:00",
                        StudyBreakReminders = true,
                        QuietHoursStart = "22:00",
                        QuietHoursEnd = "08:00"
                    },
                    LearningSettings = new LearningSettingsDto
                    {
                        DefaultDifficulty = "Intermediate",
                        StudyGoalHours = 1.0f,
                        PreferredLearningStyle = "Visual",
                        AutoAdvance = true,
                        ShowHints = true,
                        PlayAudio = true,
                        RepeatIncorrect = true,
                        ShuffleQuestions = true,
                        TimerEnabled = false,
                        SessionTimeout = 30,
                        MaxDailyReviews = 50,
                        SpacedRepetition = true
                    },
                    PrivacySettings = new PrivacySettingsDto
                    {
                        ShareProgress = false,
                        ShowInLeaderboard = true,
                        AllowDataCollection = true,
                        ShareUsageStats = false,
                        PublicProfile = false,
                        ShareAchievements = true
                    },
                    InterfaceSettings = new InterfaceSettingsDto
                    {
                        FontSize = "medium",
                        HighContrast = false,
                        ReducedMotion = false,
                        CompactView = false,
                        ShowProgressBars = true,
                        ShowStatistics = true,
                        AutoSave = true,
                        ConfirmBeforeExit = true
                    },
                    BackupSettings = new BackupSettingsDto
                    {
                        AutoBackup = true,
                        BackupFrequency = "weekly",
                        BackupToCloud = true,
                        RetainBackups = 30,
                        IncludeMediaFiles = false
                    }
                };

                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user settings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t cài ??t chung
        /// </summary>
        [HttpPut("user/general")]
        public async Task<ActionResult> UpdateGeneralSettings([FromBody] GeneralSettingsDto settings)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                // TODO: Update settings in database

                return Ok(new { Message = "General settings updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating general settings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t cài ??t thông báo
        /// </summary>
        [HttpPut("user/notifications")]
        public async Task<ActionResult> UpdateNotificationSettings([FromBody] NotificationSettingsDto settings)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                // TODO: Update settings in database

                return Ok(new { Message = "Notification settings updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating notification settings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t cài ??t h?c t?p
        /// </summary>
        [HttpPut("user/learning")]
        public async Task<ActionResult> UpdateLearningSettings([FromBody] LearningSettingsDto settings)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                // TODO: Update settings in database

                return Ok(new { Message = "Learning settings updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating learning settings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t cài ??t quy?n riêng t?
        /// </summary>
        [HttpPut("user/privacy")]
        public async Task<ActionResult> UpdatePrivacySettings([FromBody] PrivacySettingsDto settings)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                // TODO: Update settings in database

                return Ok(new { Message = "Privacy settings updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating privacy settings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t cài ??t giao di?n
        /// </summary>
        [HttpPut("user/interface")]
        public async Task<ActionResult> UpdateInterfaceSettings([FromBody] InterfaceSettingsDto settings)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                // TODO: Update settings in database

                return Ok(new { Message = "Interface settings updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating interface settings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t cài ??t sao l?u
        /// </summary>
        [HttpPut("user/backup")]
        public async Task<ActionResult> UpdateBackupSettings([FromBody] BackupSettingsDto settings)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                // TODO: Update settings in database

                return Ok(new { Message = "Backup settings updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating backup settings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y cài ??t h? th?ng (ch? admin)
        /// </summary>
        [HttpGet("system")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SystemSettingsDto>> GetSystemSettings()
        {
            try
            {
                var settings = new SystemSettingsDto
                {
                    MaintenanceMode = false,
                    AllowRegistration = true,
                    MaxUsersPerPlan = 1000,
                    SessionTimeout = 60,
                    MaxFileUploadSize = 10,
                    SupportedLanguages = new List<string> { "vi", "en", "ja" },
                    DefaultUserRole = "User",
                    RequireEmailVerification = true,
                    EnableGuestAccess = false,
                    RateLimitingEnabled = true,
                    MaxRequestsPerMinute = 100,
                    BackupRetentionDays = 30,
                    LogLevel = "Information",
                    EnableAnalytics = true,
                    DataRetentionDays = 365
                };

                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting system settings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t cài ??t h? th?ng (ch? admin)
        /// </summary>
        [HttpPut("system")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateSystemSettings([FromBody] SystemSettingsDto settings)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // TODO: Update system settings in database

                return Ok(new { Message = "System settings updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating system settings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Reset cài ??t v? m?c ??nh
        /// </summary>
        [HttpPost("user/reset")]
        public async Task<ActionResult> ResetUserSettings([FromBody] ResetSettingsRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                // TODO: Reset specified settings to default values

                return Ok(new
                {
                    Message = "Settings reset successfully",
                    ResetCategories = request.Categories,
                    ResetTime = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting user settings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Export cài ??t ng??i dùng
        /// </summary>
        [HttpGet("user/export")]
        public async Task<ActionResult<UserSettingsExportDto>> ExportUserSettings()
        {
            try
            {
                var userId = GetCurrentUserId();

                var export = new UserSettingsExportDto
                {
                    UserId = userId,
                    ExportTime = DateTime.UtcNow,
                    Version = "1.0",
                    Settings = await GetUserSettingsInternal(userId)
                };

                return Ok(export);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting user settings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Import cài ??t ng??i dùng
        /// </summary>
        [HttpPost("user/import")]
        public async Task<ActionResult> ImportUserSettings([FromBody] UserSettingsImportDto import)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                // TODO: Validate and import settings

                return Ok(new
                {
                    Message = "Settings imported successfully",
                    ImportedCategories = new List<string> { "General", "Notifications", "Learning" },
                    ImportTime = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing user settings");
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

        private async Task<object> GetUserSettingsInternal(int userId)
        {
            // TODO: Get actual user settings from database
            return new { };
        }
    }

    #region DTOs

    public class UserSettingsDto
    {
        public int UserId { get; set; }
        public GeneralSettingsDto GeneralSettings { get; set; } = new GeneralSettingsDto();
        public NotificationSettingsDto NotificationSettings { get; set; } = new NotificationSettingsDto();
        public LearningSettingsDto LearningSettings { get; set; } = new LearningSettingsDto();
        public PrivacySettingsDto PrivacySettings { get; set; } = new PrivacySettingsDto();
        public InterfaceSettingsDto InterfaceSettings { get; set; } = new InterfaceSettingsDto();
        public BackupSettingsDto BackupSettings { get; set; } = new BackupSettingsDto();
        public DateTime LastUpdated { get; set; }
    }

    public class GeneralSettingsDto
    {
        [Required]
        public string Language { get; set; } = "vi";
        public string Theme { get; set; } = "light";
        public string TimeZone { get; set; } = "Asia/Ho_Chi_Minh";
        public string DateFormat { get; set; } = "dd/MM/yyyy";
        public string TimeFormat { get; set; } = "24h";
        public string StartOfWeek { get; set; } = "Monday";
    }

    public class NotificationSettingsDto
    {
        public bool EmailNotifications { get; set; } = true;
        public bool PushNotifications { get; set; } = true;
        public bool StudyReminders { get; set; } = true;
        public bool GoalReminders { get; set; } = true;
        public bool StreakReminders { get; set; } = true;
        public bool AchievementNotifications { get; set; } = true;
        public bool WeeklyReports { get; set; } = true;
        public string ReminderTime { get; set; } = "09:00";
        public bool StudyBreakReminders { get; set; } = true;
        public string QuietHoursStart { get; set; } = "22:00";
        public string QuietHoursEnd { get; set; } = "08:00";
    }

    public class LearningSettingsDto
    {
        public string DefaultDifficulty { get; set; } = "Intermediate";
        [Range(0.1, 24.0)]
        public float StudyGoalHours { get; set; } = 1.0f;
        public string PreferredLearningStyle { get; set; } = "Visual";
        public bool AutoAdvance { get; set; } = true;
        public bool ShowHints { get; set; } = true;
        public bool PlayAudio { get; set; } = true;
        public bool RepeatIncorrect { get; set; } = true;
        public bool ShuffleQuestions { get; set; } = true;
        public bool TimerEnabled { get; set; } = false;
        [Range(5, 120)]
        public int SessionTimeout { get; set; } = 30;
        [Range(10, 200)]
        public int MaxDailyReviews { get; set; } = 50;
        public bool SpacedRepetition { get; set; } = true;
    }

    public class PrivacySettingsDto
    {
        public bool ShareProgress { get; set; } = false;
        public bool ShowInLeaderboard { get; set; } = true;
        public bool AllowDataCollection { get; set; } = true;
        public bool ShareUsageStats { get; set; } = false;
        public bool PublicProfile { get; set; } = false;
        public bool ShareAchievements { get; set; } = true;
    }

    public class InterfaceSettingsDto
    {
        public string FontSize { get; set; } = "medium";
        public bool HighContrast { get; set; } = false;
        public bool ReducedMotion { get; set; } = false;
        public bool CompactView { get; set; } = false;
        public bool ShowProgressBars { get; set; } = true;
        public bool ShowStatistics { get; set; } = true;
        public bool AutoSave { get; set; } = true;
        public bool ConfirmBeforeExit { get; set; } = true;
    }

    public class BackupSettingsDto
    {
        public bool AutoBackup { get; set; } = true;
        public string BackupFrequency { get; set; } = "weekly";
        public bool BackupToCloud { get; set; } = true;
        [Range(1, 365)]
        public int RetainBackups { get; set; } = 30;
        public bool IncludeMediaFiles { get; set; } = false;
    }

    public class SystemSettingsDto
    {
        public bool MaintenanceMode { get; set; }
        public bool AllowRegistration { get; set; }
        public int MaxUsersPerPlan { get; set; }
        public int SessionTimeout { get; set; }
        public int MaxFileUploadSize { get; set; }
        public List<string> SupportedLanguages { get; set; } = new List<string>();
        public string DefaultUserRole { get; set; } = string.Empty;
        public bool RequireEmailVerification { get; set; }
        public bool EnableGuestAccess { get; set; }
        public bool RateLimitingEnabled { get; set; }
        public int MaxRequestsPerMinute { get; set; }
        public int BackupRetentionDays { get; set; }
        public string LogLevel { get; set; } = string.Empty;
        public bool EnableAnalytics { get; set; }
        public int DataRetentionDays { get; set; }
    }

    public class ResetSettingsRequestDto
    {
        [Required]
        public List<string> Categories { get; set; } = new List<string>();
        public bool ConfirmReset { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class UserSettingsExportDto
    {
        public int UserId { get; set; }
        public DateTime ExportTime { get; set; }
        public string Version { get; set; } = string.Empty;
        public object Settings { get; set; } = new { };
    }

    public class UserSettingsImportDto
    {
        [Required]
        public string Version { get; set; } = string.Empty;
        [Required]
        public object Settings { get; set; } = new { };
        public bool OverwriteExisting { get; set; } = false;
        public List<string>? ImportCategories { get; set; }
    }

    #endregion
}