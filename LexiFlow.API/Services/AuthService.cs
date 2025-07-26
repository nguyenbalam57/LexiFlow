using LexiFlow.API.Data;
using LexiFlow.Models;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace LexiFlow.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ApplicationDbContext dbContext, ILogger<AuthService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            try
            {
                var user = await _dbContext.Users
                    .Include(u => u.UserRoles)
                    .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

                if (user == null)
                    return null;

                // Verify password
                if (!BC.Verify(password, user.PasswordHash))
                    return null;

                // Update last login timestamp
                user.LastLogin = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating user {Username}", username);
                return null;
            }
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _dbContext.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.UserID == userId && u.IsActive);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(userId);
                if (user == null)
                    return false;

                // Verify current password
                if (!BC.Verify(currentPassword, user.PasswordHash))
                    return false;

                // Update password
                user.PasswordHash = BC.HashPassword(newPassword);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user {UserId}", userId);
                return false;
            }
        }
    }
}