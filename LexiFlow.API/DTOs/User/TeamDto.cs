namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO cho nhóm làm việc
    /// </summary>
    public class TeamDto
    {
        /// <summary>
        /// ID nhóm
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên nhóm
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// ID phòng ban
        /// </summary>
        public int? DepartmentID { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// ID trưởng nhóm
        /// </summary>
        public int? LeaderID { get; set; }

        /// <summary>
        /// Tên trưởng nhóm
        /// </summary>
        public string? LeaderName { get; set; }

        /// <summary>
        /// Mô tả nhóm
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Chuỗi phiên bản hàng
        /// </summary>
        public string RowVersionString { get; set; } = string.Empty;
    }
}
