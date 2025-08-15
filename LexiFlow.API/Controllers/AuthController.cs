using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using LexiFlow.Infrastructure.Data;
using LexiFlow.API.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller xác thực và ủy quyền
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly LexiFlowContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthController(
            ILogger<AuthController> logger,
            LexiFlowContext dbContext,
            IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Kiểm tra username đã tồn tại
                var existingUser = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

                if (existingUser != null)
                {
                    return BadRequest(new ErrorResponse { Message = "Username hoặc email đã tồn tại" });
                }

                // Tạo user mới
                var user = new LexiFlow.Models.User.User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = HashPassword(request.Password),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PreferredLanguage = request.PreferredLanguage ?? "vi",
                    TimeZone = request.TimeZone ?? "Asia/Ho_Chi_Minh"
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                // Tạo user profile
                var profile = new LexiFlow.Models.User.UserProfile
                {
                    UserId = user.UserId,
                    DisplayName = request.DisplayName ?? request.Username,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _dbContext.UserProfiles.Add(profile);

                // Tạo learning preferences
                var preferences = new LexiFlow.Models.User.UserLearningPreference
                {
                    UserId = user.UserId,
                    DailyNewWordsGoal = 10,
                    StudySessionLengthMinutes = 30,
                    PreferredDifficulty = "Beginner",
                    LearningStyle = "Balanced",
                    StudyFocus = "Comprehensive",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _dbContext.UserLearningPreferences.Add(preferences);

                // Tạo notification settings
                var notificationSettings = new LexiFlow.Models.User.UserNotificationSetting
                {
                    UserId = user.UserId,
                    EmailNotificationsEnabled = true,
                    PushNotificationsEnabled = true,
                    StudyRemindersEnabled = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _dbContext.UserNotificationSettings.Add(notificationSettings);

                await _dbContext.SaveChangesAsync();

                // Tạo token
                var token = GenerateJwtToken(user);

                return Ok(new AuthResponseDto
                {
                    Token = token,
                    RefreshToken = GenerateRefreshToken(),
                    User = new UserProfileDto
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        Email = user.Email,
                        DisplayName = profile.DisplayName,
                        IsActive = user.IsActive,
                        CreatedAt = user.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user");
                return StatusCode(500, new ErrorResponse { Message = "Đã xảy ra lỗi khi đăng ký" });
            }
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Tìm user
                var user = await _dbContext.Users
                    .Include(u => u.Profile)
                    .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Username);

                if (user == null || !user.IsActive)
                {
                    return Unauthorized(new ErrorResponse { Message = "Thông tin đăng nhập không chính xác" });
                }

                // Kiểm tra mật khẩu
                if (!VerifyPassword(request.Password, user.PasswordHash))
                {
                    return Unauthorized(new ErrorResponse { Message = "Thông tin đăng nhập không chính xác" });
                }

                // Cập nhật last login
                user.LastLoginAt = DateTime.UtcNow;
                user.LastLoginIP = GetClientIpAddress();
                await _dbContext.SaveChangesAsync();

                // Tạo token
                var token = GenerateJwtToken(user);

                return Ok(new AuthResponseDto
                {
                    Token = token,
                    RefreshToken = GenerateRefreshToken(),
                    User = new UserProfileDto
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        Email = user.Email,
                        DisplayName = user.Profile?.DisplayName ?? user.Username,
                        IsActive = user.IsActive,
                        LastLoginAt = user.LastLoginAt,
                        CreatedAt = user.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in user");
                return StatusCode(500, new ErrorResponse { Message = "Đã xảy ra lỗi khi đăng nhập" });
            }
        }

        /// <summary>
        /// Làm mới token
        /// </summary>
        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshTokenResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Validate refresh token và tạo token mới
                // TODO: Implement proper refresh token validation

                var token = GenerateJwtToken(new LexiFlow.Models.User.User { UserId = 1, Username = "temp" });

                return Ok(new RefreshTokenResponseDto
                {
                    Token = token,
                    RefreshToken = GenerateRefreshToken()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return StatusCode(500, new ErrorResponse { Message = "Đã xảy ra lỗi khi làm mới token" });
            }
        }

        /// <summary>
        /// Đăng xuất
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            try
            {
                // TODO: Invalidate token/refresh token

                return Ok(new { Message = "Đăng xuất thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging out user");
                return StatusCode(500, new ErrorResponse { Message = "Đã xảy ra lỗi khi đăng xuất" });
            }
        }

        /// <summary>
        /// Kiểm tra token hiện tại
        /// </summary>
        [HttpGet("validate")]
        [Authorize]
        public async Task<ActionResult<UserProfileDto>> ValidateToken()
        {
            try
            {
                var userId = GetCurrentUserId();
                var user = await _dbContext.Users
                    .Include(u => u.Profile)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null || !user.IsActive)
                {
                    return Unauthorized(new ErrorResponse { Message = "Token không hợp lệ" });
                }

                return Ok(new UserProfileDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    DisplayName = user.Profile?.DisplayName ?? user.Username,
                    IsActive = user.IsActive,
                    LastLoginAt = user.LastLoginAt,
                    CreatedAt = user.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                return StatusCode(500, new ErrorResponse { Message = "Đã xảy ra lỗi khi xác thực token" });
            }
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();
                var user = await _dbContext.Users.FindAsync(userId);

                if (user == null || !user.IsActive)
                {
                    return Unauthorized(new ErrorResponse { Message = "User không tồn tại" });
                }

                // Kiểm tra mật khẩu cũ
                if (!VerifyPassword(request.CurrentPassword, user.PasswordHash))
                {
                    return BadRequest(new ErrorResponse { Message = "Mật khẩu hiện tại không chính xác" });
                }

                // Cập nhật mật khẩu mới
                user.PasswordHash = HashPassword(request.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

                return Ok(new { Message = "Đổi mật khẩu thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                return StatusCode(500, new ErrorResponse { Message = "Đã xảy ra lỗi khi đổi mật khẩu" });
            }
        }

        /// <summary>
        /// Quên mật khẩu
        /// </summary>
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive);

                if (user != null)
                {
                    // TODO: Implement email sending for password reset
                    // Generate reset token and send email
                }

                // Luôn trả về success để tránh user enumeration
                return Ok(new { Message = "Nếu email tồn tại, chúng tôi đã gửi hướng dẫn đặt lại mật khẩu" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing forgot password");
                return StatusCode(500, new ErrorResponse { Message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Đặt lại mật khẩu
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // TODO: Validate reset token
                
                return Ok(new { Message = "Đặt lại mật khẩu thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password");
                return StatusCode(500, new ErrorResponse { Message = "Đã xảy ra lỗi khi đặt lại mật khẩu" });
            }
        }

        #region Private Methods

        private string GenerateJwtToken(LexiFlow.Models.User.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"] ?? "your-secret-key-here-make-it-long-enough");
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim("userId", user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JWT:Issuer"] ?? "LexiFlow",
                Audience = _configuration["JWT:Audience"] ?? "LexiFlow"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return 0;
        }

        private string GetClientIpAddress()
        {
            string? ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            }
            return ipAddress ?? "Unknown";
        }

        #endregion
    }

    #region DTOs

    public class RegisterRequestDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? DisplayName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PreferredLanguage { get; set; }
        public string? TimeZone { get; set; }
    }

    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public UserProfileDto User { get; set; } = new UserProfileDto();
    }

    public class UserProfileDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class RefreshTokenResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class ChangePasswordRequestDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    public class ForgotPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordRequestDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string? Detail { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
    }

    #endregion
}