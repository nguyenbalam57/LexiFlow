using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý ti?n ?? h?c t?p c?a ng??i dùng
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserProgressController : ControllerBase
    {
        private readonly ILogger<UserProgressController> _logger;

        public UserProgressController(ILogger<UserProgressController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// L?y t?ng quan ti?n ?? h?c t?p
        /// </summary>
        [HttpGet("overview")]
        public async Task<ActionResult<UserProgressOverviewDto>> GetProgressOverview()
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var overview = new UserProgressOverviewDto
                {
                    UserId = userId,
                    TotalStudyDays = 45,
                    ConsecutiveDays = 7,
                    TotalStudyTime = 2700, // minutes
                    AverageSessionTime = 45,
                    VocabulariesLearned = 320,
                    KanjisLearned = 85,
                    GrammarPointsLearned = 25,
                    TestsPassed = 12,
                    OverallAccuracy = 84.5f,
                    CurrentLevel = "N4",
                    NextLevelProgress = 65.2f,
                    Achievements = new List<AchievementDto>
                    {
                        new AchievementDto
                        {
                            AchievementId = 1,
                            Title = "First Week",
                            Description = "Study for 7 consecutive days",
                            Icon = "??",
                            UnlockedAt = DateTime.UtcNow.AddDays(-7)
                        }
                    },
                    WeeklyStats = new WeeklyStatsDto
                    {
                        StudyDays = 5,
                        TotalMinutes = 225,
                        VocabStudied = 45,
                        KanjiStudied = 12,
                        GrammarStudied = 3
                    },
                    RecentActivities = new List<ActivityDto>
                    {
                        new ActivityDto
                        {
                            ActivityId = 1,
                            Type = "Vocabulary",
                            Description = "Studied 15 new words",
                            Timestamp = DateTime.UtcNow.AddHours(-2),
                            Points = 15
                        }
                    }
                };

                return Ok(overview);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting progress overview for user {UserId}", GetCurrentUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y ti?n ?? t? v?ng
        /// </summary>
        [HttpGet("vocabulary")]
        public async Task<ActionResult<VocabularyProgressDto>> GetVocabularyProgress(
            [FromQuery] string? level = null,
            [FromQuery] int? categoryId = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var progress = new VocabularyProgressDto
                {
                    UserId = userId,
                    TotalWords = 320,
                    MasteredWords = 240,
                    LearningWords = 60,
                    NewWords = 20,
                    AccuracyRate = 87.5f,
                    ByLevel = new Dictionary<string, int>
                    {
                        { "N5", 150 },
                        { "N4", 120 },
                        { "N3", 50 }
                    },
                    ByCategory = new Dictionary<string, int>
                    {
                        { "Greetings", 25 },
                        { "Education", 40 },
                        { "Family", 20 },
                        { "Food", 35 }
                    },
                    WeeklyProgress = new int[] { 5, 8, 12, 15, 18, 20, 22 },
                    RecentWords = new List<VocabProgressItemDto>
                    {
                        new VocabProgressItemDto
                        {
                            VocabularyId = 1,
                            Word = "?????",
                            Romaji = "konnichiwa",
                            Meaning = "Hello",
                            Status = "Mastered",
                            Accuracy = 95.0f,
                            StudiedCount = 10,
                            LastStudied = DateTime.UtcNow.AddHours(-3)
                        }
                    }
                };

                return Ok(progress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabulary progress for user {UserId}", GetCurrentUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y ti?n ?? Kanji
        /// </summary>
        [HttpGet("kanji")]
        public async Task<ActionResult<KanjiProgressDto>> GetKanjiProgress(
            [FromQuery] string? level = null,
            [FromQuery] int? minStrokes = null,
            [FromQuery] int? maxStrokes = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var progress = new KanjiProgressDto
                {
                    UserId = userId,
                    TotalKanji = 85,
                    MasteredKanji = 60,
                    LearningKanji = 20,
                    NewKanji = 5,
                    AccuracyRate = 82.3f,
                    ByLevel = new Dictionary<string, int>
                    {
                        { "N5", 50 },
                        { "N4", 25 },
                        { "N3", 10 }
                    },
                    ByStrokes = new Dictionary<string, int>
                    {
                        { "1-5", 30 },
                        { "6-10", 35 },
                        { "11-15", 15 },
                        { "16+", 5 }
                    },
                    WeeklyProgress = new int[] { 2, 3, 4, 5, 6, 7, 8 },
                    RecentKanji = new List<KanjiProgressItemDto>
                    {
                        new KanjiProgressItemDto
                        {
                            KanjiId = 1,
                            Character = "?",
                            OnYomi = "?????",
                            KunYomi = "??",
                            Meaning = "person",
                            Status = "Mastered",
                            Accuracy = 90.0f,
                            StudiedCount = 8,
                            LastStudied = DateTime.UtcNow.AddHours(-5)
                        }
                    }
                };

                return Ok(progress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting kanji progress for user {UserId}", GetCurrentUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y ti?n ?? ng? pháp
        /// </summary>
        [HttpGet("grammar")]
        public async Task<ActionResult<GrammarProgressDto>> GetGrammarProgress(
            [FromQuery] string? level = null,
            [FromQuery] string? grammarType = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var progress = new GrammarProgressDto
                {
                    UserId = userId,
                    TotalGrammar = 25,
                    MasteredGrammar = 18,
                    LearningGrammar = 5,
                    NewGrammar = 2,
                    AccuracyRate = 89.2f,
                    ByLevel = new Dictionary<string, int>
                    {
                        { "N5", 15 },
                        { "N4", 8 },
                        { "N3", 2 }
                    },
                    ByType = new Dictionary<string, int>
                    {
                        { "Particles", 8 },
                        { "Conjugations", 10 },
                        { "Expressions", 7 }
                    },
                    WeeklyProgress = new int[] { 1, 1, 2, 2, 3, 3, 4 },
                    RecentGrammar = new List<GrammarProgressItemDto>
                    {
                        new GrammarProgressItemDto
                        {
                            GrammarId = 1,
                            Title = "??",
                            Pattern = "[Noun] + ??",
                            Meaning = "To be (polite)",
                            Status = "Mastered",
                            Accuracy = 94.0f,
                            StudiedCount = 12,
                            LastStudied = DateTime.UtcNow.AddDays(-1)
                        }
                    }
                };

                return Ok(progress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting grammar progress for user {UserId}", GetCurrentUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y l?ch s? h?c t?p
        /// </summary>
        [HttpGet("history")]
        public async Task<ActionResult<PaginatedResultDto<StudyHistoryDto>>> GetStudyHistory(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] string? contentType = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var history = new List<StudyHistoryDto>
                {
                    new StudyHistoryDto
                    {
                        HistoryId = 1,
                        UserId = userId,
                        Date = DateTime.UtcNow.Date,
                        ContentType = "Vocabulary",
                        ItemsStudied = 15,
                        TimeSpent = 45,
                        Accuracy = 87.5f,
                        Score = 88,
                        Notes = "Good progress on N5 vocabulary",
                        CreatedAt = DateTime.UtcNow
                    },
                    new StudyHistoryDto
                    {
                        HistoryId = 2,
                        UserId = userId,
                        Date = DateTime.UtcNow.Date.AddDays(-1),
                        ContentType = "Kanji",
                        ItemsStudied = 8,
                        TimeSpent = 30,
                        Accuracy = 92.0f,
                        Score = 91,
                        Notes = "Focused on stroke order practice",
                        CreatedAt = DateTime.UtcNow.AddDays(-1)
                    }
                };

                // Apply filters
                if (fromDate.HasValue)
                {
                    history = history.Where(h => h.Date >= fromDate.Value.Date).ToList();
                }

                if (toDate.HasValue)
                {
                    history = history.Where(h => h.Date <= toDate.Value.Date).ToList();
                }

                if (!string.IsNullOrEmpty(contentType))
                {
                    history = history.Where(h => h.ContentType.Equals(contentType, StringComparison.OrdinalIgnoreCase))
                                    .ToList();
                }

                var totalCount = history.Count;
                var items = history.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<StudyHistoryDto>
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
                _logger.LogError(ex, "Error getting study history for user {UserId}", GetCurrentUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y danh sách thành tích
        /// </summary>
        [HttpGet("achievements")]
        public async Task<ActionResult<List<AchievementDto>>> GetAchievements([FromQuery] bool? unlocked = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var achievements = new List<AchievementDto>
                {
                    new AchievementDto
                    {
                        AchievementId = 1,
                        Title = "First Steps",
                        Description = "Complete your first study session",
                        Icon = "??",
                        Type = "Milestone",
                        RequiredValue = 1,
                        CurrentValue = 1,
                        IsUnlocked = true,
                        UnlockedAt = DateTime.UtcNow.AddDays(-45),
                        Points = 10
                    },
                    new AchievementDto
                    {
                        AchievementId = 2,
                        Title = "Week Warrior",
                        Description = "Study for 7 consecutive days",
                        Icon = "??",
                        Type = "Streak",
                        RequiredValue = 7,
                        CurrentValue = 7,
                        IsUnlocked = true,
                        UnlockedAt = DateTime.UtcNow.AddDays(-7),
                        Points = 50
                    },
                    new AchievementDto
                    {
                        AchievementId = 3,
                        Title = "Kanji Master",
                        Description = "Learn 100 kanji characters",
                        Icon = "?",
                        Type = "Progress",
                        RequiredValue = 100,
                        CurrentValue = 85,
                        IsUnlocked = false,
                        Points = 100
                    }
                };

                if (unlocked.HasValue)
                {
                    achievements = achievements.Where(a => a.IsUnlocked == unlocked.Value).ToList();
                }

                return Ok(achievements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting achievements for user {UserId}", GetCurrentUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t ti?n ?? h?c t?p
        /// </summary>
        [HttpPost("update")]
        public async Task<ActionResult<object>> UpdateProgress([FromBody] UpdateProgressDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();
                
                // TODO: Implement actual progress update logic
                
                var result = new
                {
                    Success = true,
                    Message = "Progress updated successfully",
                    PointsEarned = updateDto.ItemsStudied * 2,
                    NewAchievements = new List<string>(),
                    UpdatedAt = DateTime.UtcNow
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating progress for user {UserId}", GetCurrentUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y th?ng kê ti?n ??
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetProgressStatistics(
            [FromQuery] string period = "month") // week, month, year, all
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var statistics = new
                {
                    Period = period,
                    StudyDays = 22,
                    TotalMinutes = 990,
                    ItemsStudied = 156,
                    AverageAccuracy = 86.7f,
                    AverageScore = 84.2f,
                    StrongestArea = "Vocabulary",
                    WeakestArea = "Grammar",
                    ProgressTrend = "Improving",
                    DailyStats = new[]
                    {
                        new { Date = DateTime.UtcNow.Date.AddDays(-6), Minutes = 45, Items = 12, Accuracy = 85.0f },
                        new { Date = DateTime.UtcNow.Date.AddDays(-5), Minutes = 30, Items = 8, Accuracy = 90.0f },
                        new { Date = DateTime.UtcNow.Date.AddDays(-4), Minutes = 60, Items = 15, Accuracy = 82.0f },
                        new { Date = DateTime.UtcNow.Date.AddDays(-3), Minutes = 40, Items = 10, Accuracy = 88.0f },
                        new { Date = DateTime.UtcNow.Date.AddDays(-2), Minutes = 50, Items = 13, Accuracy = 91.0f },
                        new { Date = DateTime.UtcNow.Date.AddDays(-1), Minutes = 35, Items = 9, Accuracy = 87.0f },
                        new { Date = DateTime.UtcNow.Date, Minutes = 55, Items = 14, Accuracy = 89.0f }
                    }
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting progress statistics for user {UserId}", GetCurrentUserId());
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

    public class VocabularyProgressDto
    {
        public int UserId { get; set; }
        public int TotalWords { get; set; }
        public int MasteredWords { get; set; }
        public int LearningWords { get; set; }
        public int NewWords { get; set; }
        public float AccuracyRate { get; set; }
        public Dictionary<string, int> ByLevel { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ByCategory { get; set; } = new Dictionary<string, int>();
        public int[] WeeklyProgress { get; set; } = new int[7];
        public List<VocabProgressItemDto> RecentWords { get; set; } = new List<VocabProgressItemDto>();
    }

    public class KanjiProgressDto
    {
        public int UserId { get; set; }
        public int TotalKanji { get; set; }
        public int MasteredKanji { get; set; }
        public int LearningKanji { get; set; }
        public int NewKanji { get; set; }
        public float AccuracyRate { get; set; }
        public Dictionary<string, int> ByLevel { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ByStrokes { get; set; } = new Dictionary<string, int>();
        public int[] WeeklyProgress { get; set; } = new int[7];
        public List<KanjiProgressItemDto> RecentKanji { get; set; } = new List<KanjiProgressItemDto>();
    }

    public class GrammarProgressDto
    {
        public int UserId { get; set; }
        public int TotalGrammar { get; set; }
        public int MasteredGrammar { get; set; }
        public int LearningGrammar { get; set; }
        public int NewGrammar { get; set; }
        public float AccuracyRate { get; set; }
        public Dictionary<string, int> ByLevel { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ByType { get; set; } = new Dictionary<string, int>();
        public int[] WeeklyProgress { get; set; } = new int[7];
        public List<GrammarProgressItemDto> RecentGrammar { get; set; } = new List<GrammarProgressItemDto>();
    }

    public class StudyHistoryDto
    {
        public int HistoryId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public int ItemsStudied { get; set; }
        public int TimeSpent { get; set; }
        public float Accuracy { get; set; }
        public int Score { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class AchievementDto
    {
        public int AchievementId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int RequiredValue { get; set; }
        public int CurrentValue { get; set; }
        public bool IsUnlocked { get; set; }
        public DateTime? UnlockedAt { get; set; }
        public int Points { get; set; }
    }

    public class WeeklyStatsDto
    {
        public int StudyDays { get; set; }
        public int TotalMinutes { get; set; }
        public int VocabStudied { get; set; }
        public int KanjiStudied { get; set; }
        public int GrammarStudied { get; set; }
    }

    public class ActivityDto
    {
        public int ActivityId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public int Points { get; set; }
    }

    public class VocabProgressItemDto
    {
        public int VocabularyId { get; set; }
        public string Word { get; set; } = string.Empty;
        public string Romaji { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public float Accuracy { get; set; }
        public int StudiedCount { get; set; }
        public DateTime LastStudied { get; set; }
    }

    public class KanjiProgressItemDto
    {
        public int KanjiId { get; set; }
        public string Character { get; set; } = string.Empty;
        public string OnYomi { get; set; } = string.Empty;
        public string KunYomi { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public float Accuracy { get; set; }
        public int StudiedCount { get; set; }
        public DateTime LastStudied { get; set; }
    }

    public class GrammarProgressItemDto
    {
        public int GrammarId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Pattern { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public float Accuracy { get; set; }
        public int StudiedCount { get; set; }
        public DateTime LastStudied { get; set; }
    }

    public class UpdateProgressDto
    {
        [Required]
        public string ContentType { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000)]
        public int ItemsStudied { get; set; }

        [Required]
        [Range(1, 600)]
        public int TimeSpent { get; set; }

        [Range(0, 100)]
        public float Accuracy { get; set; }

        [Range(0, 100)]
        public int Score { get; set; }

        public string Notes { get; set; } = string.Empty;
    }

    #endregion
}