namespace LexiFlow.API.DTOs.Auth
{
    /// <summary>
    /// Kết quả refresh token
    /// </summary>
    public class RefreshTokenResponse
    {
        /// <summary>
        /// Token JWT mới
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
    }
}
