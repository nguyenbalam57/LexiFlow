using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.TechnicalTerm
{
    public class TermTranslationDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Text { get; set; }

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; }
    }
}
