using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Grammar
{
    public class CreateGrammarDto
    {
        [Required]
        [StringLength(100)]
        public string Pattern { get; set; }

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "ja";

        [StringLength(200)]
        public string Reading { get; set; }

        [StringLength(100)]
        public string Level { get; set; }

        public int? CategoryId { get; set; }

        [Range(1, 5)]
        public int DifficultyLevel { get; set; } = 3;

        public string Notes { get; set; }

        public string Tags { get; set; }

        [Required]
        public List<GrammarDefinitionDto> Definitions { get; set; } = new List<GrammarDefinitionDto>();

        public List<GrammarExampleDto> Examples { get; set; } = new List<GrammarExampleDto>();

        public List<GrammarTranslationDto> Translations { get; set; } = new List<GrammarTranslationDto>();
    }
}
