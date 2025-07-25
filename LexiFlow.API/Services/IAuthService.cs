﻿
using LexiFlow.API.DTOs.Auth;
using LexiFlow.Models;

namespace LexiFlow.API.Services
{
    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(string username, string password);
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<TokenResponseDto> GenerateTokenAsync(User user, string? deviceId = null);
        Task<TokenResponseDto?> RefreshTokenAsync(string refreshToken);
    }
}
