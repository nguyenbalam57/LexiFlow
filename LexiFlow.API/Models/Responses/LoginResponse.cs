namespace LexiFlow.API.Models.Responses
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserDto? User { get; set; }
    }
}
