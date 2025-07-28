namespace LexiFlow.API.DTOs.Auth
{
    /// <summary>
    /// Phản hồi chung của API
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Trạng thái thành công
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Thông báo
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Dữ liệu trả về (tùy chọn)
        /// </summary>
        public object Data { get; set; }
    }
}
