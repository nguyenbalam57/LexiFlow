namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO cho phòng ban
    /// </summary>
    public class DepartmentDto
    {
        /// <summary>
        /// ID phòng ban
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public string DepartmentCode { get; set; } = string.Empty;

        /// <summary>
        /// ID phòng ban cha
        /// </summary>
        public int? ParentDepartmentID { get; set; }

        /// <summary>
        /// Tên phòng ban cha
        /// </summary>
        public string? ParentDepartmentName { get; set; }

        /// <summary>
        /// ID người quản lý
        /// </summary>
        public int? ManagerID { get; set; }

        /// <summary>
        /// Tên người quản lý
        /// </summary>
        public string? ManagerName { get; set; }

        /// <summary>
        /// Mô tả phòng ban
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
