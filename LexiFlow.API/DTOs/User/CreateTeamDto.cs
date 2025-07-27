using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO để tạo mới nhóm làm việc
    /// </summary>
    public class CreateTeamDto
    {
        /// <summary>
        /// Tên nhóm
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string TeamName { get; set; } = string.Empty;

        /// <summary>
        /// ID phòng ban
        /// </summary>
        public int? DepartmentID { get; set; }

        /// <summary>
        /// ID trưởng nhóm
        /// </summary>
        public int? LeaderUserID { get; set; }

        /// <summary>
        /// Mô tả nhóm
        /// </summary>
        [StringLength(255)]
        public string? Description { get; set; }

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
