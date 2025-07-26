using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Grammar
{
    public class CreateGrammarExampleDto
    {
        [Required]
        public string JapaneseSentence { get; set; }

        public string Romaji { get; set; }

        public string VietnameseTranslation { get; set; }

        public string EnglishTranslation { get; set; }

        [StringLength(255)]
        public string Context { get; set; }

        [StringLength(255)]
        public string AudioFile { get; set; }
    }
}
