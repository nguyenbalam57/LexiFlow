using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Auth
{
    /// <summary>
    /// User DTO for responses
    /// </summary>
    public class UserDto : BaseDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        public new int Id { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; } = null!;

        /// <summary>
        /// Full name
        /// </summary>
        public string FullName { get; set; } = null!;

        /// <summary>
        /// User description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Department name
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Position/title
        /// </summary>
        public string? Position { get; set; }

        /// <summary>
        /// Last login timestamp
        /// </summary>
        public DateTime? LastLogin { get; set; }

        /// <summary>
        /// Whether the user is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Department ID
        /// </summary>
        public int? DepartmentID { get; set; }

        /// <summary>
        /// User roles
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();
    }

    /// <summary>
    /// User registration request DTO
    /// </summary>
    public class RegisterUserDto
    {
        /// <summary>
        /// Username
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = null!;

        /// <summary>
        /// Password
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;

        /// <summary>
        /// Confirm password
        /// </summary>
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = null!;

        /// <summary>
        /// Full name
        /// </summary>
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Email address
        /// </summary>
        [EmailAddress]
        [StringLength(255)]
        public string? Email { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        [StringLength(50)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Department ID
        /// </summary>
        public int? DepartmentID { get; set; }

        /// <summary>
        /// Position/title
        /// </summary>
        [StringLength(100)]
        public string? Position { get; set; }

        /// <summary>
        /// User description
        /// </summary>
        [StringLength(255)]
        public string? Description { get; set; }
    }

    /// <summary>
    /// Login request DTO
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Username
        /// </summary>
        [Required]
        public string Username { get; set; } = null!;

        /// <summary>
        /// Password
        /// </summary>
        [Required]
        public string Password { get; set; } = null!;
    }

    /// <summary>
    /// Login response DTO
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// JWT access token
        /// </summary>
        public string Token { get; set; } = null!;

        /// <summary>
        /// Refresh token
        /// </summary>
        public string RefreshToken { get; set; } = null!;

        /// <summary>
        /// Token expiration timestamp
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// User details
        /// </summary>
        public UserDto User { get; set; } = null!;
    }

    /// <summary>
    /// Update user profile request DTO
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>
        /// Full name
        /// </summary>
        [StringLength(100)]
        public string? FullName { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        [EmailAddress]
        [StringLength(255)]
        public string? Email { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        [StringLength(50)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Position/title
        /// </summary>
        [StringLength(100)]
        public string? Position { get; set; }

        /// <summary>
        /// User description
        /// </summary>
        [StringLength(255)]
        public string? Description { get; set; }
    }

    /// <summary>
    /// Change password request DTO
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// Current password
        /// </summary>
        [Required]
        public string CurrentPassword { get; set; } = null!;

        /// <summary>
        /// New password
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = null!;

        /// <summary>
        /// Confirm new password
        /// </summary>
        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmNewPassword { get; set; } = null!;
    }

    /// <summary>
    /// Reset password request DTO
    /// </summary>
    public class ResetPasswordDto
    {
        /// <summary>
        /// Username
        /// </summary>
        [Required]
        public string Username { get; set; } = null!;

        /// <summary>
        /// Reset token
        /// </summary>
        [Required]
        public string Token { get; set; } = null!;

        /// <summary>
        /// New password
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = null!;

        /// <summary>
        /// Confirm new password
        /// </summary>
        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmNewPassword { get; set; } = null!;
    }

    /// <summary>
    /// Refresh token request DTO
    /// </summary>
    public class RefreshTokenDto
    {
        /// <summary>
        /// Refresh token
        /// </summary>
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}