namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO cho API key của người dùng
    /// </summary>
    public class UserApiKeyDto
    {
        /// <summary>
        /// API key
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Thời gian hết hạn
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Còn hiệu lực
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Danh sách quyền
        /// </summary>
        public List<string> Permissions { get; set; } = new List<string>();
    }
}
