namespace LexiFlow.API.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public List<int> RoleIds { get; set; } = new List<int>();
        public List<string> RoleNames { get; set; } = new List<string>();
        public string? Department { get; set; }
        public string? Position { get; set; }
        public string? AvatarUrl { get; set; }
        public string RowVersionString { get; set; } = string.Empty;
    }
}
