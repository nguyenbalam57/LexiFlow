namespace LexiFlow.API.DTOs.Auth
{
    /// <summary>
    /// Thông báo lỗi API
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Thông báo lỗi
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Mã lỗi (tùy chọn)
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Chi tiết lỗi (tùy chọn)
        /// </summary>
        public object Details { get; set; }
    }
}
