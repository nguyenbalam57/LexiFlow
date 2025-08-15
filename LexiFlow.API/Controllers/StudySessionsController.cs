using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.StudyPlan;
using LexiFlow.API.DTOs.Common;
using System.Security.Claims;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý phiên h?c t?p
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudySessionsController : ControllerBase
    {
        private readonly ILogger<StudySessionsController> _logger;

        public StudySessionsController(ILogger<StudySessionsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// L?y danh sách phiên h?c t?p c?a ng??i dùng
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<StudySessionDto>>> GetStudySessions(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sessionType = null,
            [FromQuery] string? contentType = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var sessions = new List<StudySessionDto>
                {
                    new StudySessionDto
                    {
                        SessionId = 1,
                        UserId = userId,
                        StartTime = DateTime.UtcNow.AddHours(-2),
                        EndTime = DateTime.UtcNow.AddHours(-1),
                        Duration = 3600,
                        SessionType = "Study",
                        ContentType = "Vocabulary",
                        StudyPlanId = 1,
                        ItemsStudied = 20,
                        CorrectAnswers = 15,
                        WrongAnswers = 5,
                        Score = 75.0f,
                        Platform = "Web",
                        IsSynced = true,
                        CreatedAt = DateTime.UtcNow.AddHours(-2),
                        UpdatedAt = DateTime.UtcNow.AddHours(-1),
                        IsActive = true
                    },
                    new StudySessionDto
                    {
                        SessionId = 2,
                        UserId = userId,
                        StartTime = DateTime.UtcNow.AddDays(-1).AddHours(-1),
                        EndTime = DateTime.UtcNow.AddDays(-1),
                        Duration = 2700,
                        SessionType = "Practice",
                        ContentType = "Grammar",
                        StudyPlanId = 1,
                        ItemsStudied = 15,
                        CorrectAnswers = 12,
                        WrongAnswers = 3,
                        Score = 80.0f,
                        Platform = "Mobile",
                        IsSynced = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-1).AddHours(-1),
                        UpdatedAt = DateTime.UtcNow.AddDays(-1),
                        IsActive = true
                    }
                };

                // Apply filters
                if (!string.IsNullOrEmpty(sessionType))
                {
                    sessions = sessions.Where(s => s.SessionType.Equals(sessionType, StringComparison.OrdinalIgnoreCase))
                                     .ToList();
                }

                if (!string.IsNullOrEmpty(contentType))
                {
                    sessions = sessions.Where(s => s.ContentType.Equals(contentType, StringComparison.OrdinalIgnoreCase))
                                     .ToList();
                }

                if (fromDate.HasValue)
                {
                    sessions = sessions.Where(s => s.StartTime >= fromDate.Value).ToList();
                }

                if (toDate.HasValue)
                {
                    sessions = sessions.Where(s => s.StartTime <= toDate.Value).ToList();
                }

                var totalCount = sessions.Count;
                var items = sessions.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<StudySessionDto>
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
                _logger.LogError(ex, "Error getting study sessions for user {UserId}", GetCurrentUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y chi ti?t phiên h?c t?p
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<StudySessionDto>> GetStudySession(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var session = new StudySessionDto
                {
                    SessionId = id,
                    UserId = userId,
                    StartTime = DateTime.UtcNow.AddHours(-2),
                    EndTime = DateTime.UtcNow.AddHours(-1),
                    Duration = 3600,
                    SessionType = "Study",
                    ContentType = "Vocabulary",
                    StudyPlanId = 1,
                    ItemsStudied = 20,
                    CorrectAnswers = 15,
                    WrongAnswers = 5,
                    Score = 75.0f,
                    Platform = "Web",
                    IsSynced = true,
                    CreatedAt = DateTime.UtcNow.AddHours(-2),
                    UpdatedAt = DateTime.UtcNow.AddHours(-1),
                    IsActive = true
                };

                return Ok(session);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting study session {SessionId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// B?t ??u phiên h?c t?p m?i
        /// </summary>
        [HttpPost("start")]
        public async Task<ActionResult<StudySessionDto>> StartStudySession([FromBody] object sessionData)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var newSession = new StudySessionDto
                {
                    SessionId = new Random().Next(100, 999),
                    UserId = userId,
                    StartTime = DateTime.UtcNow,
                    EndTime = null,
                    Duration = 0,
                    SessionType = "Study",
                    ContentType = "Vocabulary",
                    ItemsStudied = 0,
                    CorrectAnswers = 0,
                    WrongAnswers = 0,
                    Score = 0,
                    Platform = "Web",
                    IsSynced = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                return CreatedAtAction(nameof(GetStudySession), new { id = newSession.SessionId }, newSession);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting study session");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// K?t thúc phiên h?c t?p
        /// </summary>
        [HttpPost("{id}/end")]
        public async Task<ActionResult<StudySessionDto>> EndStudySession(int id, [FromBody] object sessionData)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var endedSession = new StudySessionDto
                {
                    SessionId = id,
                    UserId = userId,
                    StartTime = DateTime.UtcNow.AddHours(-1),
                    EndTime = DateTime.UtcNow,
                    Duration = 3600,
                    SessionType = "Study",
                    ContentType = "Vocabulary",
                    ItemsStudied = 20,
                    CorrectAnswers = 15,
                    WrongAnswers = 5,
                    Score = 75.0f,
                    IsSynced = false,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                return Ok(endedSession);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ending study session {SessionId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t phiên h?c t?p
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<StudySessionDto>> UpdateStudySession(int id, [FromBody] object updateData)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var updatedSession = new StudySessionDto
                {
                    SessionId = id,
                    UserId = userId,
                    StartTime = DateTime.UtcNow.AddHours(-1),
                    Duration = 1800,
                    SessionType = "Study",
                    ContentType = "Vocabulary",
                    ItemsStudied = 10,
                    CorrectAnswers = 8,
                    WrongAnswers = 2,
                    Score = 80.0f,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                return Ok(updatedSession);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating study session {SessionId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Xóa phiên h?c t?p
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudySession(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // TODO: Implement actual deletion logic
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting study session {SessionId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y th?ng kê phiên h?c t?p
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetStudySessionStatistics(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var statistics = new
                {
                    TotalSessions = 45,
                    TotalStudyTime = 27000,
                    AverageSessionTime = 600,
                    TotalItemsStudied = 450,
                    OverallAccuracy = 84.4f,
                    AverageScore = 82.1f,
                    ConsecutiveDays = 7,
                    MostStudiedContent = "Vocabulary",
                    PreferredStudyTime = "Evening",
                    WeeklyProgress = new int[] { 2, 3, 1, 4, 2, 3, 2 },
                    MonthlyProgress = new float[] { 78.5f, 81.2f, 84.4f, 82.9f }
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting study session statistics");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// ??ng b? phiên h?c t?p
        /// </summary>
        [HttpPost("sync")]
        public async Task<ActionResult<object>> SyncStudySessions([FromBody] List<int> sessionIds)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var syncResult = new
                {
                    SyncedSessions = sessionIds.Count,
                    FailedSessions = 0,
                    SyncTime = DateTime.UtcNow,
                    Message = "All sessions synced successfully"
                };

                return Ok(syncResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing study sessions");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y ID ng??i dùng hi?n t?i t? JWT token
        /// </summary>
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