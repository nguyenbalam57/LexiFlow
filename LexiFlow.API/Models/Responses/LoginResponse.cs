namespace LexiFlow.API.Models.Responses
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public UserDto User { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
