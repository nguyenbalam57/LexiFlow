using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using LexiFlow.API.Extensions;
using System.Security.Claims;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller h? th?ng gamification - ?i?m s?, huy hi?u, th? thách
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GamificationController : ControllerBase
    {
        private readonly ILogger<GamificationController> _logger;

        public GamificationController(ILogger<GamificationController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// L?y thông tin gamification t?ng quan c?a user
        /// </summary>
        [HttpGet("profile")]
        public async Task<ActionResult<object>> GetGamificationProfile()
        {
            try
            {
                var userId = GetCurrentUserId();

                var profile = new
                {
                    UserId = userId,
                    TotalPoints = 2580,
                    CurrentLevel = new
                    {
                        LevelNumber = 15,
                        LevelName = "Advanced Scholar",
                        CurrentExp = 1280,
                        ExpToNextLevel = 1720,
                        ProgressToNext = 42.7f
                    },
                    Achievements = new
                    {
                        TotalAchievements = 12,
                        CompletedAchievements = 8,
                        NextMilestone = new
                        {
                            AchievementId = 1,
                            Title = "Grammar Guru",
                            Description = "Master all N4 grammar points",
                            Points = 150,
                            Progress = 78.5f
                        }
                    },
                    Leaderboard = new LeaderboardPositionDto
                    {
                        GlobalRank = 1250,
                        LevelRank = 85,
                        WeeklyRank = 42,
                        MonthlyRank = 158,
                        GlobalPercentile = 75.0f,
                        TrendDirection = "Up",
                        RankChange = "+15"
                    }
                };

                return Ok(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting gamification profile");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y danh sách achievements
        /// </summary>
        [HttpGet("achievements")]
        public async Task<ActionResult<object>> GetAchievements(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? status = null)
        {
            try
            {
                var achievements = new[]
                {
                    new
                    {
                        AchievementId = 1,
                        Title = "Grammar Guru",
                        Description = "Master all N4 grammar points",
                        Category = "Grammar",
                        Points = 150,
                        Difficulty = "Medium",
                        Progress = 78.5f,
                        IsCompleted = false
                    },
                    new
                    {
                        AchievementId = 2,
                        Title = "Speed Reader",
                        Description = "Read 10 articles in Japanese",
                        Category = "Reading",
                        Points = 100,
                        Difficulty = "Easy",
                        Progress = 100.0f,
                        IsCompleted = true
                    }
                };

                var totalCount = achievements.Length;
                var items = achievements.Skip((page - 1) * pageSize).Take(pageSize).ToArray();

                var result = new
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
                _logger.LogError(ex, "Error getting achievements");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y leaderboard
        /// </summary>
        [HttpGet("leaderboard")]
        public async Task<ActionResult<object>> GetLeaderboard(
            [FromQuery] string? type = "global",
            [FromQuery] string? period = "all-time")
        {
            try
            {
                var leaderboard = new
                {
                    Type = type ?? "global",
                    Period = period ?? "all-time",
                    LastUpdated = DateTime.UtcNow.AddMinutes(-5),
                    TotalParticipants = 5000,
                    CurrentUserPosition = 1250,
                    Entries = new[]
                    {
                        new
                        {
                            Position = 1,
                            UserId = 123,
                            Username = "StudyMaster",
                            DisplayName = "Study Master",
                            Points = 15680,
                            Level = 28,
                            Badge = "??",
                            Country = "JP"
                        },
                        new
                        {
                            Position = 2,
                            UserId = 456,
                            Username = "JapanLover",
                            DisplayName = "Japan Lover", 
                            Points = 14250,
                            Level = 26,
                            Badge = "??",
                            Country = "US"
                        }
                    }
                };

                return Ok(leaderboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leaderboard");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Tham gia challenge
        /// </summary>
        [HttpPost("challenges/{challengeId}/join")]
        public async Task<ActionResult<object>> JoinChallenge(int challengeId)
        {
            try
            {
                var userId = GetCurrentUserId();

                var result = new
                {
                    Success = true,
                    Message = "Successfully joined the challenge",
                    ChallengeId = challengeId,
                    UserId = userId,
                    JoinedAt = DateTime.UtcNow,
                    StartingProgress = 0.0f,
                    NextCheckpoint = DateTime.UtcNow.AddDays(1)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error joining challenge {ChallengeId}", challengeId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y thông tin ?i?m s? và rewards
        /// </summary>
        [HttpGet("points")]
        public async Task<ActionResult<object>> GetPointsInfo()
        {
            try
            {
                var userId = GetCurrentUserId();

                var pointsInfo = new
                {
                    TotalPoints = 2580,
                    AvailablePoints = 450,
                    SpentPoints = 2130,
                    PointsToday = 85,
                    PointsThisWeek = 320,
                    PointsThisMonth = 1250,
                    RecentTransactions = new[]
                    {
                        new
                        {
                            TransactionId = 1,
                            Amount = 15,
                            Type = "Earned",
                            Source = "Daily Study Session",
                            Description = "Completed 30-minute study session",
                            Timestamp = DateTime.UtcNow.AddHours(-1)
                        }
                    }
                };

                return Ok(pointsInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting points info");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y daily tasks
        /// </summary>
        [HttpGet("daily-tasks")]
        public async Task<ActionResult<object>> GetDailyTasks()
        {
            try
            {
                var tasks = new
                {
                    Date = DateTime.UtcNow.Date,
                    TasksCompleted = 2,
                    TotalTasks = 5,
                    BonusMultiplier = 1.5f,
                    TotalRewards = 150,
                    Tasks = new[]
                    {
                        new
                        {
                            TaskId = 1,
                            TaskName = "Study for 30 minutes",
                            Description = "Complete at least 30 minutes of study",
                            Category = "Study Time",
                            Target = 30,
                            Current = 35,
                            Unit = "minutes",
                            Progress = 100.0f,
                            IsCompleted = true,
                            CompletedAt = DateTime.UtcNow.AddHours(-2)
                        }
                    }
                };

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting daily tasks");
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
}