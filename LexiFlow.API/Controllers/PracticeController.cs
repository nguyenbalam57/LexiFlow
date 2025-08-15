using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using LexiFlow.API.Extensions;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý luy?n t?p và bài t?p
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PracticeController : ControllerBase
    {
        private readonly ILogger<PracticeController> _logger;

        public PracticeController(ILogger<PracticeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// L?y danh sách practice sets
        /// </summary>
        [HttpGet("sets")]
        public async Task<ActionResult<PaginatedResultDto<PracticeSetDto>>> GetPracticeSets(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? setType = null,
            [FromQuery] string? level = null,
            [FromQuery] string? category = null,
            [FromQuery] bool? isPublic = null,
            [FromQuery] string? sortBy = "CreatedAt",
            [FromQuery] string? sortDirection = "desc")
        {
            try
            {
                var practiceSets = new List<PracticeSetDto>
                {
                    new PracticeSetDto
                    {
                        PracticeSetId = 1,
                        SetName = "N5 Vocabulary Basics",
                        SetType = "Vocabulary",
                        Level = "N5",
                        Description = "Essential vocabulary for JLPT N5 level",
                        Skills = "Reading, Recognition",
                        ItemCount = 50,
                        EstimatedMinutes = 30,
                        Difficulty = 2,
                        Tags = "JLPT, N5, Essential, Beginner",
                        CategoryName = "Basic Vocabulary",
                        UsageCount = 1250,
                        AverageRating = 4.5,
                        RatingCount = 180,
                        IsPublic = true,
                        IsFeatured = true,
                        IsActive = true,
                        CreatedByUserName = "Admin",
                        CreatedAt = DateTime.UtcNow.AddDays(-30),
                        UserProgress = new UserPracticeProgressDto
                        {
                            IsStarted = true,
                            CompletionPercentage = 75.0f,
                            CorrectPercentage = 82.5f,
                            LastPracticed = DateTime.UtcNow.AddDays(-2),
                            BestScore = 88.0f,
                            SessionCount = 5,
                            TotalTime = 125
                        }
                    }
                };

                // Apply filters
                if (!string.IsNullOrEmpty(setType))
                {
                    practiceSets = practiceSets.Where(ps => ps.SetType.Equals(setType, StringComparison.OrdinalIgnoreCase))
                                              .ToList();
                }

                if (!string.IsNullOrEmpty(level))
                {
                    practiceSets = practiceSets.Where(ps => ps.Level.Equals(level, StringComparison.OrdinalIgnoreCase))
                                              .ToList();
                }

                if (!string.IsNullOrEmpty(category))
                {
                    practiceSets = practiceSets.Where(ps => ps.CategoryName?.Contains(category, StringComparison.OrdinalIgnoreCase) == true)
                                              .ToList();
                }

                if (isPublic.HasValue)
                {
                    practiceSets = practiceSets.Where(ps => ps.IsPublic == isPublic.Value).ToList();
                }

                // Apply sorting
                practiceSets = sortBy?.ToLower() switch
                {
                    "name" => sortDirection?.ToLower() == "desc"
                        ? practiceSets.OrderByDescending(ps => ps.SetName).ToList()
                        : practiceSets.OrderBy(ps => ps.SetName).ToList(),
                    "rating" => sortDirection?.ToLower() == "desc"
                        ? practiceSets.OrderByDescending(ps => ps.AverageRating).ToList()
                        : practiceSets.OrderBy(ps => ps.AverageRating).ToList(),
                    "usage" => sortDirection?.ToLower() == "desc"
                        ? practiceSets.OrderByDescending(ps => ps.UsageCount).ToList()
                        : practiceSets.OrderBy(ps => ps.UsageCount).ToList(),
                    "difficulty" => sortDirection?.ToLower() == "desc"
                        ? practiceSets.OrderByDescending(ps => ps.Difficulty).ToList()
                        : practiceSets.OrderBy(ps => ps.Difficulty).ToList(),
                    _ => sortDirection?.ToLower() == "desc"
                        ? practiceSets.OrderByDescending(ps => ps.CreatedAt).ToList()
                        : practiceSets.OrderBy(ps => ps.CreatedAt).ToList()
                };

                var totalCount = practiceSets.Count;
                var items = practiceSets.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<PracticeSetDto>
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
                _logger.LogError(ex, "Error getting practice sets");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// G?i câu tr? l?i
        /// </summary>
        [HttpPost("sessions/{sessionId}/answer")]
        public async Task<ActionResult<object>> SubmitAnswer(
            string sessionId, 
            [FromBody] SubmitAnswerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // S? d?ng properties t? request object
                var response = new
                {
                    IsCorrect = request.Answer == "B",
                    CorrectAnswer = "B", 
                    Explanation = "????? (konnichiwa) is the standard greeting used during the day.",
                    PointsEarned = request.Answer == "B" ? 10 : 0,
                    Feedback = request.Answer == "B" 
                        ? "Excellent! You got it right." 
                        : "Not quite. ????? is used during daytime hours.",
                    TimeSpent = request.TimeSpent,
                    SessionProgress = new
                    {
                        TotalQuestions = 50,
                        CurrentQuestion = 2,
                        AnsweredQuestions = 1,
                        CorrectAnswers = request.Answer == "B" ? 1 : 0,
                        SkippedQuestions = 0,
                        TimeSpent = request.TimeSpent,
                        Score = request.Answer == "B" ? 100.0f : 0.0f
                    },
                    NextAction = "NextQuestion",
                    BonusPoints = 0,
                    StreakCount = request.Answer == "B" ? 1 : 0
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting answer for session {SessionId}", sessionId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y th?ng kê luy?n t?p
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetPracticeStatistics(
            [FromQuery] int? days = 30)
        {
            try
            {
                var userId = GetCurrentUserId();
                var endDate = DateTime.UtcNow;
                var startDate = endDate.AddDays(-(days ?? 30));

                var statistics = new
                {
                    Period = new PeriodDto { StartDate = startDate, EndDate = endDate },
                    OverallStats = new
                    {
                        TotalSessions = 25,
                        TotalQuestionsAnswered = 1250,
                        TotalTimeSpent = 42000,
                        AverageScore = 82.5f,
                        AverageAccuracy = 84.2f,
                        BestScore = 96.0f,
                        CurrentStreak = 7,
                        LongestStreak = 15
                    },
                    WeeklyProgress = new[]
                    {
                        new
                        {
                            Week = endDate.StartOfWeek().AddDays(-21),
                            Sessions = 5,
                            AverageScore = 78.5f,
                            TotalTime = 8400,
                            Accuracy = 80.1f
                        },
                        new
                        {
                            Week = endDate.StartOfWeek().AddDays(-14),
                            Sessions = 7,
                            AverageScore = 81.2f,
                            TotalTime = 11200,
                            Accuracy = 82.8f
                        }
                    }
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting practice statistics");
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

    #region Practice DTOs

    public class SubmitAnswerRequest
    {
        [Required]
        public int QuestionId { get; set; }

        [Required]
        public string Answer { get; set; } = string.Empty;

        public int TimeSpent { get; set; }

        public bool UsedHint { get; set; }
    }

    public class PracticeSetDto
    {
        public int PracticeSetId { get; set; }
        public string SetName { get; set; } = string.Empty;
        public string SetType { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Skills { get; set; } = string.Empty;
        public int ItemCount { get; set; }
        public int? EstimatedMinutes { get; set; }
        public int Difficulty { get; set; }
        public string Tags { get; set; } = string.Empty;
        public string? CategoryName { get; set; }
        public int UsageCount { get; set; }
        public double? AverageRating { get; set; }
        public int RatingCount { get; set; }
        public bool IsPublic { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; }
        public string CreatedByUserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public UserPracticeProgressDto? UserProgress { get; set; }
    }

    public class UserPracticeProgressDto
    {
        public bool IsStarted { get; set; }
        public float CompletionPercentage { get; set; }
        public float CorrectPercentage { get; set; }
        public DateTime? LastPracticed { get; set; }
        public float? BestScore { get; set; }
        public int SessionCount { get; set; }
        public int? TotalTime { get; set; }
        public int? UserRating { get; set; }
    }

    #endregion
}