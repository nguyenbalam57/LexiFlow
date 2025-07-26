using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    public class UpdateUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        public string? Department { get; set; }
        public string? Position { get; set; }
        public List<int> RoleIds { get; set; } = new List<int>();
        public bool IsActive { get; set; } = true;
        public string RowVersionString { get; set; } = string.Empty;
    }
}
