using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Vocabulary
{
    public class ExampleDto
    {
        [Required]
        [StringLength(500)]
        public string Text { get; set; } = string.Empty;

        [StringLength(500)]
        public string Translation { get; set; } = string.Empty;

        [Range(1, 5)]
        public int DifficultyLevel { get; set; } = 3;
    }
}
