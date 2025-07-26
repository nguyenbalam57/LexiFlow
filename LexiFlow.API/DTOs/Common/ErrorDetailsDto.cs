namespace LexiFlow.API.DTOs.Common
{
    /// <summary>
    /// DTO cho chi tiết lỗi
    /// </summary>
    public class ErrorDetailsDto
    {
        /// <summary>
        /// Mã lỗi
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Thông báo lỗi
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Thông tin chi tiết
        /// </summary>
        public string? Details { get; set; }

        /// <summary>
        /// Đường dẫn lỗi
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// Dấu thời gian
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
