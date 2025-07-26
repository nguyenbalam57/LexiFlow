namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO cho nhật ký hoạt động người dùng
    /// </summary>
    public class UserActivityLogDto
    {
        /// <summary>
        /// ID hoạt động
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID người dùng
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Tên người dùng
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Module
        /// </summary>
        public string Module { get; set; } = string.Empty;

        /// <summary>
        /// Hành động
        /// </summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Chi tiết
        /// </summary>
        public string Details { get; set; } = string.Empty;

        /// <summary>
        /// Địa chỉ IP
        /// </summary>
        public string IpAddress { get; set; } = string.Empty;

        /// <summary>
        /// Trình duyệt/thiết bị
        /// </summary>
        public string UserAgent { get; set; } = string.Empty;

        /// <summary>
        /// Thời gian
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }
}
