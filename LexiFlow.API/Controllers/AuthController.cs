using LexiFlow.API.DTOs.Auth;
using LexiFlow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LexiFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticate user and generate JWT token
        /// </summary>
        /// <param name="loginDto">Login credentials</param>
        /// <returns>JWT token and user information</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var response = await _authService.LoginAsync(loginDto);
                if (!response.Success)
                {
                    return Unauthorized(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {Username}", loginDto.Username);
                return BadRequest(new { Success = false, Message = "Login failed", Error = ex.Message });
            }
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="registerDto">User registration details</param>
        /// <returns>Created user information</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
        {
            try
            {
                var response = await _authService.RegisterAsync(registerDto);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Created($"api/users/{response.Data?.Id}", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration for username {Username}", registerDto.Username);
                return BadRequest(new { Success = false, Message = "Registration failed", Error = ex.Message });
            }
        }

        /// <summary>
        /// Refresh JWT token using refresh token
        /// </summary>
        /// <param name="refreshTokenDto">Refresh token</param>
        /// <returns>New JWT token and user information</returns>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
                if (!response.Success)
                {
                    return Unauthorized(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return BadRequest(new { Success = false, Message = "Token refresh failed", Error = ex.Message });
            }
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="changePasswordDto">Password change details</param>
        /// <returns>Success message</returns>
        [Authorize]
        [HttpPost("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _authService.ChangePasswordAsync(userId, changePasswordDto);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password change for user ID {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { Success = false, Message = "Password change failed", Error = ex.Message });
            }
        }

        /// <summary>
        /// Logout user by revoking refresh token
        /// </summary>
        /// <returns>Success message</returns>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { Success = false, Message = "Invalid user identity" });
                }

                var response = await _authService.RevokeTokenAsync(username);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout for user {Username}", User.FindFirst(ClaimTypes.Name)?.Value);
                return BadRequest(new { Success = false, Message = "Logout failed", Error = ex.Message });
            }
        }
    }
}