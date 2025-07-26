using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.TechnicalTerm
{
    public class TermExampleDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        public string Translation { get; set; }

        public string LanguageCode { get; set; } = "vi";

        public string Context { get; set; }
    }
}
