using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    /// <summary>
    /// DTO cho tạo API key mới
    /// </summary>
    public class GenerateApiKeyDto
    {
        /// <summary>
        /// Tên API key
        /// </summary>
        [Required]
        [StringLength(100)]
        public string KeyName { get; set; } = string.Empty;

        /// <summary>
        /// Thời gian hết hạn (ngày)
        /// </summary>
        [Range(1, 365)]
        public int ExpiryDays { get; set; } = 30;

        /// <summary>
        /// Danh sách quyền
        /// </summary>
        public List<string> Permissions { get; set; } = new List<string>();
    }
}
