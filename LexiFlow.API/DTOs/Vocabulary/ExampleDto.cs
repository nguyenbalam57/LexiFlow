using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.API.DTOs.Vocabulary
{
    public class ExampleDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; } = string.Empty;

        [StringLength(500)]
        public string Translation { get; set; } = string.Empty;

        [Range(1, 5)]
        public int DifficultyLevel { get; set; } = 3;
    }
}
