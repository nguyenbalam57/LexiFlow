namespace LexiFlow.API.DTOs.Auth
{
    /// <summary>
    /// DTO cho trạng thái xác thực
    /// </summary>
    public class AuthStatusDto
    {
        /// <summary>
        /// Đã đăng nhập
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// ID người dùng
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Tên người dùng
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Vai trò người dùng
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// Thời gian hết hạn token
        /// </summary>
        public DateTime? TokenExpiration { get; set; }
    }
}
