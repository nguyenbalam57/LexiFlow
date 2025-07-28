using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Auth
{
    /// <summary>
    /// Yêu cầu đổi mật khẩu
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// Mật khẩu hiện tại
        /// </summary>
        [Required(ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// Mật khẩu mới
        /// </summary>
        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "New password must be between 6 and 100 characters")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Xác nhận mật khẩu mới
        /// </summary>
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
