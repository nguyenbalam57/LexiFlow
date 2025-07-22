using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LexiFlow.API.Models;
using LexiFlow.Core.Entities;
using LexiFlow.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LexiFlow.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/sync")]
    public class SyncController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SyncController> _logger;

        public SyncController(IUnitOfWork unitOfWork, ILogger<SyncController> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets changes for a specific table since the specified timestamp
        /// </summary>
        [HttpGet("{tableName}")]
        public async Task<ActionResult<List<SyncItemDto>>> GetChanges(string tableName, [FromQuery] DateTime? lastSyncTime = null)
        {
            try
            {
                // Get the current user ID for filtering
                int currentUserId = GetCurrentUserId();

                // Define the result list
                var result = new List<SyncItemDto>();

                // Get changes based on table name
                switch (tableName.ToLowerInvariant())
                {
                    case "users":
                        if (!User.IsInRole("Admin"))
                        {
                            return Forbid();
                        }

                        result = await GetUserChangesAsync(lastSyncTime);
                        break;

                    case "roles":
                        if (!User.IsInRole("Admin"))
                        {
                            return Forbid();
                        }

                        result = await GetRoleChangesAsync(lastSyncTime);
                        break;

                    case "categories":
                        result = await GetCategoryChangesAsync(lastSyncTime);
                        break;

                    case "vocabularyitems":
                        result = await GetVocabularyChangesAsync(lastSyncTime, currentUserId);
                        break;

                    case "lessons":
                        result = await GetLessonChangesAsync(lastSyncTime, currentUserId);
                        break;

                    case "courses":
                        result = await GetCourseChangesAsync(lastSyncTime, currentUserId);
                        break;

                    case "exercises":
                        result = await GetExerciseChangesAsync(lastSyncTime, currentUserId);
                        break;

                    default:
                        return BadRequest(new ApiResponse { Success = false, Message = $"Unsupported table: {tableName}" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting changes for table {TableName}", tableName);
                return StatusCode(500, new ApiResponse { Success = false, Message = $"Error getting changes for table {tableName}" });
            }
        }

        /// <summary>
        /// Applies changes to a specific table
        /// </summary>
        [HttpPost("{tableName}")]
        public async Task<ActionResult<ApiResponse>> ApplyChanges(string tableName, [FromBody] List<SyncItemDto> changes)
        {
            try
            {
                // Get the current user ID
                int currentUserId = GetCurrentUserId();

                // Track statistics
                int created = 0;
                int updated = 0;
                int deleted = 0;
                int errors = 0;

                // Process changes based on table name
                switch (tableName.ToLowerInvariant())
                {
                    case "users":
                        if (!User.IsInRole("Admin"))
                        {
                            return Forbid();
                        }

                        (created, updated, deleted, errors) = await ApplyUserChangesAsync(changes, currentUserId);
                        break;

                    case "roles":
                        if (!User.IsInRole("Admin"))
                        {
                            return Forbid();
                        }

                        (created, updated, deleted, errors) = await ApplyRoleChangesAsync(changes, currentUserId);
                        break;

                    case "categories":
                        (created, updated, deleted, errors) = await ApplyCategoryChangesAsync(changes, currentUserId);
                        break;

                    case "vocabularyitems":
                        (created, updated, deleted, errors) = await ApplyVocabularyChangesAsync(changes, currentUserId);
                        break;

                    case "lessons":
                        (created, updated, deleted, errors) = await ApplyLessonChangesAsync(changes, currentUserId);
                        break;

                    case "courses":
                        (created, updated, deleted, errors) = await ApplyCourseChangesAsync(changes, currentUserId);
                        break;

                    case "exercises":
                        (created, updated, deleted, errors) = await ApplyExerciseChangesAsync(changes, currentUserId);
                        break;

                    default:
                        return BadRequest(new ApiResponse { Success = false, Message = $"Unsupported table: {tableName}" });
                }

                var message = $"Changes applied to {tableName}: " +
                              $"{created} created, {updated} updated, {deleted} deleted, {errors} errors";

                return Ok(new ApiResponse
                {
                    Success = errors == 0,
                    Message = message,
                    Data = new
                    {
                        Created = created,
                        Updated = updated,
                        Deleted = deleted,
                        Errors = errors
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying changes to table {TableName}", tableName);
                return StatusCode(500, new ApiResponse { Success = false, Message = $"Error applying changes to table {tableName}" });
            }
        }

        /// <summary>
        /// Gets the current server timestamp for synchronization
        /// </summary>
        [HttpGet("timestamp")]
        public ActionResult<SyncTimestampDto> GetServerTimestamp()
        {
            return Ok(new SyncTimestampDto
            {
                Timestamp = DateTime.UtcNow
            });
        }

        #region Get Changes Methods

        private async Task<List<SyncItemDto>> GetUserChangesAsync(DateTime? lastSyncTime)
        {
            var result = new List<SyncItemDto>();

            // Query condition
            var condition = lastSyncTime.HasValue
                ? (Func<User, bool>)(u => u.ModifiedAt >= lastSyncTime || u.CreatedAt >= lastSyncTime)
                : (u => true);

            // Get users
            var users = await _unitOfWork.Users.GetAsync(condition);

            // Convert to sync items
            foreach (var user in users)
            {
                var item = new SyncItemDto
                {
                    Id = user.Id.ToString(),
                    TableName = "Users",
                    SyncAction = SyncAction.Update,
                    Timestamp = user.ModifiedAt ?? user.CreatedAt,
                    RowVersion = user.RowVersionString,
                    Data = System.Text.Json.JsonSerializer.Serialize(MapEntityToDto(user))
                };

                result.Add(item);
            }

            // Get deleted users if supported
            // Implementation depends on your database schema

            return result;
        }

        private async Task<List<SyncItemDto>> GetRoleChangesAsync(DateTime? lastSyncTime)
        {
            var result = new List<SyncItemDto>();

            // Query condition
            var condition = lastSyncTime.HasValue
                ? (Func<Role, bool>)(r => r.ModifiedAt >= lastSyncTime || r.CreatedAt >= lastSyncTime)
                : (r => true);

            // Get roles
            var roles = await _unitOfWork.Roles.GetAsync(condition);

            // Convert to sync items
            foreach (var role in roles)
            {
                var item = new SyncItemDto
                {
                    Id = role.Id.ToString(),
                    TableName = "Roles",
                    SyncAction = SyncAction.Update,
                    Timestamp = role.ModifiedAt ?? role.CreatedAt,
                    RowVersion = role.RowVersionString,
                    Data = System.Text.Json.JsonSerializer.Serialize(MapEntityToDto(role))
                };

                result.Add(item);
            }

            return result;
        }

        private async Task<List<SyncItemDto>> GetCategoryChangesAsync(DateTime? lastSyncTime)
        {
            var result = new List<SyncItemDto>();

            // Query condition
            var condition = lastSyncTime.HasValue
                ? (Func<Category, bool>)(c => c.ModifiedAt >= lastSyncTime || c.CreatedAt >= lastSyncTime)
                : (c => true);

            // Get categories
            var categories = await _unitOfWork.Categories.GetAsync(condition);

            // Convert to sync items
            foreach (var category in categories)
            {
                var item = new SyncItemDto
                {
                    Id = category.Id.ToString(),
                    TableName = "Categories",
                    SyncAction = SyncAction.Update,
                    Timestamp = category.ModifiedAt ?? category.CreatedAt,
                    RowVersion = category.RowVersionString,
                    Data = System.Text.Json.JsonSerializer.Serialize(MapEntityToDto(category))
                };

                result.Add(item);
            }

            return result;
        }

        private async Task<List<SyncItemDto>> GetVocabularyChangesAsync(DateTime? lastSyncTime, int userId)
        {
            var result = new List<SyncItemDto>();

            // Query condition
            var condition = lastSyncTime.HasValue
                ? (Func<VocabularyItem, bool>)(v => v.ModifiedAt >= lastSyncTime || v.CreatedAt >= lastSyncTime)
                : (v => true);

            // Get vocabulary items
            var items = await _unitOfWork.VocabularyItems.GetAsync(condition);

            // Convert to sync items
            foreach (var item in items)
            {
                var syncItem = new SyncItemDto
                {
                    Id = item.Id.ToString(),
                    TableName = "VocabularyItems",
                    SyncAction = SyncAction.Update,
                    Timestamp = item.ModifiedAt ?? item.CreatedAt,
                    RowVersion = item.RowVersionString,
                    Data = System.Text.Json.JsonSerializer.Serialize(MapEntityToDto(item))
                };

                result.Add(syncItem);
            }

            return result;
        }

        private async Task<List<SyncItemDto>> GetLessonChangesAsync(DateTime? lastSyncTime, int userId)
        {
            var result = new List<SyncItemDto>();

            // Query condition
            var condition = lastSyncTime.HasValue
                ? (Func<Lesson, bool>)(l => l.ModifiedAt >= lastSyncTime || l.CreatedAt >= lastSyncTime)
                : (l => true);

            // Get lessons
            var lessons = await _unitOfWork.Lessons.GetAsync(condition);

            // Convert to sync items
            foreach (var lesson in lessons)
            {
                var item = new SyncItemDto
                {
                    Id = lesson.Id.ToString(),
                    TableName = "Lessons",
                    SyncAction = SyncAction.Update,
                    Timestamp = lesson.ModifiedAt ?? lesson.CreatedAt,
                    RowVersion = lesson.RowVersionString,
                    Data = System.Text.Json.JsonSerializer.Serialize(MapEntityToDto(lesson))
                };

                result.Add(item);
            }

            return result;
        }

        private async Task<List<SyncItemDto>> GetCourseChangesAsync(DateTime? lastSyncTime, int userId)
        {
            var result = new List<SyncItemDto>();

            // Query condition
            var condition = lastSyncTime.HasValue
                ? (Func<Course, bool>)(c => c.ModifiedAt >= lastSyncTime || c.CreatedAt >= lastSyncTime)
                : (c => true);

            // Get courses
            var courses = await _unitOfWork.Courses.GetAsync(condition);

            // Convert to sync items
            foreach (var course in courses)
            {
                var item = new SyncItemDto
                {
                    Id = course.Id.ToString(),
                    TableName = "Courses",
                    SyncAction = SyncAction.Update,
                    Timestamp = course.ModifiedAt ?? course.CreatedAt,
                    RowVersion = course.RowVersionString,
                    Data = System.Text.Json.JsonSerializer.Serialize(MapEntityToDto(course))
                };

                result.Add(item);
            }

            return result;
        }

        private async Task<List<SyncItemDto>> GetExerciseChangesAsync(DateTime? lastSyncTime, int userId)
        {
            var result = new List<SyncItemDto>();

            // Query condition
            var condition = lastSyncTime.HasValue
                ? (Func<Exercise, bool>)(e => e.ModifiedAt >= lastSyncTime || e.CreatedAt >= lastSyncTime)
                : (e => true);

            // Get exercises
            var exercises = await _unitOfWork.Exercises.GetAsync(condition);

            // Convert to sync items
            foreach (var exercise in exercises)
            {
                var item = new SyncItemDto
                {
                    Id = exercise.Id.ToString(),
                    TableName = "Exercises",
                    SyncAction = SyncAction.Update,
                    Timestamp = exercise.ModifiedAt ?? exercise.CreatedAt,
                    RowVersion = exercise.RowVersionString,
                    Data = System.Text.Json.JsonSerializer.Serialize(MapEntityToDto(exercise))
                };

                result.Add(item);
            }

            return result;
        }

        #endregion

        #region Apply Changes Methods

        private async Task<(int created, int updated, int deleted, int errors)> ApplyUserChangesAsync(List<SyncItemDto> changes, int currentUserId)
        {
            int created = 0;
            int updated = 0;
            int deleted = 0;
            int errors = 0;

            foreach (var change in changes)
            {
                try
                {
                    switch (change.SyncAction)
                    {
                        case SyncAction.Create:
                        case SyncAction.Update:
                            var dto = System.Text.Json.JsonSerializer.Deserialize<UserDto>(change.Data!);

                            if (dto == null)
                            {
                                errors++;
                                continue;
                            }

                            var existingUser = await _unitOfWork.Users.GetByIdAsync(int.Parse(change.Id));

                            if (existingUser == null)
                            {
                                // Create new user
                                var newUser = new User
                                {
                                    Username = dto.Username,
                                    Email = dto.Email,
                                    FirstName = dto.FirstName,
                                    LastName = dto.LastName,
                                    IsActive = dto.IsActive,
                                    IsEmailVerified = dto.IsEmailVerified,
                                    RoleIds = dto.RoleIds,
                                    RoleNames = dto.RoleNames
                                };

                                await _unitOfWork.Users.AddAsync(newUser, currentUserId);
                                created++;
                            }
                            else
                            {
                                // Update existing user
                                existingUser.Username = dto.Username;
                                existingUser.Email = dto.Email;
                                existingUser.FirstName = dto.FirstName;
                                existingUser.LastName = dto.LastName;
                                existingUser.IsActive = dto.IsActive;
                                existingUser.IsEmailVerified = dto.IsEmailVerified;
                                existingUser.RoleIds = dto.RoleIds;
                                existingUser.RoleNames = dto.RoleNames;

                                if (!string.IsNullOrEmpty(change.RowVersion))
                                {
                                    existingUser.RowVersionString = change.RowVersion;
                                }

                                await _unitOfWork.Users.UpdateAsync(existingUser, currentUserId);
                                updated++;
                            }
                            break;

                        case SyncAction.Delete:
                            await _unitOfWork.Users.DeleteAsync(int.Parse(change.Id), currentUserId);
                            deleted++;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error applying user change {Id}", change.Id);
                    errors++;
                }
            }

            return (created, updated, deleted, errors);
        }

        private async Task<(int created, int updated, int deleted, int errors)> ApplyRoleChangesAsync(List<SyncItemDto> changes, int currentUserId)
        {
            int created = 0;
            int updated = 0;
            int deleted = 0;
            int errors = 0;

            foreach (var change in changes)
            {
                try
                {
                    switch (change.SyncAction)
                    {
                        case SyncAction.Create:
                        case SyncAction.Update:
                            var dto = System.Text.Json.JsonSerializer.Deserialize<RoleDto>(change.Data!);

                            if (dto == null)
                            {
                                errors++;
                                continue;
                            }

                            var existingRole = await _unitOfWork.Roles.GetByIdAsync(int.Parse(change.Id));

                            if (existingRole == null)
                            {
                                // Create new role
                                var newRole = new Role
                                {
                                    Name = dto.Name,
                                    Description = dto.Description,
                                    IsSystemRole = dto.IsSystemRole,
                                    Permissions = dto.Permissions
                                };

                                await _unitOfWork.Roles.AddAsync(newRole, currentUserId);
                                created++;
                            }
                            else
                            {
                                // Update existing role
                                existingRole.Name = dto.Name;
                                existingRole.Description = dto.Description;
                                existingRole.IsSystemRole = dto.IsSystemRole;
                                existingRole.Permissions = dto.Permissions;

                                if (!string.IsNullOrEmpty(change.RowVersion))
                                {
                                    existingRole.RowVersionString = change.RowVersion;
                                }

                                await _unitOfWork.Roles.UpdateAsync(existingRole, currentUserId);
                                updated++;
                            }
                            break;

                        case SyncAction.Delete:
                            await _unitOfWork.Roles.DeleteAsync(int.Parse(change.Id), currentUserId);
                            deleted++;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error applying role change {Id}", change.Id);
                    errors++;
                }
            }

            return (created, updated, deleted, errors);
        }

        private async Task<(int created, int updated, int deleted, int errors)> ApplyCategoryChangesAsync(List<SyncItemDto> changes, int currentUserId)
        {
            // Implementation similar to the methods above
            return (0, 0, 0, 0);
        }

        private async Task<(int created, int updated, int deleted, int errors)> ApplyVocabularyChangesAsync(List<SyncItemDto> changes, int currentUserId)
        {
            // Implementation similar to the methods above
            return (0, 0, 0, 0);
        }

        private async Task<(int created, int updated, int deleted, int errors)> ApplyLessonChangesAsync(List<SyncItemDto> changes, int currentUserId)
        {
            // Implementation similar to the methods above
            return (0, 0, 0, 0);
        }

        private async Task<(int created, int updated, int deleted, int errors)> ApplyCourseChangesAsync(List<SyncItemDto> changes, int currentUserId)
        {
            // Implementation similar to the methods above
            return (0, 0, 0, 0);
        }

        private async Task<(int created, int updated, int deleted, int errors)> ApplyExerciseChangesAsync(List<SyncItemDto> changes, int currentUserId)
        {
            // Implementation similar to the methods above
            return (0, 0, 0, 0);
        }

        #endregion

        #region Helper Methods

        private int GetCurrentUserId()
        {
            // Implementation depends on your authentication system
            // For simplicity, we'll return a fixed ID
            return 1;
        }

        private object MapEntityToDto<T>(T entity) where T : BaseEntity
        {
            // Map entity to DTO based on type
            if (entity is User user)
            {
                return new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    IsEmailVerified = user.IsEmailVerified,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt,
                    RoleIds = user.RoleIds,
                    RoleNames = user.RoleNames,
                    RowVersionString = user.RowVersionString
                };
            }
            else if (entity is Role role)
            {
                return new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    IsSystemRole = role.IsSystemRole,
                    Permissions = role.Permissions,
                    UsersCount = role.UsersCount
                };
            }

            // Add mappings for other entity types

            // Default fallback
            return entity;
        }

        #endregion
    }

    /// <summary>
    /// Sync item DTO
    /// </summary>
    public class SyncItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public SyncAction SyncAction { get; set; }
        public string? Data { get; set; }
        public DateTime Timestamp { get; set; }
        public string? RowVersion { get; set; }
    }

    /// <summary>
    /// Sync action enum
    /// </summary>
    public enum SyncAction
    {
        Create,
        Update,
        Delete
    }

    /// <summary>
    /// Sync timestamp DTO
    /// </summary>
    public class SyncTimestampDto
    {
        public DateTime Timestamp { get; set; }
    }
}