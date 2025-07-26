using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Grammar
{
    public class GrammarDto
    {
        public int GrammarID { get; set; }
        public string GrammarPoint { get; set; }
        public string JLPTLevel { get; set; }
        public string Pattern { get; set; }
        public string Meaning { get; set; }
        public string Usage { get; set; }
        public string Notes { get; set; }
        public string Conjugation { get; set; }
        public string RelatedGrammar { get; set; }
        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<GrammarExampleDto> Examples { get; set; } = new List<GrammarExampleDto>();
    }

    public class GrammarExampleDto
    {
        public int ExampleID { get; set; }
        public string JapaneseSentence { get; set; }
        public string Romaji { get; set; }
        public string VietnameseTranslation { get; set; }
        public string EnglishTranslation { get; set; }
        public string Context { get; set; }
        public string AudioFile { get; set; }
    }

    public class CreateGrammarDto
    {
        [Required]
        [StringLength(100)]
        public string GrammarPoint { get; set; }

        [StringLength(10)]
        public string JLPTLevel { get; set; }

        [StringLength(255)]
        public string Pattern { get; set; }

        public string Meaning { get; set; }

        public string Usage { get; set; }

        public string Notes { get; set; }

        public string Conjugation { get; set; }

        [StringLength(255)]
        public string RelatedGrammar { get; set; }

        public int? CategoryID { get; set; }

        public List<CreateGrammarExampleDto> Examples { get; set; } = new List<CreateGrammarExampleDto>();
    }

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

    public class UpdateGrammarDto : CreateGrammarDto
    {
        public string RowVersionString { get; set; }
    }

    public class GrammarSearchRequestDto
    {
        public string Query { get; set; } = "";
        public string JLPTLevel { get; set; } = "";
        public int? CategoryID { get; set; }
        public string Pattern { get; set; } = "";
        public int MaxResults { get; set; } = 20;
    }

    public class GrammarProgressDto
    {
        public int ProgressID { get; set; }
        public int UserID { get; set; }
        public int GrammarID { get; set; }
        public string GrammarPoint { get; set; }
        public int UnderstandingLevel { get; set; }
        public int UsageLevel { get; set; }
        public DateTime? LastStudied { get; set; }
        public int StudyCount { get; set; }
        public float? TestScore { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public string PersonalNotes { get; set; }
    }

    public class UpdateGrammarProgressDto
    {
        [Required]
        public int GrammarID { get; set; }
        public int UnderstandingLevel { get; set; }
        public int UsageLevel { get; set; }
        public float? TestScore { get; set; }
        public string PersonalNotes { get; set; }
    }
}
