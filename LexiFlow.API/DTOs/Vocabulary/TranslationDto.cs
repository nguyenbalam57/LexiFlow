using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Vocabulary
{
    public class TranslationDto
    {
        [Required]
        [StringLength(100)]
        public string Text { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "en";
    }
}
