using LexiFlow.AdminDashboard.Services;
using LexiFlow.Models.User;
using LexiFlow.Models.User.UserRelations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LexiFlow.Infrastructure.Data;
using System.Text.Json;
using System.IO;
using System.Text;

namespace LexiFlow.AdminDashboard.Services.Implementation
{
    /// <summary>
    /// User Management Service Implementation
    /// </summary>
    public class UserManagementService : IUserManagementService
    {
        private readonly LexiFlowContext _context;
        private readonly ILogger<UserManagementService> _logger;
        private readonly IApiClient _apiClient;

        public UserManagementService(
            LexiFlowContext context,
            ILogger<UserManagementService> logger,
            IApiClient apiClient)
        {
            _context = context;
            _logger = logger;
            _apiClient = apiClient;
        }

        #region User CRUD Operations

        public async Task<List<User>> GetUsersAsync(int page = 1, int pageSize = 50, string searchTerm = "")
        {
            try
            {
                var query = _context.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(u => 
                        u.Username.ToLower().Contains(searchTerm) ||
                        u.Email.ToLower().Contains(searchTerm));
                }

                return await query
                    .OrderBy(u => u.Username)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID {UserId}", userId);
                throw;
            }
        }

        public async Task<User> CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                // Check if username or email already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

                if (existingUser != null)
                {
                    throw new InvalidOperationException("Username or email already exists");
                }

                // Create user
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    PreferredLanguage = request.PreferredLanguage,
                    TimeZone = request.TimeZone,
                    IsActive = request.IsActive,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Create user profile if we have profile data
                if (!string.IsNullOrEmpty(request.FirstName) || !string.IsNullOrEmpty(request.LastName))
                {
                    var userProfile = new UserProfile
                    {
                        UserId = user.UserId,
                        FirstName = request.FirstName ?? "",
                        LastName = request.LastName ?? "",
                        PhoneNumber = request.PhoneNumber,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.UserProfiles.Add(userProfile);
                }

                // Assign roles
                if (request.RoleIds?.Any() == true)
                {
                    var userRoles = request.RoleIds.Select(roleId => new UserRole
                    {
                        UserId = user.UserId,
                        RoleId = roleId
                    }).ToList();

                    _context.UserRoles.AddRange(userRoles);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("User created successfully: {Username}", request.Username);

                // Return user with includes
                return await GetUserByIdAsync(user.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user: {Username}", request.Username);
                throw;
            }
        }

        public async Task<User> UpdateUserAsync(int userId, UpdateUserRequest request)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                var user = await _context.Users
                    .Include(u => u.UserRoles)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }

                // Check if username or email conflicts with other users
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserId != userId && 
                        (u.Username == request.Username || u.Email == request.Email));

                if (existingUser != null)
                {
                    throw new InvalidOperationException("Username or email already exists");
                }

                // Update user
                user.Username = request.Username;
                user.Email = request.Email;
                user.PreferredLanguage = request.PreferredLanguage;
                user.TimeZone = request.TimeZone;
                user.IsActive = request.IsActive;
                user.UpdatedAt = DateTime.UtcNow;

                // Update or create user profile
                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserId == userId);
                if (userProfile != null)
                {
                    userProfile.FirstName = request.FirstName ?? "";
                    userProfile.LastName = request.LastName ?? "";
                    userProfile.PhoneNumber = request.PhoneNumber;
                    userProfile.UpdatedAt = DateTime.UtcNow;
                }
                else if (!string.IsNullOrEmpty(request.FirstName) || !string.IsNullOrEmpty(request.LastName))
                {
                    var newProfile = new UserProfile
                    {
                        UserId = userId,
                        FirstName = request.FirstName ?? "",
                        LastName = request.LastName ?? "",
                        PhoneNumber = request.PhoneNumber,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.UserProfiles.Add(newProfile);
                }

                // Update roles
                if (request.RoleIds != null)
                {
                    // Remove existing roles
                    _context.UserRoles.RemoveRange(user.UserRoles);

                    // Add new roles
                    var newUserRoles = request.RoleIds.Select(roleId => new UserRole
                    {
                        UserId = userId,
                        RoleId = roleId
                    }).ToList();

                    _context.UserRoles.AddRange(newUserRoles);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("User updated successfully: {UserId}", userId);

                // Return updated user with includes
                return await GetUserByIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int userId, bool softDelete = true)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return false;
                }

                if (softDelete)
                {
                    user.IsActive = false;
                    user.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    _context.Users.Remove(user);
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("User deleted (soft: {SoftDelete}): {UserId}", softDelete, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> RestoreUserAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return false;
                }

                user.IsActive = true;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("User restored: {UserId}", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restoring user: {UserId}", userId);
                throw;
            }
        }

        #endregion

        #region Role Management

        public async Task<List<Role>> GetRolesAsync()
        {
            try
            {
                return await _context.Roles
                    .Where(r => r.IsActive)
                    .OrderBy(r => r.RoleName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting roles");
                throw;
            }
        }

        public async Task<List<Role>> GetUserRolesAsync(int userId)
        {
            try
            {
                return await _context.UserRoles
                    .Where(ur => ur.UserId == userId)
                    .Include(ur => ur.Role)
                    .Select(ur => ur.Role)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user roles for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> AssignRoleAsync(int userId, int roleId)
        {
            try
            {
                var existingUserRole = await _context.UserRoles
                    .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

                if (existingUserRole != null)
                {
                    return true; // Already assigned
                }

                var userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                };

                _context.UserRoles.Add(userRole);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Role assigned: User {UserId}, Role {RoleId}", userId, roleId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role: User {UserId}, Role {RoleId}", userId, roleId);
                throw;
            }
        }

        public async Task<bool> RemoveRoleAsync(int userId, int roleId)
        {
            try
            {
                var userRole = await _context.UserRoles
                    .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

                if (userRole == null)
                {
                    return false;
                }

                _context.UserRoles.Remove(userRole);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Role removed: User {UserId}, Role {RoleId}", userId, roleId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing role: User {UserId}, Role {RoleId}", userId, roleId);
                throw;
            }
        }

        public async Task<bool> UpdateUserRolesAsync(int userId, List<int> roleIds)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                // Remove existing roles
                var existingRoles = await _context.UserRoles
                    .Where(ur => ur.UserId == userId)
                    .ToListAsync();

                _context.UserRoles.RemoveRange(existingRoles);

                // Add new roles
                var newUserRoles = roleIds.Select(roleId => new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                }).ToList();

                _context.UserRoles.AddRange(newUserRoles);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("User roles updated: User {UserId}, Roles {RoleIds}", userId, string.Join(",", roleIds));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user roles: User {UserId}", userId);
                throw;
            }
        }

        #endregion

        #region Permission Management

        public async Task<List<Permission>> GetPermissionsAsync()
        {
            try
            {
                return await _context.Permissions
                    .Where(p => p.IsActive)
                    .OrderBy(p => p.PermissionName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permissions");
                throw;
            }
        }

        public async Task<List<Permission>> GetUserPermissionsAsync(int userId)
        {
            try
            {
                return await _context.UserRoles
                    .Where(ur => ur.UserId == userId)
                    .SelectMany(ur => ur.Role.RolePermissions)
                    .Select(rp => rp.Permission)
                    .Distinct()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user permissions for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<List<Permission>> GetRolePermissionsAsync(int roleId)
        {
            try
            {
                return await _context.RolePermissions
                    .Where(rp => rp.RoleId == roleId)
                    .Include(rp => rp.Permission)
                    .Select(rp => rp.Permission)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting role permissions for role: {RoleId}", roleId);
                throw;
            }
        }

        #endregion

        #region Search and Filter

        public async Task<List<User>> SearchUsersAsync(UserSearchFilter filter)
        {
            try
            {
                var query = _context.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    var searchTerm = filter.SearchTerm.ToLower();
                    query = query.Where(u =>
                        u.Username.ToLower().Contains(searchTerm) ||
                        u.Email.ToLower().Contains(searchTerm));
                }

                if (filter.RoleIds?.Any() == true)
                {
                    query = query.Where(u => u.UserRoles.Any(ur => filter.RoleIds.Contains(ur.RoleId)));
                }

                if (filter.IsActive.HasValue)
                {
                    query = query.Where(u => u.IsActive == filter.IsActive.Value);
                }

                if (filter.CreatedFrom.HasValue)
                {
                    query = query.Where(u => u.CreatedAt >= filter.CreatedFrom.Value);
                }

                if (filter.CreatedTo.HasValue)
                {
                    query = query.Where(u => u.CreatedAt <= filter.CreatedTo.Value);
                }

                // Apply sorting
                query = filter.SortBy?.ToLower() switch
                {
                    "email" => filter.SortDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                    "createdat" => filter.SortDescending ? query.OrderByDescending(u => u.CreatedAt) : query.OrderBy(u => u.CreatedAt),
                    _ => filter.SortDescending ? query.OrderByDescending(u => u.Username) : query.OrderBy(u => u.Username)
                };

                // Apply pagination
                return await query
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching users");
                throw;
            }
        }

        public async Task<int> GetUserCountAsync(string searchTerm = "")
        {
            try
            {
                var query = _context.Users.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(u =>
                        u.Username.ToLower().Contains(searchTerm) ||
                        u.Email.ToLower().Contains(searchTerm));
                }

                return await query.CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user count");
                throw;
            }
        }

        #endregion

        #region Bulk Operations

        public async Task<List<User>> ImportUsersAsync(List<CreateUserRequest> users)
        {
            try
            {
                var createdUsers = new List<User>();

                foreach (var userRequest in users)
                {
                    try
                    {
                        var user = await CreateUserAsync(userRequest);
                        createdUsers.Add(user);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to import user: {Username}", userRequest.Username);
                        // Continue with other users
                    }
                }

                _logger.LogInformation("Bulk import completed: {CreatedCount}/{TotalCount} users", 
                    createdUsers.Count, users.Count);

                return createdUsers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during bulk user import");
                throw;
            }
        }

        public async Task<byte[]> ExportUsersAsync(List<int> userIds = null)
        {
            try
            {
                var query = _context.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .AsQueryable();

                if (userIds?.Any() == true)
                {
                    query = query.Where(u => userIds.Contains(u.UserId));
                }

                var users = await query.ToListAsync();

                // Create CSV content
                var csv = new StringBuilder();
                
                // Headers
                csv.AppendLine("Username,Email,Language,TimeZone,Roles,IsActive,CreatedDate");

                // Data
                foreach (var user in users)
                {
                    var roles = string.Join(";", user.UserRoles.Select(ur => ur.Role.RoleName));
                    csv.AppendLine($"{user.Username},{user.Email},{user.PreferredLanguage},{user.TimeZone},{roles},{(user.IsActive ? "Yes" : "No")},{user.CreatedAt:yyyy-MM-dd}");
                }

                return Encoding.UTF8.GetBytes(csv.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting users");
                throw;
            }
        }

        #endregion

        #region User Status Management

        public async Task<bool> ActivateUserAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return false;
                }

                user.IsActive = true;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("User activated: {UserId}", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating user: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return false;
                }

                user.IsActive = false;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("User deactivated: {UserId}", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating user: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> ResetPasswordAsync(int userId, string newPassword = null)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return false;
                }

                // Generate random password if not provided
                if (string.IsNullOrEmpty(newPassword))
                {
                    newPassword = GenerateRandomPassword();
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Password reset for user: {UserId}", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for user: {UserId}", userId);
                throw;
            }
        }

        #endregion

        #region Department Management

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            try
            {
                // This is a simplified implementation - you may need to create Department model
                return new List<Department>
                {
                    new Department { Id = 1, Name = "IT Department", Description = "Information Technology", IsActive = true },
                    new Department { Id = 2, Name = "HR Department", Description = "Human Resources", IsActive = true },
                    new Department { Id = 3, Name = "Finance Department", Description = "Finance and Accounting", IsActive = true },
                    new Department { Id = 4, Name = "Marketing Department", Description = "Marketing and Sales", IsActive = true }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting departments");
                throw;
            }
        }

        public async Task<bool> AssignUserToDepartmentAsync(int userId, int departmentId)
        {
            try
            {
                // This would require Department model and relationship - placeholder implementation
                _logger.LogInformation("User {UserId} assigned to department {DepartmentId}", userId, departmentId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning user to department: User {UserId}, Department {DepartmentId}", userId, departmentId);
                throw;
            }
        }

        #endregion

        #region Analytics

        public async Task<UserStatistics> GetUserStatisticsAsync()
        {
            try
            {
                var totalUsers = await _context.Users.CountAsync();
                var activeUsers = await _context.Users.CountAsync(u => u.IsActive);
                var usersCreatedThisMonth = await _context.Users
                    .CountAsync(u => u.CreatedAt >= DateTime.UtcNow.AddDays(-30));

                var usersByRole = await _context.UserRoles
                    .Include(ur => ur.Role)
                    .GroupBy(ur => ur.Role.RoleName)
                    .Select(g => new { Role = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Role, x => x.Count);

                return new UserStatistics
                {
                    TotalUsers = totalUsers,
                    ActiveUsers = activeUsers,
                    InactiveUsers = totalUsers - activeUsers,
                    UsersCreatedThisMonth = usersCreatedThisMonth,
                    UsersActiveToday = 0, // Would require login tracking
                    UsersByRole = usersByRole,
                    UsersByDepartment = new Dictionary<string, int>() // Would require department model
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user statistics");
                throw;
            }
        }

        public async Task<List<UserActivityLog>> GetUserActivityAsync(int userId, int days = 30)
        {
            try
            {
                // This would require Activity Log model - placeholder implementation
                return new List<UserActivityLog>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user activity for user: {UserId}", userId);
                throw;
            }
        }

        #endregion

        #region Helper Methods

        private string GenerateRandomPassword(int length = 12)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion
    }
}