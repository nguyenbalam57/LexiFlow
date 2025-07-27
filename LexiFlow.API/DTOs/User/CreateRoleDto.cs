using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO để tạo mới vai trò
    /// </summary>
    public class CreateRoleDto
    {
        /// <summary>
        /// Tên vai trò
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string RoleName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả vai trò
        /// </summary>
        [StringLength(255)]
        public string? Description { get; set; }

        /// <summary>
        /// Danh sách ID quyền được gán cho vai trò
        /// </summary>
        public List<int> PermissionIds { get; set; } = new List<int>();

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
