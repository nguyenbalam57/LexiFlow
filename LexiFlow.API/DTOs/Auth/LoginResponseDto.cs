
using LexiFlow.API.DTOs.User;

namespace LexiFlow.API.DTOs.Auth
{
    /// <summary>
    /// Kết quả đăng nhập
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Token JWT
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Thời hạn token (giây)
        /// </summary>
        public int ExpiresIn { get; set; } = 86400; // 24 hours

        /// <summary>
        /// Loại token
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Thông tin người dùng
        /// </summary>
        public UserProfileDto User { get; set; }
    }
}
