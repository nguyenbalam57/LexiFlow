using LexiFlow.Core.Entities;
using LexiFlow.Core.Interfaces;

namespace LexiFlow.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly SqlEntityAdapter _sqlAdapter;

        public AuthService(IUserRepository userRepository, SqlEntityAdapter sqlAdapter)
        {
            _userRepository = userRepository;
            _sqlAdapter = sqlAdapter;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            try
            {
                // Log login attempt
                _logger.LogInformation($"Login attempt for user: {username}");

                // Existing authentication logic...
                var user = await _sqlAdapter.GetUserByUsernameAsync(username);

                if (user == null || !user.IsActive)
                {
                    // Update failed login attempts
                    UpdateFailedLoginAttempts();
                    return null;
                }

                if (!VerifyPassword(password, user.PasswordHash))
                {
                    UpdateFailedLoginAttempts();
                    return null;
                }

                // Reset failed attempts on successful login
                ResetFailedLoginAttempts();

                // Update last login time
                await _sqlAdapter.UpdateUserLastLoginAsync(user.Id);

                _logger.LogInformation($"Successful login for user: {username}");
                return user;
            }
            catch
            {
                _logger.LogError(ex, $"Authentication error for user: {username}");
                throw;
            }
        }

        private void UpdateFailedLoginAttempts()
        {
            var settings = Properties.Settings.Default;
            settings.LoginAttempts++;
            settings.LastFailedLogin = DateTime.Now;
            settings.Save();
        }

        private void ResetFailedLoginAttempts()
        {
            var settings = Properties.Settings.Default;
            settings.LoginAttempts = 0;
            settings.Save();
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            try
            {
                // Đầu tiên thử dùng SQL adapter
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
                // Đầu tiên thử dùng SQL adapter
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
                // Đầu tiên thử dùng SQL adapter
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
                // Đầu tiên thử dùng SQL adapter
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
            // Using direct BCrypt verification to avoid dependency issues
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
