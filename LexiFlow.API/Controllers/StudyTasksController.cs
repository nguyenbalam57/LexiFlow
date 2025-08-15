using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.StudyPlan;
using LexiFlow.API.DTOs.Common;
using System.Security.Claims;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller qu?n lý nhi?m v? h?c t?p
    /// </summary>
    [ApiController]
    [Route("api/study-goals/{goalId}/[controller]")]
    [Authorize]
    public class StudyTasksController : ControllerBase
    {
        private readonly ILogger<StudyTasksController> _logger;

        public StudyTasksController(ILogger<StudyTasksController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// L?y danh sách nhi?m v? c?a m?c tiêu h?c t?p
        /// </summary>
        /// <param name="goalId">ID m?c tiêu h?c t?p</param>
        /// <param name="page">Trang hi?n t?i</param>
        /// <param name="pageSize">S? b?n ghi m?i trang</param>
        /// <param name="search">T? khóa tìm ki?m</param>
        /// <param name="status">Tr?ng thái nhi?m v?</param>
        /// <param name="taskType">Lo?i nhi?m v?</param>
        /// <returns>Danh sách nhi?m v?</returns>
        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<StudyTaskDto>>> GetStudyTasks(
            int goalId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? status = null,
            [FromQuery] string? taskType = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var tasks = new List<StudyTaskDto>
                {
                    new StudyTaskDto
                    {
                        TaskId = 1,
                        GoalId = goalId,
                        TaskName = "H?c 20 t? v?ng m?i ngày",
                        Description = "Ghi nh? và th?c hành 20 t? v?ng m?i m?i ngày",
                        EstimatedDuration = 30,
                        DurationUnit = "Minutes",
                        Priority = 3,
                        TaskType = "Study",
                        TaskCategory = "Vocabulary",
                        ScheduledDate = DateTime.UtcNow.AddDays(1),
                        DueDate = DateTime.UtcNow.AddDays(7),
                        HasTimeConstraint = false,
                        RequiredResources = "Sách t? v?ng, Flashcards",
                        IsRequired = true,
                        IsCompleted = false,
                        Status = "NotStarted",
                        CompletionPercentage = 0,
                        IsRecurring = true,
                        RecurrencePattern = "Daily",
                        EnableReminders = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-7),
                        UpdatedAt = DateTime.UtcNow,
                        IsActive = true,
                        TaskCompletions = new List<TaskCompletionDto>()
                    },
                    new StudyTaskDto
                    {
                        TaskId = 2,
                        GoalId = goalId,
                        TaskName = "Ôn t?p t? v?ng ?ã h?c",
                        Description = "Ôn t?p l?i các t? v?ng ?ã h?c trong tu?n",
                        EstimatedDuration = 45,
                        DurationUnit = "Minutes",
                        Priority = 2,
                        TaskType = "Review",
                        TaskCategory = "Vocabulary",
                        ScheduledDate = DateTime.UtcNow.AddDays(7),
                        DueDate = DateTime.UtcNow.AddDays(7),
                        HasTimeConstraint = true,
                        RequiredResources = "Flashcards, App h?c t? v?ng",
                        IsRequired = true,
                        IsCompleted = false,
                        Status = "InProgress",
                        CompletionPercentage = 30,
                        IsRecurring = true,
                        RecurrencePattern = "Weekly",
                        EnableReminders = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-7),
                        UpdatedAt = DateTime.UtcNow.AddDays(-1),
                        IsActive = true,
                        TaskCompletions = new List<TaskCompletionDto>()
                    }
                };

                // Apply filters
                if (!string.IsNullOrEmpty(search))
                {
                    tasks = tasks.Where(t => t.TaskName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                           t.Description.Contains(search, StringComparison.OrdinalIgnoreCase))
                               .ToList();
                }

                if (!string.IsNullOrEmpty(status))
                {
                    tasks = tasks.Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                               .ToList();
                }

                if (!string.IsNullOrEmpty(taskType))
                {
                    tasks = tasks.Where(t => t.TaskType.Equals(taskType, StringComparison.OrdinalIgnoreCase))
                               .ToList();
                }

                // Pagination
                var totalCount = tasks.Count;
                var items = tasks.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var result = new PaginatedResultDto<StudyTaskDto>
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
                _logger.LogError(ex, "Error getting study tasks for goal {GoalId}", goalId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y chi ti?t nhi?m v? h?c t?p
        /// </summary>
        /// <param name="goalId">ID m?c tiêu h?c t?p</param>
        /// <param name="taskId">ID nhi?m v?</param>
        /// <returns>Chi ti?t nhi?m v?</returns>
        [HttpGet("{taskId}")]
        public async Task<ActionResult<StudyTaskDto>> GetStudyTask(int goalId, int taskId)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var task = new StudyTaskDto
                {
                    TaskId = taskId,
                    GoalId = goalId,
                    TaskName = "H?c 20 t? v?ng m?i ngày",
                    Description = "Ghi nh? và th?c hành 20 t? v?ng m?i m?i ngày",
                    EstimatedDuration = 30,
                    DurationUnit = "Minutes",
                    Priority = 3,
                    TaskType = "Study",
                    TaskCategory = "Vocabulary",
                    ScheduledDate = DateTime.UtcNow.AddDays(1),
                    DueDate = DateTime.UtcNow.AddDays(7),
                    HasTimeConstraint = false,
                    RequiredResources = "Sách t? v?ng, Flashcards",
                    IsRequired = true,
                    IsCompleted = false,
                    Status = "NotStarted",
                    CompletionPercentage = 0,
                    IsRecurring = true,
                    RecurrencePattern = "Daily",
                    EnableReminders = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true,
                    TaskCompletions = new List<TaskCompletionDto>
                    {
                        new TaskCompletionDto
                        {
                            CompletionId = 1,
                            TaskId = taskId,
                            CompletionDate = DateTime.UtcNow.AddDays(-1),
                            CompletionStatus = 100,
                            ActualDuration = 25,
                            DurationUnit = "Minutes",
                            Difficulty = 3,
                            Satisfaction = 4,
                            Effectiveness = 4,
                            Notes = "Hoàn thành t?t, c?n ôn t?p thêm",
                            Score = 85,
                            CorrectCount = 17,
                            TotalCount = 20
                        }
                    }
                };

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting study task {TaskId} for goal {GoalId}", taskId, goalId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// T?o nhi?m v? h?c t?p m?i
        /// </summary>
        /// <param name="goalId">ID m?c tiêu h?c t?p</param>
        /// <param name="createDto">Thông tin nhi?m v? m?i</param>
        /// <returns>Nhi?m v? ?ã ???c t?o</returns>
        [HttpPost]
        public async Task<ActionResult<StudyTaskDto>> CreateStudyTask(int goalId, [FromBody] CreateStudyTaskDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();
                
                var newTask = new StudyTaskDto
                {
                    TaskId = new Random().Next(100, 999),
                    GoalId = goalId,
                    TaskName = createDto.TaskName,
                    Description = createDto.Description,
                    EstimatedDuration = createDto.EstimatedDuration,
                    DurationUnit = createDto.DurationUnit,
                    Priority = createDto.Priority,
                    TaskType = createDto.TaskType,
                    TaskCategory = createDto.TaskCategory,
                    ScheduledDate = createDto.ScheduledDate,
                    DueDate = createDto.DueDate,
                    HasTimeConstraint = createDto.HasTimeConstraint,
                    RequiredResources = createDto.RequiredResources,
                    AttachmentUrls = createDto.AttachmentUrls,
                    IsRequired = createDto.IsRequired,
                    IsCompleted = false,
                    Status = "NotStarted",
                    CompletionPercentage = 0,
                    IsRecurring = createDto.IsRecurring,
                    RecurrencePattern = createDto.RecurrencePattern,
                    EnableReminders = createDto.EnableReminders,
                    ReminderSettings = createDto.ReminderSettings,
                    Dependencies = createDto.Dependencies,
                    CompletionConditions = createDto.CompletionConditions,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true,
                    TaskCompletions = new List<TaskCompletionDto>()
                };

                return CreatedAtAction(nameof(GetStudyTask), new { goalId = goalId, taskId = newTask.TaskId }, newTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating study task for goal {GoalId}", goalId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?p nh?t nhi?m v? h?c t?p
        /// </summary>
        /// <param name="goalId">ID m?c tiêu h?c t?p</param>
        /// <param name="taskId">ID nhi?m v?</param>
        /// <param name="updateDto">Thông tin c?p nh?t</param>
        /// <returns>Nhi?m v? ?ã c?p nh?t</returns>
        [HttpPut("{taskId}")]
        public async Task<ActionResult<StudyTaskDto>> UpdateStudyTask(int goalId, int taskId, [FromBody] UpdateStudyTaskDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();
                
                var updatedTask = new StudyTaskDto
                {
                    TaskId = taskId,
                    GoalId = goalId,
                    TaskName = updateDto.TaskName ?? "Updated Task",
                    Description = updateDto.Description ?? "Updated description",
                    EstimatedDuration = updateDto.EstimatedDuration,
                    DurationUnit = updateDto.DurationUnit ?? "Minutes",
                    Priority = updateDto.Priority ?? 3,
                    TaskType = updateDto.TaskType ?? "Study",
                    TaskCategory = updateDto.TaskCategory ?? "General",
                    ScheduledDate = updateDto.ScheduledDate,
                    DueDate = updateDto.DueDate,
                    HasTimeConstraint = updateDto.HasTimeConstraint ?? false,
                    RequiredResources = updateDto.RequiredResources ?? "",
                    AttachmentUrls = updateDto.AttachmentUrls ?? "",
                    IsRequired = updateDto.IsRequired ?? true,
                    IsCompleted = false,
                    Status = updateDto.Status ?? "InProgress",
                    CompletionPercentage = updateDto.CompletionPercentage ?? 25,
                    IsRecurring = updateDto.IsRecurring ?? false,
                    RecurrencePattern = updateDto.RecurrencePattern ?? "",
                    EnableReminders = updateDto.EnableReminders ?? true,
                    ReminderSettings = updateDto.ReminderSettings ?? "",
                    Dependencies = updateDto.Dependencies ?? "",
                    CompletionConditions = updateDto.CompletionConditions ?? "",
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = updateDto.IsActive ?? true,
                    TaskCompletions = new List<TaskCompletionDto>()
                };

                return Ok(updatedTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating study task {TaskId} for goal {GoalId}", taskId, goalId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Xóa nhi?m v? h?c t?p
        /// </summary>
        /// <param name="goalId">ID m?c tiêu h?c t?p</param>
        /// <param name="taskId">ID nhi?m v?</param>
        /// <returns>K?t qu? xóa</returns>
        [HttpDelete("{taskId}")]
        public async Task<ActionResult> DeleteStudyTask(int goalId, int taskId)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // TODO: Implement actual deletion logic
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting study task {TaskId} for goal {GoalId}", taskId, goalId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// ?ánh d?u nhi?m v? ?ã hoàn thành
        /// </summary>
        /// <param name="goalId">ID m?c tiêu h?c t?p</param>
        /// <param name="taskId">ID nhi?m v?</param>
        /// <param name="completionDto">Thông tin hoàn thành</param>
        /// <returns>Nhi?m v? ?ã c?p nh?t</returns>
        [HttpPost("{taskId}/complete")]
        public async Task<ActionResult<StudyTaskDto>> CompleteStudyTask(int goalId, int taskId, [FromBody] object? completionDto = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var completedTask = new StudyTaskDto
                {
                    TaskId = taskId,
                    GoalId = goalId,
                    TaskName = "H?c 20 t? v?ng m?i ngày",
                    Description = "Ghi nh? và th?c hành 20 t? v?ng m?i m?i ngày",
                    IsCompleted = true,
                    CompletedAt = DateTime.UtcNow,
                    Status = "Completed",
                    CompletionPercentage = 100,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true,
                    TaskCompletions = new List<TaskCompletionDto>()
                };

                return Ok(completedTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing study task {TaskId} for goal {GoalId}", taskId, goalId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y l?ch s? hoàn thành nhi?m v?
        /// </summary>
        /// <param name="goalId">ID m?c tiêu h?c t?p</param>
        /// <param name="taskId">ID nhi?m v?</param>
        /// <returns>L?ch s? hoàn thành</returns>
        [HttpGet("{taskId}/completions")]
        public async Task<ActionResult<List<TaskCompletionDto>>> GetTaskCompletions(int goalId, int taskId)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                var completions = new List<TaskCompletionDto>
                {
                    new TaskCompletionDto
                    {
                        CompletionId = 1,
                        TaskId = taskId,
                        CompletionDate = DateTime.UtcNow.AddDays(-1),
                        CompletionStatus = 100,
                        ActualDuration = 25,
                        DurationUnit = "Minutes",
                        Difficulty = 3,
                        Satisfaction = 4,
                        Effectiveness = 4,
                        Notes = "Hoàn thành t?t",
                        Score = 85,
                        CorrectCount = 17,
                        TotalCount = 20
                    },
                    new TaskCompletionDto
                    {
                        CompletionId = 2,
                        TaskId = taskId,
                        CompletionDate = DateTime.UtcNow.AddDays(-2),
                        CompletionStatus = 90,
                        ActualDuration = 30,
                        DurationUnit = "Minutes",
                        Difficulty = 4,
                        Satisfaction = 3,
                        Effectiveness = 3,
                        Notes = "C?n ôn t?p thêm",
                        Score = 75,
                        CorrectCount = 15,
                        TotalCount = 20
                    }
                };

                return Ok(completions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting task completions for task {TaskId}", taskId);
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
            return 1;
        }
    }
}