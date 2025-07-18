using System.Text;

namespace LexiFlow.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ApplicationDbContext context, ILogger<AuthService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> ValidateUserAsync(string username, string password)
        {
            var user = await _context.Users
                .Where(u => u.Username == username && u.IsActive)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                _logger.LogWarning("Login attempt failed: User {Username} not found or inactive", username);
                return null;
            }

            // Verify password hash
            if (!VerifyPassword(password, user.PasswordHash))
            {
                _logger.LogWarning("Login attempt failed: Invalid password for user {Username}", username);
                return null;
            }

            // Update last login time
            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {Username} logged in successfully", username);
            return user;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.UserID == userId && u.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Where(u => u.Username == username && u.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                _logger.LogWarning("Password change failed: User ID {UserId} not found", userId);
                return false;
            }

            // Verify current password
            if (!VerifyPassword(currentPassword, user.PasswordHash))
            {
                _logger.LogWarning("Password change failed: Current password invalid for user ID {UserId}", userId);
                return false;
            }

            // Update password
            user.PasswordHash = HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Password changed successfully for user ID {UserId}", userId);
            return true;
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // In a real application, use a proper password hashing library like BCrypt
            // This is a simplified example
            return HashPassword(password) == storedHash;
        }

        private string HashPassword(string password)
        {
            // In a real application, use a proper password hashing library like BCrypt
            // This is a simplified example using SHA256
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
