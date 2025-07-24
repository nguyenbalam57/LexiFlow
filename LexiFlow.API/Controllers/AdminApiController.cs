using LexiFlow.AdminDashboard;
using LexiFlow.API.Models;
using LexiFlow.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LexiFlow.Models;

namespace LexiFlow.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IUnitOfWork unitOfWork, ILogger<AdminController> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Dashboard

        [HttpGet("dashboard/statistics")]
        public async Task<ActionResult<DashboardStatisticsDto>> GetDashboardStatistics()
        {
            try
            {
                var result = new DashboardStatisticsDto
                {
                    TotalUsers = await _unitOfWork.Users.CountAsync(u => true),
                    ActiveUsers = await _unitOfWork.Users.CountAsync(u => u.IsActive),
                    NewUsersThisMonth = await _unitOfWork.Users.CountAsync(u => u.CreatedAt >= DateTime.UtcNow.AddMonths(-1)),
                    TotalContent = await _unitOfWork.VocabularyItems.CountAsync(v => true),
                    ContentAddedThisMonth = await _unitOfWork.VocabularyItems.CountAsync(v => v.CreatedAt >= DateTime.UtcNow.AddMonths(-1)),

                    // Get user statistics by month for the last 12 months
                    MonthlyUserStats = await GetMonthlyUserStatsAsync(),

                    // Get content distribution by category
                    ContentByCategory = await GetContentByCategoryAsync(),

                    // Get recent user activities
                    RecentUserActivities = await GetRecentUserActivitiesAsync(10)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard statistics");
                return StatusCode(500, new ApiResponse { Success = false, Message = "Error getting dashboard statistics" });
            }
        }

        [HttpGet("dashboard/user-stats")]
        public async Task<ActionResult<UserManagementStatsDto>> GetUserManagementStats()
        {
            try
            {
                var result = new UserManagementStatsDto
                {
                    TotalUsers = await _unitOfWork.Users.CountAsync(u => true),
                    ActiveUsers = await _unitOfWork.Users.CountAsync(u => u.IsActive),
                    InactiveUsers = await _unitOfWork.Users.CountAsync(u => !u.IsActive),
                    AdminUsers = await _unitOfWork.Users.CountAsync(u => u.Role.Name == "Admin"),
                    TeacherUsers = await _unitOfWork.Users.CountAsync(u => u.Role.Name == "Teacher"),
                    StudentUsers = await _unitOfWork.Users.CountAsync(u => u.Role.Name == "Student"),
                    NewUsersToday = await _unitOfWork.Users.CountAsync(u => u.CreatedAt >= DateTime.UtcNow.Date),
                    NewUsersThisWeek = await _unitOfWork.Users.CountAsync(u => u.CreatedAt >= DateTime.UtcNow.AddDays(-7)),
                    NewUsersThisMonth = await _unitOfWork.Users.CountAsync(u => u.CreatedAt >= DateTime.UtcNow.AddMonths(-1))
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user management stats");
                return StatusCode(500, new ApiResponse { Success = false, Message = "Error getting user management stats" });
            }
        }

        #endregion

        #region Users

        [HttpGet("users")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var pagedResult = await _unitOfWork.Users.GetPagedAsync(
                    u => true,
                    page,
                    pageSize,
                    "Username",
                    true);

                var items = pagedResult.Items;
                var totalCount = pagedResult.TotalCount;

                var result = new PaginatedResultDto<UserDto>
                {
                    Items = items.Select(MapUserToDto).ToList(),
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = page
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return StatusCode(500, new ApiResponse { Success = false, Message = "Error getting all users" });
            }
        }

        [HttpGet("users/{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new ApiResponse { Success = false, Message = $"User with ID {id} not found" });
                }

                return Ok(MapUserToDto(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user with ID {UserId}", id);
                return StatusCode(500, new ApiResponse { Success = false, Message = $"Error getting user with ID {id}" });
            }
        }

        [HttpPost("users")]
        public async Task<ActionResult<ApiResponse>> CreateUser(CreateUserDto dto)
        {
            try
            {
                // Check if username is already taken
                if (await _unitOfWork.Users.AnyAsync(u => u.Username == dto.User.Username))
                {
                    return BadRequest(new ApiResponse { Success = false, Message = "Username is already taken" });
                }

                // Check if email is already taken
                if (await _unitOfWork.Users.AnyAsync(u => u.Email == dto.User.Email))
                {
                    return BadRequest(new ApiResponse { Success = false, Message = "Email is already taken" });
                }

                // Get the current user ID
                int currentUserId = GetCurrentUserId();

                // Create the user
                var user = new User
                {
                    Username = dto.User.Username,
                    Email = dto.User.Email,
                    FullName = $"{dto.User.FirstName} {dto.User.LastName}",
                    IsActive = dto.User.IsActive,
                    // Hash the password - implementation depends on your authentication system
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
                };

                await _unitOfWork.Users.AddAsync(user, currentUserId);
                await _unitOfWork.SaveChangesAsync();

                // Log the activity
                await LogActivityAsync(
                    currentUserId,
                    "User",
                    "Create",
                    $"Created user {user.Username} (ID: {user.Id})");

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "User created successfully",
                    Data = MapUserToDto(user)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, new ApiResponse { Success = false, Message = "Error creating user" });
            }
        }

        [HttpPut("users/{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateUser(int id, UserDto dto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new ApiResponse { Success = false, Message = $"User with ID {id} not found" });
                }

                // Check if username is already taken by another user
                if (dto.Username != user.Username &&
                    await _unitOfWork.Users.AnyAsync(u => u.Username == dto.Username && u.Id != id))
                {
                    return BadRequest(new ApiResponse { Success = false, Message = "Username is already taken" });
                }

                // Check if email is already taken by another user
                if (dto.Email != user.Email &&
                    await _unitOfWork.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id))
                {
                    return BadRequest(new ApiResponse { Success = false, Message = "Email is already taken" });
                }

                // Get the current user ID
                int currentUserId = GetCurrentUserId();

                // Update the user
                user.Username = dto.Username;
                user.Email = dto.Email;
                user.FullName = $"{dto.FirstName} {dto.LastName}";
                user.IsActive = dto.IsActive;

                // Set the row version for concurrency check
                if (!string.IsNullOrEmpty(dto.RowVersionString))
                {
                    user.RowVersionString = dto.RowVersionString;
                }

                await _unitOfWork.Users.UpdateAsync(user, currentUserId);
                await _unitOfWork.SaveChangesAsync();

                // Log the activity
                await LogActivityAsync(
                    currentUserId,
                    "User",
                    "Update",
                    $"Updated user {user.Username} (ID: {user.Id})");

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "User updated successfully",
                    Data = MapUserToDto(user)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID {UserId}", id);
                return StatusCode(500, new ApiResponse { Success = false, Message = $"Error updating user with ID {id}" });
            }
        }

        [HttpDelete("users/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteUser(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new ApiResponse { Success = false, Message = $"User with ID {id} not found" });
                }

                // Get the current user ID
                int currentUserId = GetCurrentUserId();

                // Prevent deleting yourself
                if (id == currentUserId)
                {
                    return BadRequest(new ApiResponse { Success = false, Message = "You cannot delete your own account" });
                }

                // Delete the user (soft delete)
                await _unitOfWork.Users.DeleteAsync(id, currentUserId);
                await _unitOfWork.SaveChangesAsync();

                // Log the activity
                await LogActivityAsync(
                    currentUserId,
                    "User",
                    "Delete",
                    $"Deleted user {user.Username} (ID: {user.Id})");

                return Ok(new ApiResponse { Success = true, Message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
                return StatusCode(500, new ApiResponse { Success = false, Message = $"Error deleting user with ID {id}" });
            }
        }

        [HttpPost("users/{id}/deactivate")]
        public async Task<ActionResult<ApiResponse>> DeactivateUser(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new ApiResponse { Success = false, Message = $"User with ID {id} not found" });
                }

                // Get the current user ID
                int currentUserId = GetCurrentUserId();

                // Prevent deactivating yourself
                if (id == currentUserId)
                {
                    return BadRequest(new ApiResponse { Success = false, Message = "You cannot deactivate your own account" });
                }

                // Deactivate the user
                user.IsActive = false;
                await _unitOfWork.Users.UpdateAsync(user, currentUserId);
                await _unitOfWork.SaveChangesAsync();

                // Log the activity
                await LogActivityAsync(
                    currentUserId,
                    "User",
                    "Deactivate",
                    $"Deactivated user {user.Username} (ID: {user.Id})");

                return Ok(new ApiResponse { Success = true, Message = "User deactivated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating user with ID {UserId}", id);
                return StatusCode(500, new ApiResponse { Success = false, Message = $"Error deactivating user with ID {id}" });
            }
        }

        [HttpPost("users/{id}/activate")]
        public async Task<ActionResult<ApiResponse>> ActivateUser(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new ApiResponse { Success = false, Message = $"User with ID {id} not found" });
                }

                // Get the current user ID
                int currentUserId = GetCurrentUserId();

                // Activate the user
                user.IsActive = true;
                await _unitOfWork.Users.UpdateAsync(user, currentUserId);
                await _unitOfWork.SaveChangesAsync();

                // Log the activity
                await LogActivityAsync(
                    currentUserId,
                    "User",
                    "Activate",
                    $"Activated user {user.Username} (ID: {user.Id})");

                return Ok(new ApiResponse { Success = true, Message = "User activated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating user with ID {UserId}", id);
                return StatusCode(500, new ApiResponse { Success = false, Message = $"Error activating user with ID {id}" });
            }
        }

        [HttpPost("users/{id}/reset-password")]
        public async Task<ActionResult<ApiResponse>> ResetUserPassword(int id, ResetPasswordDto dto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new ApiResponse { Success = false, Message = $"User with ID {id} not found" });
                }

                // Get the current user ID
                int currentUserId = GetCurrentUserId();

                // Reset the password
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
                await _unitOfWork.Users.UpdateAsync(user, currentUserId);
                await _unitOfWork.SaveChangesAsync();

                // Log the activity
                await LogActivityAsync(
                    currentUserId,
                    "User",
                    "ResetPassword",
                    $"Reset password for user {user.Username} (ID: {user.Id})");

                return Ok(new ApiResponse { Success = true, Message = "Password reset successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for user with ID {UserId}", id);
                return StatusCode(500, new ApiResponse { Success = false, Message = $"Error resetting password for user with ID {id}" });
            }
        }

        [HttpPost("users/assign-role")]
        public async Task<ActionResult<ApiResponse>> AssignUserToRole(AssignRoleDto dto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);

                if (user == null)
                {
                    return NotFound(new ApiResponse { Success = false, Message = $"User with ID {dto.UserId} not found" });
                }

                var role = await _unitOfWork.Roles.GetByIdAsync(dto.RoleId);

                if (role == null)
                {
                    return NotFound(new ApiResponse { Success = false, Message = $"Role with ID {dto.RoleId} not found" });
                }

                // Get the current user ID
                int currentUserId = GetCurrentUserId();

                // Add role to user
                user.RoleId = dto.RoleId;
                user.Role = role;

                await _unitOfWork.Users.UpdateAsync(user, currentUserId);
                await _unitOfWork.SaveChangesAsync();

                // Log the activity
                await LogActivityAsync(
                    currentUserId,
                    "User",
                    "AssignRole",
                    $"Assigned role {role.Name} to user {user.Username} (ID: {user.Id})");

                return Ok(new ApiResponse { Success = true, Message = $"Role {role.Name} assigned to user {user.Username}" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role {RoleId} to user {UserId}", dto.RoleId, dto.UserId);
                return StatusCode(500, new ApiResponse { Success = false, Message = "Error assigning role to user" });
            }
        }

        #endregion

        #region Roles

        [HttpGet("roles")]
        public async Task<ActionResult<List<RoleDto>>> GetAllRoles()
        {
            try
            {
                var roles = await _unitOfWork.Roles.GetAllAsync();
                var roleDtos = roles.Select(MapRoleToDto).ToList();

                return Ok(roleDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all roles");
                return StatusCode(500, new ApiResponse { Success = false, Message = "Error getting all roles" });
            }
        }

        #endregion

        #region Activities

        [HttpGet("activities")]
        public async Task<ActionResult<List<UserActivityDto>>> GetRecentUserActivities([FromQuery] int count = 50)
        {
            try
            {
                var activities = await _unitOfWork.UserActivities.GetAsync(
                    a => true,
                    a => a.OrderByDescending(x => x.Timestamp),
                    count);

                var activityDtos = activities.Select(MapActivityToDto).ToList();

                return Ok(activityDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent user activities");
                return StatusCode(500, new ApiResponse { Success = false, Message = "Error getting recent user activities" });
            }
        }

        #endregion

        #region Settings

        [HttpGet("settings")]
        public async Task<ActionResult<SystemSettingsDto>> GetSystemSettings()
        {
            try
            {
                // Implementation depends on how you store system settings
                // For simplicity, we'll return a default settings object
                var settings = new SystemSettingsDto
                {
                    ApplicationName = "LexiFlow",
                    CompanyName = "LexiFlow Inc.",
                    SupportEmail = "support@lexiflow.com",
                    DefaultLanguage = "en-US",
                    DefaultTimeZone = "UTC",
                    EnableRegistration = true,
                    RequireEmailVerification = true,
                    MinimumPasswordLength = 8,
                    RequirePasswordComplexity = true,
                    SessionTimeoutMinutes = 30
                };

                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting system settings");
                return StatusCode(500, new ApiResponse { Success = false, Message = "Error getting system settings" });
            }
        }

        [HttpPut("settings")]
        public async Task<ActionResult<ApiResponse>> UpdateSystemSettings(SystemSettingsDto dto)
        {
            try
            {
                // Implementation depends on how you store system settings
                // For simplicity, we'll just log the update

                // Get the current user ID
                int currentUserId = GetCurrentUserId();

                // Log the activity
                await LogActivityAsync(
                    currentUserId,
                    "Settings",
                    "Update",
                    "Updated system settings");

                return Ok(new ApiResponse { Success = true, Message = "System settings updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating system settings");
                return StatusCode(500, new ApiResponse { Success = false, Message = "Error updating system settings" });
            }
        }

        #endregion

        #region Helper Methods

        private int GetCurrentUserId()
        {
            // Implementation depends on your authentication system
            // For simplicity, we'll return a fixed ID
            return 1;
        }

        private async Task LogActivityAsync(int userId, string module, string action, string details)
        {
            try
            {
                var activity = new UserActivity
                {
                    UserId = userId,
                    Module = module,
                    Action = action,
                    Timestamp = DateTime.UtcNow,
                    Details = details,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                    UserAgent = HttpContext.Request.Headers["User-Agent"].ToString()
                };

                // Get the username
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user != null)
                {
                    activity.Username = user.Username;
                }

                await _unitOfWork.UserActivities.AddAsync(activity, userId);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging user activity");
            }
        }

        private async Task<List<MonthlyStatsDto>> GetMonthlyUserStatsAsync()
        {
            var result = new List<MonthlyStatsDto>();

            // Get user registrations for the last 12 months
            var startDate = DateTime.UtcNow.AddMonths(-11).Date.AddDays(1 - DateTime.UtcNow.Day);
            var endDate = DateTime.UtcNow;

            var users = await _unitOfWork.Users.GetAsync(u => u.CreatedAt >= startDate && u.CreatedAt <= endDate);

            // Group by month
            var grouped = users
                .GroupBy(u => new { u.CreatedAt.Year, u.CreatedAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToList();

            // Fill in missing months
            for (int i = 0; i < 12; i++)
            {
                var date = startDate.AddMonths(i);
                var item = grouped.FirstOrDefault(g => g.Year == date.Year && g.Month == date.Month);

                result.Add(new MonthlyStatsDto
                {
                    Month = new DateTime(date.Year, date.Month, 1),
                    Value = item?.Count ?? 0,
                    Label = date.ToString("MMM yyyy")
                });
            }

            return result;
        }

        private async Task<Dictionary<string, int>> GetContentByCategoryAsync()
        {
            var result = new Dictionary<string, int>();

            // Get all categories
            var categories = await _unitOfWork.Categories.GetAllAsync();

            // Get vocabulary items for each category
            foreach (var category in categories)
            {
                var count = await _unitOfWork.VocabularyItems.CountAsync(v => v.CategoryId == category.Id);
                result.Add(category.Name, count);
            }

            // Get items with no category
            var uncategorizedCount = await _unitOfWork.VocabularyItems.CountAsync(v => v.CategoryId == null);
            if (uncategorizedCount > 0)
            {
                result.Add("Uncategorized", uncategorizedCount);
            }

            return result;
        }

        private async Task<List<UserActivityDto>> GetRecentUserActivitiesAsync(int count)
        {
            var activities = await _unitOfWork.UserActivities.GetAsync(
                a => true,
                a => a.OrderByDescending(x => x.Timestamp),
                count);

            return activities.Select(MapActivityToDto).ToList();
        }

        #endregion

        #region Mapping Methods

        private UserDto MapUserToDto(User user)
        {
            return new UserDto
            {
                Id = user.UserID,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FullName.Split(' ').FirstOrDefault() ?? "",
                LastName = user.FullName.Split(' ').Skip(1).FirstOrDefault() ?? "",
                IsActive = user.IsActive,
                IsEmailVerified = false, // Not available in your User entity
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                RoleIds = new List<int> { user.RoleId },
                RoleNames = new List<string> { user.Role?.Name ?? "" },
                RowVersionString = Convert.ToBase64String(user.RowVersion ?? Array.Empty<byte>())
            };
        }

        private RoleDto MapRoleToDto(Role role)
        {
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                IsSystemRole = false, // Not available in your Role entity
                Permissions = new List<string>(), // Not available in your Role entity
                UsersCount = role.Users?.Count ?? 0
            };
        }

        private UserActivityDto MapActivityToDto(UserActivity activity)
        {
            return new UserActivityDto
            {
                Id = activity.Id,
                UserId = activity.UserId,
                Username = activity.Username,
                Action = activity.Action,
                Module = activity.Module,
                Timestamp = activity.Timestamp,
                Details = activity.Details,
                IpAddress = activity.IpAddress
            };
        }

        #endregion
    }
}