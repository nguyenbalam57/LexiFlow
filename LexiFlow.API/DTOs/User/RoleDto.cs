namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO cho vai trò
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// ID vai trò
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên vai trò
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả vai trò
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Có phải vai trò hệ thống
        /// </summary>
        public bool IsSystemRole { get; set; } = false;

        /// <summary>
        /// Danh sách quyền
        /// </summary>
        public List<string> Permissions { get; set; } = new List<string>();

        /// <summary>
        /// Số lượng người dùng có vai trò này
        /// </summary>
        public int UsersCount { get; set; } = 0;

        /// <summary>
        /// Chuỗi phiên bản hàng
        /// </summary>
        public string RowVersionString { get; set; } = string.Empty;
    }
}
