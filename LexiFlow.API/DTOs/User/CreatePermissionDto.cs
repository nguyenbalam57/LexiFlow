using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO để tạo mới quyền
    /// </summary>
    public class CreatePermissionDto
    {
        /// <summary>
        /// Tên quyền
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string PermissionName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả quyền
        /// </summary>
        [StringLength(255)]
        public string? Description { get; set; }

        /// <summary>
        /// Module của quyền
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Module { get; set; } = string.Empty;
    }
}
