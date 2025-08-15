using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.StudyPlan;
using LexiFlow.API.DTOs.Common;
using System.Security.Claims;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý m?c tiêu h?c t?p
    /// </summary>
    [ApiController]
    [Route("api/study-plans/{planId}/[controller]")]
    [Authorize]
    public class StudyGoalsController : ControllerBase
    {
        private readonly ILogger<StudyGoalsController> _logger;

        public StudyGoalsController(ILogger<StudyGoalsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// L?y danh sách m?c tiêu c?a k? ho?ch h?c t?p
        /// </summary>
        /// <param name="planId">ID k? ho?ch h?c t?p</param>
        /// <returns>Danh sách m?c tiêu</returns>
        [HttpGet]
        public async Task<ActionResult<List<StudyGoalDto>>> GetStudyGoals(int planId)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var goals = new List<StudyGoalDto>
                {
                    new StudyGoalDto
                    {
                        GoalId = 1,
                        PlanId = planId,
                        GoalName = "H?c t? v?ng N5",
                        GoalType = "Vocabulary",
                        Description = "H?c 300 t? v?ng N5",
                        TargetCount = 300,
                        TargetDate = DateTime.UtcNow.AddDays(60),
                        IsCompleted = false,
                        CreatedAt = DateTime.UtcNow.AddDays(-30),
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = userId,
                        IsActive = true,
                        Tasks = new List<StudyTaskDto>(),
                        ChildGoals = new List<StudyGoalDto>()
                    }
                };

                return Ok(goals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting study goals for plan {PlanId}", planId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y chi ti?t m?c tiêu h?c t?p
        /// </summary>
        /// <param name="planId">ID k? ho?ch h?c t?p</param>
        /// <param name="goalId">ID m?c tiêu</param>
        /// <returns>Chi ti?t m?c tiêu</returns>
        [HttpGet("{goalId}")]
        public async Task<ActionResult<StudyGoalDto>> GetStudyGoal(int planId, int goalId)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var goal = new StudyGoalDto
                {
                    GoalId = goalId,
                    PlanId = planId,
                    GoalName = "H?c t? v?ng N5",
                    GoalType = "Vocabulary",
                    Description = "H?c 300 t? v?ng N5",
                    TargetCount = 300,
                    TargetDate = DateTime.UtcNow.AddDays(60),
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    IsActive = true,
                    Tasks = new List<StudyTaskDto>(),
                    ChildGoals = new List<StudyGoalDto>()
                };

                return Ok(goal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting study goal {GoalId} for plan {PlanId}", goalId, planId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// T?o m?c tiêu h?c t?p m?i
        /// </summary>
        /// <param name="planId">ID k? ho?ch h?c t?p</param>
        /// <param name="createDto">Thông tin m?c tiêu</param>
        /// <returns>M?c tiêu ?ã ???c t?o</returns>
        [HttpPost]
        public async Task<ActionResult<StudyGoalDto>> CreateStudyGoal(int planId, [FromBody] CreateStudyGoalDto createDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var newGoal = new StudyGoalDto
                {
                    GoalId = new Random().Next(100, 999),
                    PlanId = planId,
                    GoalName = createDto.GoalName,
                    GoalType = createDto.GoalType,
                    Description = createDto.Description,
                    TargetDate = createDto.TargetDate,
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    IsActive = true,
                    Tasks = new List<StudyTaskDto>(),
                    ChildGoals = new List<StudyGoalDto>()
                };

                return CreatedAtAction(nameof(GetStudyGoal), new { planId = planId, goalId = newGoal.GoalId }, newGoal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating study goal for plan {PlanId}", planId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t m?c tiêu h?c t?p
        /// </summary>
        /// <param name="planId">ID k? ho?ch h?c t?p</param>
        /// <param name="goalId">ID m?c tiêu</param>
        /// <param name="updateDto">Thông tin c?p nh?t</param>
        /// <returns>M?c tiêu ?ã c?p nh?t</returns>
        [HttpPut("{goalId}")]
        public async Task<ActionResult<StudyGoalDto>> UpdateStudyGoal(int planId, int goalId, [FromBody] UpdateStudyGoalDto updateDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var updatedGoal = new StudyGoalDto
                {
                    GoalId = goalId,
                    PlanId = planId,
                    GoalName = updateDto.GoalName ?? "Updated Goal",
                    GoalType = updateDto.GoalType ?? "Vocabulary",
                    Description = updateDto.Description ?? "Updated description",
                    TargetDate = updateDto.TargetDate,
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    IsActive = updateDto.IsActive ?? true,
                    Tasks = new List<StudyTaskDto>(),
                    ChildGoals = new List<StudyGoalDto>()
                };

                return Ok(updatedGoal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating study goal {GoalId} for plan {PlanId}", goalId, planId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Xóa m?c tiêu h?c t?p
        /// </summary>
        /// <param name="planId">ID k? ho?ch h?c t?p</param>
        /// <param name="goalId">ID m?c tiêu</param>
        /// <returns>K?t qu? xóa</returns>
        [HttpDelete("{goalId}")]
        public async Task<ActionResult> DeleteStudyGoal(int planId, int goalId)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // TODO: Implement actual deletion logic
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting study goal {GoalId} for plan {PlanId}", goalId, planId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// ?ánh d?u m?c tiêu ?ã hoàn thành
        /// </summary>
        /// <param name="planId">ID k? ho?ch h?c t?p</param>
        /// <param name="goalId">ID m?c tiêu</param>
        /// <returns>M?c tiêu ?ã c?p nh?t</returns>
        [HttpPost("{goalId}/complete")]
        public async Task<ActionResult<StudyGoalDto>> CompleteStudyGoal(int planId, int goalId)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var completedGoal = new StudyGoalDto
                {
                    GoalId = goalId,
                    PlanId = planId,
                    GoalName = "H?c t? v?ng N5",
                    GoalType = "Vocabulary",
                    Description = "H?c 300 t? v?ng N5",
                    TargetCount = 300,
                    TargetDate = DateTime.UtcNow.AddDays(60),
                    IsCompleted = true,
                    CompletedDate = DateTime.UtcNow,
                    ProgressPercentage = 100,
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    IsActive = true,
                    Tasks = new List<StudyTaskDto>(),
                    ChildGoals = new List<StudyGoalDto>()
                };

                return Ok(completedGoal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing study goal {GoalId} for plan {PlanId}", goalId, planId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y ID ng??i dùng hi?n t?i t? JWT token
        /// </summary>
        /// <returns>User ID</returns>
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            
            // Fallback for development
            return 1;
        }
    }
}