using LexiFlow.API.DTOs.User;

namespace LexiFlow.API.DTOs.Common
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserProfileDto? User { get; set; }
    }
}
