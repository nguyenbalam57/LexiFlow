using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Auth
{
    /// <summary>
    /// Thông tin đăng nhập
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        /// <summary>
        /// Ghi nhớ đăng nhập
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
