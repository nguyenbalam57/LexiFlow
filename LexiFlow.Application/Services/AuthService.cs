using LexiFlow.Core.Entities;
using LexiFlow.Core.Interfaces;
using Microsoft.Extensions.Logging;
using LexiFlow.Infrastructure.Data;

namespace LexiFlow.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly SqlEntityAdapter _sqlAdapter;
        private readonly ILogger<AuthService>? _logger;

        public AuthService(IUserRepository userRepository, SqlEntityAdapter sqlAdapter, ILogger<AuthService>? logger = null)
        {
            _userRepository = userRepository;
            _sqlAdapter = sqlAdapter;
            _logger = logger;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            try
            {
                // Log login attempt
                _logger?.LogInformation($"Login attempt for user: {username}");

                // Thử đầu tiên với SQL adapter (truy cập trực tiếp DB)
                User? user = null;
                try
                {
                    user = await _sqlAdapter.GetUserByUsernameAsync(username);
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning($"SQL adapter failed: {ex.Message}, falling back to repository");
                    // Nếu không thành công, thử với repository (Entity Framework)
                    user = await _userRepository.GetByUsernameAsync(username);
                }

                if (user == null || !user.IsActive)
                {
                    // Update failed login attempts
                    _logger?.LogWarning($"Failed login attempt for user: {username} - User not found or inactive");
                    return null;
                }

                if (!VerifyPassword(password, user.PasswordHash))
                {
                    _logger?.LogWarning($"Failed login attempt for user: {username} - Invalid password");
                    return null;
                }

                // Update last login time
                try
                {
                    await _sqlAdapter.UpdateUserLastLoginAsync(user.Id);
                }
                catch
                {
                    // Nếu cập nhật qua SQL adapter thất bại, cập nhật qua repository
                    user.LastLoginAt = DateTime.Now;
                    await _userRepository.UpdateAsync(user);
                }

                _logger?.LogInformation($"Successful login for user: {username}");
                return user;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Authentication error for user: {username}");
                throw;
            }
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            try
            {
                // Thử dùng SQL adapter
                return await _sqlAdapter.ValidateUserCredentialsAsync(username, password);
            }
            catch
            {
                // Nếu có lỗi, thử dùng Entity Framework
                var user = await _userRepository.GetByUsernameAsync(username);

                if (user == null || !user.IsActive)
                    return false;

                return VerifyPassword(password, user.PasswordHash);
            }
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            try
            {
                // Thử dùng SQL adapter
                return await _sqlAdapter.UserExistsAsync(username);
            }
            catch
            {
                // Nếu có lỗi, thử dùng Entity Framework
                return await _userRepository.UsernameExistsAsync(username);
            }
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            try
            {
                // Thử dùng SQL adapter
                return await _sqlAdapter.GetUserByUsernameAsync(username);
            }
            catch
            {
                // Nếu có lỗi, thử dùng Entity Framework
                return await _userRepository.GetByUsernameAsync(username);
            }
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            try
            {
                // Thử dùng SQL adapter
                await _sqlAdapter.UpdateUserLastLoginAsync(userId);
            }
            catch
            {
                // Nếu có lỗi, thử dùng Entity Framework
                var user = await _userRepository.GetByIdAsync(userId);

                if (user != null)
                {
                    user.LastLoginAt = DateTime.Now;
                    await _userRepository.UpdateAsync(user);
                }
            }
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            // Using BCrypt verification
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
