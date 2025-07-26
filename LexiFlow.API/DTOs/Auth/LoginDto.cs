using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Auth
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string? DeviceId { get; set; }
    }

}
