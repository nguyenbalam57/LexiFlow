using LexiFlow.API.Models.DTOs;
using LexiFlow.API.Models.Responses;
using LexiFlow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LexiFlow.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _authService.ValidateUserAsync(loginDto.Username, loginDto.Password);

                if (user == null)
                {
                    _logger.LogWarning("Failed login attempt for username: {Username}", loginDto.Username);
                    return Unauthorized(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid username or password"
                    });
                }

                var token = GenerateJwtToken(user);

                _logger.LogInformation("User {Username} logged in successfully", user.Username);

                return Ok(new LoginResponse
                {
                    Success = true,
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(8),
                    User = new UserDto
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FullName = user.FullName,
                        Email = user.Email,
                        Role = user.Role?.Name ?? "User",
                        IsActive = user.IsActive,
                        CreatedAt = user.CreatedAt,
                        LastLoginAt = user.LastLoginAt
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login process");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred during the login process. Please try again."
                });
            }
        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid token"
                    });
                }

                var user = await _authService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return Unauthorized(new ApiResponse
                    {
                        Success = false,
                        Message = "User not found"
                    });
                }

                var token = GenerateJwtToken(user);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Token refreshed successfully",
                    Data = new
                    {
                        token,
                        expiresAt = DateTime.UtcNow.AddHours(8)
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred during token refresh. Please try again."
                });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid token"
                    });
                }

                if (string.IsNullOrEmpty(dto.CurrentPassword) || string.IsNullOrEmpty(dto.NewPassword))
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Current password and new password are required"
                    });
                }

                if (dto.NewPassword.Length < 6)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "New password must be at least 6 characters long"
                    });
                }

                var result = await _authService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
                if (!result)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Current password is incorrect"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Password changed successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user ID: {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while changing password. Please try again."
                });
            }
        }

        private string GenerateJwtToken(Core.Entities.User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? "LexiFlowDefaultSecretKey123!@#");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
            };

            if (!string.IsNullOrEmpty(user.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = jwtSettings["Issuer"] ?? "LexiFlow.API",
                Audience = jwtSettings["Audience"] ?? "LexiFlowClient",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}