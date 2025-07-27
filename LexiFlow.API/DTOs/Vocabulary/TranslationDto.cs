using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.API.DTOs.Vocabulary
{
    public class TranslationDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Text { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "en";
    }
}
