using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Grammar
{
    public class GrammarExampleDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string JapaneseSentence { get; set; }

        public string Reading { get; set; }

        public string TranslationText { get; set; }

        public string LanguageCode { get; set; } = "vi";

        public string Context { get; set; }

        public string AudioFile { get; set; }
    }
}
