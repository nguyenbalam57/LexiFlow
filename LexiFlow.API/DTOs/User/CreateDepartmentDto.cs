using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO để tạo mới phòng ban
    /// </summary>
    public class CreateDepartmentDto
    {
        /// <summary>
        /// Tên phòng ban
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string DepartmentName { get; set; } = string.Empty;

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        [StringLength(20)]
        public string DepartmentCode { get; set; } = string.Empty;

        /// <summary>
        /// ID phòng ban cha (nếu có)
        /// </summary>
        public int? ParentDepartmentID { get; set; }

        /// <summary>
        /// ID người quản lý
        /// </summary>
        public int? ManagerUserID { get; set; }

        /// <summary>
        /// Mô tả phòng ban
        /// </summary>
        [StringLength(255)]
        public string? Description { get; set; }

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public bool IsActive { get; set; } = true;
    }

}
