using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Common
{
    /// <summary>
    /// DTO chung cho Achievement - VERSION 3.0
    /// </summary>
    public class AchievementDto
    {
        public int AchievementId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Points { get; set; }
        public string Difficulty { get; set; } = string.Empty;
        public float Progress { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? EstimatedCompletion { get; set; }
        public DateTime? UnlockedAt { get; set; }
        public string Icon { get; set; } = string.Empty;
        public List<string> Requirements { get; set; } = new List<string>();
        public List<string> Rewards { get; set; } = new List<string>();
    }

    /// <summary>
    /// DTO chung cho Period - VERSION 3.0
    /// </summary>
    public class PeriodDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    /// <summary>
    /// DTO chung cho Submit Answer - VERSION 3.0
    /// </summary>
    public class SubmitAnswerDto
    {
        [Required]
        public int QuestionId { get; set; }

        [Required]
        public string SelectedAnswer { get; set; } = string.Empty;

        public int TimeSpent { get; set; }

        public bool UsedHint { get; set; }

        public string? OpenAnswer { get; set; }
    }

    /// <summary>
    /// DTO chung cho Notification Settings - VERSION 3.0
    /// </summary>
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

    /// <summary>
    /// DTO chung cho User Progress Overview - VERSION 3.0
    /// </summary>
    public class UserProgressOverviewDto
    {
        public int UserId { get; set; }
        public int TotalStudyDays { get; set; }
        public int ConsecutiveDays { get; set; }
        public int TotalStudyTime { get; set; }
        public int AverageSessionTime { get; set; }
        public int VocabulariesLearned { get; set; }
        public int KanjisLearned { get; set; }
        public int GrammarPointsLearned { get; set; }
        public int TestsPassed { get; set; }
        public float OverallAccuracy { get; set; }
        public string CurrentLevel { get; set; } = string.Empty;
        public float NextLevelProgress { get; set; }
        public List<AchievementDto> Achievements { get; set; } = new List<AchievementDto>();
        public WeeklyStatsDto WeeklyStats { get; set; } = new WeeklyStatsDto();
        public List<ActivityDto> RecentActivities { get; set; } = new List<ActivityDto>();
    }

    /// <summary>
    /// DTO chung cho Activity - VERSION 3.0
    /// </summary>
    public class ActivityDto
    {
        public int ActivityId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public int Points { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO chung cho Weekly Stats - VERSION 3.0
    /// </summary>
    public class WeeklyStatsDto
    {
        public int StudyDays { get; set; }
        public int TotalMinutes { get; set; }
        public int VocabStudied { get; set; }
        public int KanjiStudied { get; set; }
        public int GrammarStudied { get; set; }
        public float AverageScore { get; set; }
        public int StreakDays { get; set; }
    }

    /// <summary>
    /// DTO chung cho Error Response - VERSION 3.0
    /// </summary>
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string? Detail { get; set; }
        public string? ErrorCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public Dictionary<string, string[]>? Errors { get; set; }
        public object? Details { get; set; }
    }

    /// <summary>
    /// DTO chung cho Recommendation - VERSION 3.0
    /// </summary>
    public class RecommendationDto
    {
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string ActionUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO chung cho Leaderboard Position - VERSION 3.0
    /// </summary>
    public class LeaderboardPositionDto
    {
        public int GlobalRank { get; set; }
        public int LevelRank { get; set; }
        public int WeeklyRank { get; set; }
        public int MonthlyRank { get; set; }
        public float GlobalPercentile { get; set; }
        public string TrendDirection { get; set; } = string.Empty;
        public string RankChange { get; set; } = string.Empty;
    }
}