using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Vocabulary
{
    /// <summary>
    /// Vocabulary DTO for responses
    /// </summary>
    public class VocabularyDto : BaseDto
    {
        /// <summary>
        /// Vocabulary ID
        /// </summary>
        public new int Id { get; set; }

        /// <summary>
        /// Japanese word/phrase
        /// </summary>
        public string Japanese { get; set; } = null!;

        /// <summary>
        /// Kana reading
        /// </summary>
        public string? Kana { get; set; }

        /// <summary>
        /// Romaji reading
        /// </summary>
        public string? Romaji { get; set; }

        /// <summary>
        /// Vietnamese translation
        /// </summary>
        public string? Vietnamese { get; set; }

        /// <summary>
        /// English translation
        /// </summary>
        public string? English { get; set; }

        /// <summary>
        /// Example usage
        /// </summary>
        public string? Example { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Group ID
        /// </summary>
        public int? GroupID { get; set; }

        /// <summary>
        /// Group name
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
        /// JLPT level
        /// </summary>
        public string? Level { get; set; }

        /// <summary>
        /// Part of speech
        /// </summary>
        public string? PartOfSpeech { get; set; }

        /// <summary>
        /// Audio file path
        /// </summary>
        public string? AudioFile { get; set; }

        /// <summary>
        /// Last modification timestamp
        /// </summary>
        public DateTime LastModifiedAt { get; set; }

        /// <summary>
        /// Related kanji list
        /// </summary>
        public List<KanjiInfoDto>? Kanjis { get; set; }
    }

    /// <summary>
    /// Create vocabulary request DTO
    /// </summary>
    public class CreateVocabularyDto
    {
        /// <summary>
        /// Japanese word/phrase
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Japanese { get; set; } = null!;

        /// <summary>
        /// Kana reading
        /// </summary>
        [StringLength(100)]
        public string? Kana { get; set; }

        /// <summary>
        /// Romaji reading
        /// </summary>
        [StringLength(100)]
        public string? Romaji { get; set; }

        /// <summary>
        /// Vietnamese translation
        /// </summary>
        [StringLength(255)]
        public string? Vietnamese { get; set; }

        /// <summary>
        /// English translation
        /// </summary>
        [StringLength(255)]
        public string? English { get; set; }

        /// <summary>
        /// Example usage
        /// </summary>
        public string? Example { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Group ID
        /// </summary>
        public int? GroupID { get; set; }

        /// <summary>
        /// JLPT level
        /// </summary>
        [StringLength(20)]
        public string? Level { get; set; }

        /// <summary>
        /// Part of speech
        /// </summary>
        [StringLength(50)]
        public string? PartOfSpeech { get; set; }

        /// <summary>
        /// List of kanji IDs to associate
        /// </summary>
        public List<int>? KanjiIDs { get; set; }
    }

    /// <summary>
    /// Update vocabulary request DTO
    /// </summary>
    public class UpdateVocabularyDto
    {
        /// <summary>
        /// Japanese word/phrase
        /// </summary>
        [StringLength(100)]
        public string? Japanese { get; set; }

        /// <summary>
        /// Kana reading
        /// </summary>
        [StringLength(100)]
        public string? Kana { get; set; }

        /// <summary>
        /// Romaji reading
        /// </summary>
        [StringLength(100)]
        public string? Romaji { get; set; }

        /// <summary>
        /// Vietnamese translation
        /// </summary>
        [StringLength(255)]
        public string? Vietnamese { get; set; }

        /// <summary>
        /// English translation
        /// </summary>
        [StringLength(255)]
        public string? English { get; set; }

        /// <summary>
        /// Example usage
        /// </summary>
        public string? Example { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Group ID
        /// </summary>
        public int? GroupID { get; set; }

        /// <summary>
        /// JLPT level
        /// </summary>
        [StringLength(20)]
        public string? Level { get; set; }

        /// <summary>
        /// Part of speech
        /// </summary>
        [StringLength(50)]
        public string? PartOfSpeech { get; set; }

        /// <summary>
        /// List of kanji IDs to associate
        /// </summary>
        public List<int>? KanjiIDs { get; set; }
    }

    /// <summary>
    /// Kanji information DTO
    /// </summary>
    public class KanjiInfoDto
    {
        /// <summary>
        /// Kanji ID
        /// </summary>
        public int KanjiID { get; set; }

        /// <summary>
        /// Kanji character
        /// </summary>
        public string Character { get; set; } = null!;

        /// <summary>
        /// On'yomi reading
        /// </summary>
        public string? Onyomi { get; set; }

        /// <summary>
        /// Kun'yomi reading
        /// </summary>
        public string? Kunyomi { get; set; }

        /// <summary>
        /// Meaning
        /// </summary>
        public string? Meaning { get; set; }

        /// <summary>
        /// JLPT level
        /// </summary>
        public string? JLPTLevel { get; set; }

        /// <summary>
        /// Position in the vocabulary word
        /// </summary>
        public int? Position { get; set; }
    }

    /// <summary>
    /// Vocabulary group DTO for responses
    /// </summary>
    public class VocabularyGroupDto : BaseDto
    {
        /// <summary>
        /// Group ID
        /// </summary>
        public new int Id { get; set; }

        /// <summary>
        /// Group name
        /// </summary>
        public string GroupName { get; set; } = null!;

        /// <summary>
        /// Group description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Category ID
        /// </summary>
        public int? CategoryID { get; set; }

        /// <summary>
        /// Category name
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Whether the group is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Vocabulary count in this group
        /// </summary>
        public int VocabularyCount { get; set; }
    }

    /// <summary>
    /// Create vocabulary group request DTO
    /// </summary>
    public class CreateVocabularyGroupDto
    {
        /// <summary>
        /// Group name
        /// </summary>
        [Required]
        [StringLength(100)]
        public string GroupName { get; set; } = null!;

        /// <summary>
        /// Group description
        /// </summary>
        [StringLength(255)]
        public string? Description { get; set; }

        /// <summary>
        /// Category ID
        /// </summary>
        public int? CategoryID { get; set; }
    }

    /// <summary>
    /// Update vocabulary group request DTO
    /// </summary>
    public class UpdateVocabularyGroupDto
    {
        /// <summary>
        /// Group name
        /// </summary>
        [StringLength(100)]
        public string? GroupName { get; set; }

        /// <summary>
        /// Group description
        /// </summary>
        [StringLength(255)]
        public string? Description { get; set; }

        /// <summary>
        /// Category ID
        /// </summary>
        public int? CategoryID { get; set; }

        /// <summary>
        /// Whether the group is active
        /// </summary>
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// Kanji DTO for responses
    /// </summary>
    public class KanjiDto : BaseDto
    {
        /// <summary>
        /// Kanji ID
        /// </summary>
        public new int Id { get; set; }

        /// <summary>
        /// Kanji character
        /// </summary>
        public string Character { get; set; } = null!;

        /// <summary>
        /// On'yomi reading
        /// </summary>
        public string? Onyomi { get; set; }

        /// <summary>
        /// Kun'yomi reading
        /// </summary>
        public string? Kunyomi { get; set; }

        /// <summary>
        /// Meaning
        /// </summary>
        public string? Meaning { get; set; }

        /// <summary>
        /// Stroke count
        /// </summary>
        public int? StrokeCount { get; set; }

        /// <summary>
        /// JLPT level
        /// </summary>
        public string? JLPTLevel { get; set; }

        /// <summary>
        /// School grade
        /// </summary>
        public int? Grade { get; set; }

        /// <summary>
        /// Radicals
        /// </summary>
        public string? Radicals { get; set; }

        /// <summary>
        /// Components
        /// </summary>
        public string? Components { get; set; }

        /// <summary>
        /// Examples
        /// </summary>
        public string? Examples { get; set; }

        /// <summary>
        /// Mnemonic hint
        /// </summary>
        public string? MnemonicHint { get; set; }

        /// <summary>
        /// Writing order image path
        /// </summary>
        public string? WritingOrderImage { get; set; }

        /// <summary>
        /// Component details
        /// </summary>
        public List<KanjiComponentDto>? KanjiComponents { get; set; }
    }

    /// <summary>
    /// Create kanji request DTO
    /// </summary>
    public class CreateKanjiDto
    {
        /// <summary>
        /// Kanji character
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Character { get; set; } = null!;

        /// <summary>
        /// On'yomi reading
        /// </summary>
        [StringLength(100)]
        public string? Onyomi { get; set; }

        /// <summary>
        /// Kun'yomi reading
        /// </summary>
        [StringLength(100)]
        public string? Kunyomi { get; set; }

        /// <summary>
        /// Meaning
        /// </summary>
        [StringLength(255)]
        public string? Meaning { get; set; }

        /// <summary>
        /// Stroke count
        /// </summary>
        public int? StrokeCount { get; set; }

        /// <summary>
        /// JLPT level
        /// </summary>
        [StringLength(10)]
        public string? JLPTLevel { get; set; }

        /// <summary>
        /// School grade
        /// </summary>
        public int? Grade { get; set; }

        /// <summary>
        /// Radicals
        /// </summary>
        [StringLength(100)]
        public string? Radicals { get; set; }

        /// <summary>
        /// Components
        /// </summary>
        [StringLength(100)]
        public string? Components { get; set; }

        /// <summary>
        /// Examples
        /// </summary>
        public string? Examples { get; set; }

        /// <summary>
        /// Mnemonic hint
        /// </summary>
        public string? MnemonicHint { get; set; }

        /// <summary>
        /// Component IDs with positions
        /// </summary>
        public List<KanjiComponentMappingDto>? ComponentMappings { get; set; }
    }

    /// <summary>
    /// Kanji component DTO
    /// </summary>
    public class KanjiComponentDto
    {
        /// <summary>
        /// Component ID
        /// </summary>
        public int ComponentID { get; set; }

        /// <summary>
        /// Component name
        /// </summary>
        public string ComponentName { get; set; } = null!;

        /// <summary>
        /// Component character
        /// </summary>
        public string? Character { get; set; }

        /// <summary>
        /// Component meaning
        /// </summary>
        public string? Meaning { get; set; }

        /// <summary>
        /// Component type
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Position in the kanji
        /// </summary>
        public string? Position { get; set; }
    }

    /// <summary>
    /// Kanji component mapping DTO
    /// </summary>
    public class KanjiComponentMappingDto
    {
        /// <summary>
        /// Component ID
        /// </summary>
        public int ComponentID { get; set; }

        /// <summary>
        /// Position in the kanji
        /// </summary>
        public string? Position { get; set; }
    }
}