using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Auth
{
    /// <summary>
    /// DTO cho yêu cầu quên mật khẩu
    /// </summary>
    public class ForgotPasswordDto
    {
        /// <summary>
        /// Email người dùng
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
