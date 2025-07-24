using LexiFlow.API.DTOs;
using LexiFlow.API.DTOs.Auth;

namespace LexiFlow.API.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ApiResponse<UserDto>> RegisterAsync(RegisterUserDto registerDto);
        Task<ApiResponse<LoginResponseDto>> RefreshTokenAsync(string refreshToken);
        Task<ApiResponse<bool>> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task<ApiResponse<bool>> RevokeTokenAsync(string username);
    }
}
