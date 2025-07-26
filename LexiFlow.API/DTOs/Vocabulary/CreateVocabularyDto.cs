using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Vocabulary
{
    public class CreateVocabularyDto
    {
        [Required]
        [StringLength(100)]
        public string Term { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "ja";

        [StringLength(200)]
        public string Reading { get; set; } = string.Empty;

        public int? CategoryId { get; set; }

        [Required]
        public List<DefinitionDto> Definitions { get; set; } = new();

        public List<ExampleDto>? Examples { get; set; }

        public List<TranslationDto>? Translations { get; set; }

        [Range(1, 5)]
        public int DifficultyLevel { get; set; } = 3;

        public string? Notes { get; set; }

        public string? Tags { get; set; }
    }

    public class DefinitionDto
    {
        [Required]
        [StringLength(500)]
        public string Text { get; set; } = string.Empty;

        [StringLength(50)]
        public string PartOfSpeech { get; set; } = string.Empty;

        public int SortOrder { get; set; } = 0;
    }

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
