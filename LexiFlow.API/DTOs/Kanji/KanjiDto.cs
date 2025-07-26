using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Kanji
{
    public class KanjiDto
    {
        public int KanjiID { get; set; }
        public string Character { get; set; }
        public string Onyomi { get; set; }
        public string Kunyomi { get; set; }
        public string Meaning { get; set; }
        public int? StrokeCount { get; set; }
        public string JLPTLevel { get; set; }
        public int? Grade { get; set; }
        public string Radicals { get; set; }
        public string Components { get; set; }
        public string Examples { get; set; }
        public string MnemonicHint { get; set; }
        public string WritingOrderImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<KanjiComponentDto> KanjiComponents { get; set; } = new List<KanjiComponentDto>();
        public List<VocabularyReferenceDto> RelatedVocabulary { get; set; } = new List<VocabularyReferenceDto>();
    }

    public class KanjiComponentDto
    {
        public int ComponentID { get; set; }
        public string ComponentName { get; set; }
        public string Character { get; set; }
        public string Meaning { get; set; }
        public string Type { get; set; }
        public int? StrokeCount { get; set; }
        public string Position { get; set; }
    }

    public class VocabularyReferenceDto
    {
        public int VocabularyID { get; set; }
        public string Japanese { get; set; }
        public string Kana { get; set; }
        public string Meaning { get; set; }
        public string Position { get; set; }
    }

    public class CreateKanjiDto
    {
        [Required]
        [StringLength(10)]
        public string Character { get; set; }

        [StringLength(100)]
        public string Onyomi { get; set; }

        [StringLength(100)]
        public string Kunyomi { get; set; }

        [StringLength(255)]
        public string Meaning { get; set; }

        public int? StrokeCount { get; set; }

        [StringLength(10)]
        public string JLPTLevel { get; set; }

        public int? Grade { get; set; }

        [StringLength(100)]
        public string Radicals { get; set; }

        [StringLength(100)]
        public string Components { get; set; }

        public string Examples { get; set; }

        public string MnemonicHint { get; set; }

        [StringLength(255)]
        public string WritingOrderImage { get; set; }

        public List<int> ComponentIds { get; set; } = new List<int>();
    }

    public class UpdateKanjiDto : CreateKanjiDto
    {
        public string RowVersionString { get; set; }
    }

    public class KanjiSearchRequestDto
    {
        public string Query { get; set; } = "";
        public string JLPTLevel { get; set; } = "";
        public int? Grade { get; set; }
        public int? StrokeCount { get; set; }
        public string Radical { get; set; } = "";
        public int MaxResults { get; set; } = 20;
    }

    public class KanjiProgressDto
    {
        public int ProgressID { get; set; }
        public int UserID { get; set; }
        public int KanjiID { get; set; }
        public string Character { get; set; }
        public int RecognitionLevel { get; set; }
        public int WritingLevel { get; set; }
        public DateTime? LastPracticed { get; set; }
        public int PracticeCount { get; set; }
        public int CorrectCount { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateKanjiProgressDto
    {
        [Required]
        public int KanjiID { get; set; }
        public int RecognitionLevel { get; set; }
        public int WritingLevel { get; set; }
        public bool CorrectAnswer { get; set; }
        public string Notes { get; set; }
    }
}
