using LexiFlow.API.DTOs.Auth;
using LexiFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LexiFlow.API.DTOs.User;
using LexiFlow.Models.User;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Xác thực người dùng và quản lý token
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly Infrastructure.Data.LexiFlowContext _dbContext;

        /// <summary>
        /// Khởi tạo AuthController
        /// </summary>
        public AuthController(
            IConfiguration configuration,
            ILogger<AuthController> logger,
            Infrastructure.Data.LexiFlowContext dbContext)
        {
            _configuration = configuration;
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Đăng nhập và lấy token
        /// </summary>
        /// <param name="request">Thông tin đăng nhập</param>
        /// <returns>Token JWT và thông tin người dùng</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Validate request
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new ErrorResponse { Message = "Username and password are required." });
                }

                // Find user
                var user = _dbContext.Users.FirstOrDefault(u => u.Username == request.Username);
                if (user == null)
                {
                    _logger.LogWarning("Login attempt with invalid username: {Username}", request.Username);
                    return Unauthorized(new ErrorResponse { Message = "Invalid username or password." });
                }

                // Verify password
                if (!VerifyPassword(request.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Failed login attempt for user: {Username}", request.Username);
                    return Unauthorized(new ErrorResponse { Message = "Invalid username or password." });
                }

                // Check if user is active
                if (!user.IsActive)
                {
                    _logger.LogWarning("Login attempt for deactivated account: {Username}", request.Username);
                    return Unauthorized(new ErrorResponse { Message = "This account has been deactivated. Please contact an administrator." });
                }

                // Generate JWT token
                var token = GenerateJwtToken(user);

                // Update last login timestamp
                //user.LastLogin = DateTime.UtcNow;
                _dbContext.SaveChanges();

                _logger.LogInformation("User {Username} logged in successfully", request.Username);

                // Return token and user info
                return Ok(new LoginResponse
                {
                    Token = token,
                    User = new UserProfileDto
                    {
                        Id = user.UserId,
                        Username = user.Username,
                        Email = user.Email,
                        IsActive = user.IsActive
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new ErrorResponse { Message = "An error occurred during login. Please try again." });
            }
        }

        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        /// <param name="request">Thông tin đăng ký</param>
        /// <returns>Thông báo thành công</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                // Validate request
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new ErrorResponse { Message = "Username and password are required." });
                }

                // Check if username already exists
                if (_dbContext.Users.Any(u => u.Username == request.Username))
                {
                    return BadRequest(new ErrorResponse { Message = "Username already exists." });
                }

                // Check if email already exists (if provided)
                if (!string.IsNullOrEmpty(request.Email) && _dbContext.Users.Any(u => u.Email == request.Email))
                {
                    return BadRequest(new ErrorResponse { Message = "Email address already in use." });
                }

                // Create new user
                var newUser = new User
                {
                    Username = request.Username,
                    PasswordHash = HashPassword(request.Password),
                    Email = request.Email,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Save to database
                _dbContext.Users.Add(newUser);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("New user registered: {Username}", request.Username);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Registration successful. You can now log in."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return StatusCode(500, new ErrorResponse { Message = "An error occurred during registration. Please try again." });
            }
        }

        /// <summary>
        /// Refresh token hiện tại
        /// </summary>
        /// <returns>Token mới</returns>
        [HttpPost("refresh-token")]
        [Authorize]
        [ProducesResponseType(typeof(RefreshTokenResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                // Get user ID from claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new ErrorResponse { Message = "Invalid token." });
                }

                // Get user
                var user = await _dbContext.Users.FindAsync(userId);
                if (user == null || !user.IsActive)
                {
                    return Unauthorized(new ErrorResponse { Message = "User not found or inactive." });
                }

                // Generate new token
                var newToken = GenerateJwtToken(user);

                return Ok(new RefreshTokenResponse
                {
                    Token = newToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return StatusCode(500, new ErrorResponse { Message = "An error occurred while refreshing the token." });
            }
        }

        /// <summary>
        /// Validate current token
        /// </summary>
        /// <returns>User information</returns>
        [HttpGet("validate")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        public async Task<IActionResult> ValidateToken()
        {
            try
            {
                // Get user ID from claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new ErrorResponse { Message = "Invalid token." });
                }

                // Get user
                var user = await _dbContext.Users.FindAsync(userId);
                if (user == null || !user.IsActive)
                {
                    return Unauthorized(new ErrorResponse { Message = "User not found or inactive." });
                }

                // Return user info
                return Ok(new UserProfileDto
                {
                    Id = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    IsActive = user.IsActive
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                return StatusCode(500, new ErrorResponse { Message = "An error occurred while validating the token." });
            }
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="request">Thông tin đổi mật khẩu</param>
        /// <returns>Thông báo thành công</returns>
        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                // Get user ID from claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new ErrorResponse { Message = "Invalid token." });
                }

                // Get user
                var user = await _dbContext.Users.FindAsync(userId);
                if (user == null || !user.IsActive)
                {
                    return Unauthorized(new ErrorResponse { Message = "User not found or inactive." });
                }

                // Verify current password
                if (!VerifyPassword(request.CurrentPassword, user.PasswordHash))
                {
                    return BadRequest(new ErrorResponse { Message = "Current password is incorrect." });
                }

                // Update password
                user.PasswordHash = HashPassword(request.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Password changed for user ID: {UserId}", userId);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Password changed successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                return StatusCode(500, new ErrorResponse { Message = "An error occurred while changing the password." });
            }
        }

        /// <summary>
        /// Tạo JWT token
        /// </summary>
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add user roles as claims
            if (user.UserRoles != null)
            {
                foreach (var userRole in user.UserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.Role.RoleName));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["ExpiryHours"] ?? "24")),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Hash mật khẩu
        /// </summary>
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Xác thực mật khẩu
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}