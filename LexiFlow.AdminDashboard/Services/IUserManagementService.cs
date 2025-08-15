using LexiFlow.Models.User;
using LexiFlow.Models.User.UserRelations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LexiFlow.AdminDashboard.Services
{
    /// <summary>
    /// Interface for User Management Service
    /// </summary>
    public interface IUserManagementService
    {
        // User CRUD Operations
        Task<List<User>> GetUsersAsync(int page = 1, int pageSize = 50, string searchTerm = "");
        Task<User> GetUserByIdAsync(int userId);
        Task<User> CreateUserAsync(CreateUserRequest request);
        Task<User> UpdateUserAsync(int userId, UpdateUserRequest request);
        Task<bool> DeleteUserAsync(int userId, bool softDelete = true);
        Task<bool> RestoreUserAsync(int userId);

        // Role Management
        Task<List<Role>> GetRolesAsync();
        Task<List<Role>> GetUserRolesAsync(int userId);
        Task<bool> AssignRoleAsync(int userId, int roleId);
        Task<bool> RemoveRoleAsync(int userId, int roleId);
        Task<bool> UpdateUserRolesAsync(int userId, List<int> roleIds);

        // Permission Management
        Task<List<Permission>> GetPermissionsAsync();
        Task<List<Permission>> GetUserPermissionsAsync(int userId);
        Task<List<Permission>> GetRolePermissionsAsync(int roleId);

        // Search and Filter
        Task<List<User>> SearchUsersAsync(UserSearchFilter filter);
        Task<int> GetUserCountAsync(string searchTerm = "");

        // Bulk Operations
        Task<List<User>> ImportUsersAsync(List<CreateUserRequest> users);
        Task<byte[]> ExportUsersAsync(List<int> userIds = null);

        // User Status Management
        Task<bool> ActivateUserAsync(int userId);
        Task<bool> DeactivateUserAsync(int userId);
        Task<bool> ResetPasswordAsync(int userId, string newPassword = null);

        // Department Management
        Task<List<Department>> GetDepartmentsAsync();
        Task<bool> AssignUserToDepartmentAsync(int userId, int departmentId);

        // Analytics
        Task<UserStatistics> GetUserStatisticsAsync();
        Task<List<UserActivityLog>> GetUserActivityAsync(int userId, int days = 30);
    }

    /// <summary>
    /// Request models for User Management
    /// </summary>
    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string PreferredLanguage { get; set; } = "en";
        public string TimeZone { get; set; } = "UTC";
        public int? DepartmentId { get; set; }
        public List<int> RoleIds { get; set; } = new List<int>();
        public bool IsActive { get; set; } = true;
    }

    public class UpdateUserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string PreferredLanguage { get; set; }
        public string TimeZone { get; set; }
        public int? DepartmentId { get; set; }
        public List<int> RoleIds { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserSearchFilter
    {
        public string SearchTerm { get; set; }
        public List<int> RoleIds { get; set; } = new List<int>();
        public List<int> DepartmentIds { get; set; } = new List<int>();
        public bool? IsActive { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string SortBy { get; set; } = "Username";
        public bool SortDescending { get; set; } = false;
    }

    public class UserStatistics
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int UsersCreatedThisMonth { get; set; }
        public int UsersActiveToday { get; set; }
        public Dictionary<string, int> UsersByRole { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> UsersByDepartment { get; set; } = new Dictionary<string, int>();
    }

    public class UserActivityLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }

    /// <summary>
    /// Department model for user assignment
    /// </summary>
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentDepartmentId { get; set; }
        public bool IsActive { get; set; }
        public int UserCount { get; set; }
    }
}