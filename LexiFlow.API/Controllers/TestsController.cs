using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý bài ki?m tra và thi th?
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TestsController : ControllerBase
    {
        private readonly ILogger<TestsController> _logger;

        public TestsController(ILogger<TestsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// L?y danh sách bài ki?m tra
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<TestDto>>> GetTests(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? testType = null,
            [FromQuery] string? level = null,
            [FromQuery] string? category = null)
        {
            try
            {
                var tests = new List<TestDto>
                {
                    new TestDto
                    {
                        TestId = 1,
                        Title = "JLPT N5 Mock Test - Vocabulary",
                        Description = "Practice test for JLPT N5 vocabulary section",
                        TestType = "Vocabulary",
                        Level = "N5",
                        Category = "JLPT",
                        Duration = 30,
                        QuestionCount = 50,
                        PassingScore = 70,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-10),
                        UpdatedAt = DateTime.UtcNow
                    },
                    new TestDto
                    {
                        TestId = 2,
                        Title = "Basic Kanji Recognition",
                        Description = "Test your knowledge of basic kanji characters",
                        TestType = "Kanji",
                        Level = "N5",
                        Category = "Practice",
                        Duration = 20,
                        QuestionCount = 30,
                        PassingScore = 75,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-8),
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                // Apply filters
                if (!string.IsNullOrEmpty(testType))
                {
                    tests = tests.Where(t => t.TestType.Equals(testType, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!string.IsNullOrEmpty(level))
                {
                    tests = tests.Where(t => t.Level.Equals(level, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!string.IsNullOrEmpty(category))
                {
                    tests = tests.Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                var totalCount = tests.Count;
                var items = tests.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<TestDto>
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
                _logger.LogError(ex, "Error getting tests");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y chi ti?t bài ki?m tra
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TestDetailDto>> GetTest(int id)
        {
            try
            {
                var test = new TestDetailDto
                {
                    TestId = id,
                    Title = "JLPT N5 Mock Test - Vocabulary",
                    Description = "Comprehensive practice test for JLPT N5 vocabulary section covering all major topics",
                    TestType = "Vocabulary",
                    Level = "N5",
                    Category = "JLPT",
                    Duration = 30,
                    QuestionCount = 50,
                    PassingScore = 70,
                    Instructions = "Choose the best answer for each question. You have 30 minutes to complete this test.",
                    Tags = new List<string> { "JLPT", "N5", "vocabulary", "practice" },
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow,
                    Questions = new List<QuestionDto>
                    {
                        new QuestionDto
                        {
                            QuestionId = 1,
                            QuestionText = "What does '?????' mean?",
                            QuestionType = "MultipleChoice",
                            Points = 2,
                            Options = new List<OptionDto>
                            {
                                new OptionDto { OptionId = 1, Text = "Good morning", IsCorrect = false },
                                new OptionDto { OptionId = 2, Text = "Good afternoon/Hello", IsCorrect = true },
                                new OptionDto { OptionId = 3, Text = "Good evening", IsCorrect = false },
                                new OptionDto { OptionId = 4, Text = "Good night", IsCorrect = false }
                            }
                        }
                    }
                };

                return Ok(test);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting test {TestId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// B?t ??u làm bài ki?m tra
        /// </summary>
        [HttpPost("{id}/start")]
        public async Task<ActionResult<TestSessionDto>> StartTest(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var session = new TestSessionDto
                {
                    SessionId = new Random().Next(1000, 9999),
                    TestId = id,
                    UserId = userId,
                    StartTime = DateTime.UtcNow,
                    Duration = 30,
                    Status = "InProgress",
                    CurrentQuestionIndex = 0,
                    TotalQuestions = 50,
                    TimeRemaining = 1800, // 30 minutes in seconds
                    Answers = new List<TestAnswerDto>()
                };

                return Ok(session);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting test {TestId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// N?p câu tr? l?i
        /// </summary>
        [HttpPost("sessions/{sessionId}/answer")]
        public async Task<ActionResult<object>> SubmitAnswer(int sessionId, [FromBody] SubmitAnswerDto answerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = new
                {
                    Success = true,
                    QuestionId = answerDto.QuestionId,
                    IsCorrect = new Random().Next(0, 2) == 1, // Mock random result
                    NextQuestionIndex = answerDto.QuestionIndex + 1,
                    Message = "Answer submitted successfully"
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting answer for session {SessionId}", sessionId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Hoàn thành bài ki?m tra
        /// </summary>
        [HttpPost("sessions/{sessionId}/complete")]
        public async Task<ActionResult<TestResultDto>> CompleteTest(int sessionId)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var result = new TestResultDto
                {
                    SessionId = sessionId,
                    UserId = userId,
                    TestId = 1,
                    Score = 85,
                    CorrectAnswers = 42,
                    TotalQuestions = 50,
                    TimeSpent = 1200, // 20 minutes
                    IsPassed = true,
                    CompletedAt = DateTime.UtcNow,
                    Feedback = "Excellent work! You have a strong grasp of N5 vocabulary.",
                    DetailedResults = new List<QuestionResultDto>
                    {
                        new QuestionResultDto
                        {
                            QuestionId = 1,
                            IsCorrect = true,
                            UserAnswer = "Good afternoon/Hello",
                            CorrectAnswer = "Good afternoon/Hello",
                            TimeSpent = 15
                        }
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing test session {SessionId}", sessionId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y l?ch s? thi c?a ng??i dùng
        /// </summary>
        [HttpGet("history")]
        public async Task<ActionResult<PaginatedResultDto<TestHistoryDto>>> GetTestHistory(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var history = new List<TestHistoryDto>
                {
                    new TestHistoryDto
                    {
                        SessionId = 1,
                        TestId = 1,
                        TestTitle = "JLPT N5 Mock Test - Vocabulary",
                        Score = 85,
                        IsPassed = true,
                        TimeSpent = 1200,
                        CompletedAt = DateTime.UtcNow.AddDays(-2)
                    },
                    new TestHistoryDto
                    {
                        SessionId = 2,
                        TestId = 2,
                        TestTitle = "Basic Kanji Recognition",
                        Score = 78,
                        IsPassed = true,
                        TimeSpent = 900,
                        CompletedAt = DateTime.UtcNow.AddDays(-5)
                    }
                };

                var totalCount = history.Count;
                var items = history.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<TestHistoryDto>
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
                _logger.LogError(ex, "Error getting test history for user {UserId}", GetCurrentUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y th?ng kê thi c?
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetTestStatistics()
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var statistics = new
                {
                    TotalTestsTaken = 12,
                    TestsPassed = 10,
                    AverageScore = 82.5f,
                    BestScore = 95,
                    TotalTimeSpent = 300, // minutes
                    ByTestType = new
                    {
                        Vocabulary = new { Taken = 5, Passed = 4, AvgScore = 85.2f },
                        Kanji = new { Taken = 4, Passed = 4, AvgScore = 78.5f },
                        Grammar = new { Taken = 3, Passed = 2, AvgScore = 84.7f }
                    },
                    ByLevel = new
                    {
                        N5 = new { Taken = 8, Passed = 7, AvgScore = 83.1f },
                        N4 = new { Taken = 4, Passed = 3, AvgScore = 81.2f }
                    },
                    RecentTests = new[]
                    {
                        new { Date = DateTime.UtcNow.Date.AddDays(-1), Score = 88, TestType = "Vocabulary" },
                        new { Date = DateTime.UtcNow.Date.AddDays(-3), Score = 75, TestType = "Kanji" },
                        new { Date = DateTime.UtcNow.Date.AddDays(-7), Score = 92, TestType = "Grammar" }
                    }
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting test statistics for user {UserId}", GetCurrentUserId());
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

    public class TestDto
    {
        public int TestId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TestType { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Duration { get; set; }
        public int QuestionCount { get; set; }
        public int PassingScore { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class TestDetailDto : TestDto
    {
        public string Instructions { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }

    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string QuestionType { get; set; } = string.Empty;
        public int Points { get; set; }
        public List<OptionDto> Options { get; set; } = new List<OptionDto>();
    }

    public class OptionDto
    {
        public int OptionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }

    public class TestSessionDto
    {
        public int SessionId { get; set; }
        public int TestId { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; } = string.Empty;
        public int CurrentQuestionIndex { get; set; }
        public int TotalQuestions { get; set; }
        public int TimeRemaining { get; set; }
        public List<TestAnswerDto> Answers { get; set; } = new List<TestAnswerDto>();
    }

    public class TestAnswerDto
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; } = string.Empty;
        public DateTime AnsweredAt { get; set; }
        public int TimeSpent { get; set; }
    }

    public class SubmitAnswerDto
    {
        [Required]
        public int QuestionId { get; set; }

        [Required]
        public string Answer { get; set; } = string.Empty;

        public int QuestionIndex { get; set; }
        public int TimeSpent { get; set; }
    }

    public class TestResultDto
    {
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        public int Score { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public int TimeSpent { get; set; }
        public bool IsPassed { get; set; }
        public DateTime CompletedAt { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public List<QuestionResultDto> DetailedResults { get; set; } = new List<QuestionResultDto>();
    }

    public class QuestionResultDto
    {
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }
        public string UserAnswer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public int TimeSpent { get; set; }
    }

    public class TestHistoryDto
    {
        public int SessionId { get; set; }
        public int TestId { get; set; }
        public string TestTitle { get; set; } = string.Empty;
        public int Score { get; set; }
        public bool IsPassed { get; set; }
        public int TimeSpent { get; set; }
        public DateTime CompletedAt { get; set; }
    }

    #endregion
}