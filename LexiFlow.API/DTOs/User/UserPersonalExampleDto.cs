using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.User
{
    public class UserPersonalExampleDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        [StringLength(500)]
        public string Translation { get; set; }

        public string Context { get; set; }
    }
}
