using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Core.Entities
{
    /// <summary>
    /// Represents a vocabulary item in the system
    /// </summary>
    public class VocabularyItem : BaseEntity
    {
        /// <summary>
        /// The vocabulary term/word
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Term { get; set; } = string.Empty;

        /// <summary>
        /// The language of this vocabulary item
        /// </summary>
        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "en";

        /// <summary>
        /// IPA (International Phonetic Alphabet) pronunciation
        /// </summary>
        [StringLength(100)]
        public string? Pronunciation { get; set; }

        /// <summary>
        /// Audio file path for pronunciation
        /// </summary>
        [StringLength(255)]
        public string? AudioPath { get; set; }

        /// <summary>
        /// Part of speech (noun, verb, adjective, etc.)
        /// </summary>
        [StringLength(50)]
        public string? PartOfSpeech { get; set; }

        /// <summary>
        /// Difficulty level (1-5)
        /// </summary>
        public int DifficultyLevel { get; set; } = 1;

        /// <summary>
        /// Category ID this vocabulary item belongs to
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Navigation property for Category
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        /// <summary>
        /// Tags for this vocabulary item (comma-separated)
        /// </summary>
        [StringLength(255)]
        public string? Tags { get; set; }

        /// <summary>
        /// Usage examples for this vocabulary item
        /// </summary>
        public virtual ICollection<Example> Examples { get; set; } = new List<Example>();

        /// <summary>
        /// Definitions for this vocabulary item
        /// </summary>
        public virtual ICollection<Definition> Definitions { get; set; } = new List<Definition>();

        /// <summary>
        /// Translations of this vocabulary item
        /// </summary>
        public virtual ICollection<Translation> Translations { get; set; } = new List<Translation>();

        /// <summary>
        /// Usage frequency (based on corpus data)
        /// </summary>
        public double? UsageFrequency { get; set; }

        /// <summary>
        /// Notes or additional information
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Image URL or path for visual representation
        /// </summary>
        [StringLength(255)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Synonyms (comma-separated)
        /// </summary>
        [StringLength(255)]
        public string? Synonyms { get; set; }

        /// <summary>
        /// Antonyms (comma-separated)
        /// </summary>
        [StringLength(255)]
        public string? Antonyms { get; set; }

        /// <summary>
        /// Related terms (comma-separated)
        /// </summary>
        [StringLength(255)]
        public string? RelatedTerms { get; set; }

        /// <summary>
        /// Word family (root, prefixes, suffixes)
        /// </summary>
        public string? WordFamily { get; set; }

        /// <summary>
        /// Status of the vocabulary item (Draft, Published, Archived)
        /// </summary>
        [StringLength(20)]
        public string Status { get; set; } = "Draft";

        /// <summary>
        /// View count
        /// </summary>
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// Flag for featured vocabulary items
        /// </summary>
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// Navigation property for user learning progress
        /// </summary>
        public virtual ICollection<UserVocabularyProgress> UserProgress { get; set; } = new List<UserVocabularyProgress>();
    }

    /// <summary>
    /// Represents a definition of a vocabulary item
    /// </summary>
    public class Definition : BaseEntity
    {
        /// <summary>
        /// The vocabulary item this definition belongs to
        /// </summary>
        public int VocabularyItemId { get; set; }

        /// <summary>
        /// Navigation property for VocabularyItem
        /// </summary>
        [ForeignKey("VocabularyItemId")]
        public virtual VocabularyItem? VocabularyItem { get; set; }

        /// <summary>
        /// The actual definition text
        /// </summary>
        [Required]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Part of speech for this specific definition
        /// </summary>
        [StringLength(50)]
        public string? PartOfSpeech { get; set; }

        /// <summary>
        /// Order/priority of this definition
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// Source of this definition (dictionary name, etc.)
        /// </summary>
        [StringLength(100)]
        public string? Source { get; set; }
    }

    /// <summary>
    /// Represents a usage example for a vocabulary item
    /// </summary>
    public class Example : BaseEntity
    {
        /// <summary>
        /// The vocabulary item this example belongs to
        /// </summary>
        public int VocabularyItemId { get; set; }

        /// <summary>
        /// Navigation property for VocabularyItem
        /// </summary>
        [ForeignKey("VocabularyItemId")]
        public virtual VocabularyItem? VocabularyItem { get; set; }

        /// <summary>
        /// The example sentence or phrase
        /// </summary>
        [Required]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Translation of the example (optional)
        /// </summary>
        public string? Translation { get; set; }

        /// <summary>
        /// Source of this example (book, article, etc.)
        /// </summary>
        [StringLength(255)]
        public string? Source { get; set; }

        /// <summary>
        /// Difficulty level of this example (1-5)
        /// </summary>
        public int DifficultyLevel { get; set; } = 1;

        /// <summary>
        /// Order/priority of this example
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// Flag for native/non-native example
        /// </summary>
        public bool IsNativeSource { get; set; } = true;
    }

    /// <summary>
    /// Represents a translation of a vocabulary item
    /// </summary>
    public class Translation : BaseEntity
    {
        /// <summary>
        /// The vocabulary item this translation belongs to
        /// </summary>
        public int VocabularyItemId { get; set; }

        /// <summary>
        /// Navigation property for VocabularyItem
        /// </summary>
        [ForeignKey("VocabularyItemId")]
        public virtual VocabularyItem? VocabularyItem { get; set; }

        /// <summary>
        /// The translated text
        /// </summary>
        [Required]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Language code of this translation
        /// </summary>
        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = string.Empty;

        /// <summary>
        /// Part of speech for this specific translation
        /// </summary>
        [StringLength(50)]
        public string? PartOfSpeech { get; set; }

        /// <summary>
        /// Notes about this translation
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Gender of the translation (for languages with grammatical gender)
        /// </summary>
        [StringLength(20)]
        public string? Gender { get; set; }

        /// <summary>
        /// Register or formality level
        /// </summary>
        [StringLength(50)]
        public string? Register { get; set; }
    }

    /// <summary>
    /// Tracks user progress for vocabulary items
    /// </summary>
    public class UserVocabularyProgress : BaseEntity
    {
        /// <summary>
        /// User ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property for User
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        /// <summary>
        /// Vocabulary item ID
        /// </summary>
        public int VocabularyItemId { get; set; }

        /// <summary>
        /// Navigation property for VocabularyItem
        /// </summary>
        [ForeignKey("VocabularyItemId")]
        public virtual VocabularyItem? VocabularyItem { get; set; }

        /// <summary>
        /// Learning status (New, Learning, Reviewing, Mastered)
        /// </summary>
        [StringLength(20)]
        public string Status { get; set; } = "New";

        /// <summary>
        /// Familiarity level (0-100%)
        /// </summary>
        public int FamiliarityLevel { get; set; } = 0;

        /// <summary>
        /// Last time this item was reviewed
        /// </summary>
        public DateTime? LastReviewedAt { get; set; }

        /// <summary>
        /// Next scheduled review date
        /// </summary>
        public DateTime? NextReviewAt { get; set; }

        /// <summary>
        /// Number of times reviewed
        /// </summary>
        public int ReviewCount { get; set; } = 0;

        /// <summary>
        /// Number of correct answers
        /// </summary>
        public int CorrectCount { get; set; } = 0;

        /// <summary>
        /// Number of incorrect answers
        /// </summary>
        public int IncorrectCount { get; set; } = 0;

        /// <summary>
        /// Flag if this item is bookmarked
        /// </summary>
        public bool IsBookmarked { get; set; } = false;

        /// <summary>
        /// User notes for this vocabulary item
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Custom difficulty rating set by user (1-5)
        /// </summary>
        public int? UserDifficultyRating { get; set; }
    }
}