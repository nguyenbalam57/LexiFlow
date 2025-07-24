using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Models
{
    #region Vocabulary Management Models

    /// <summary>
    /// Represents a vocabulary category
    /// </summary>
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(20)]
        public string Level { get; set; }

        public int? DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<VocabularyGroup> VocabularyGroups { get; set; }
        public virtual ICollection<Grammar> Grammars { get; set; }
    }

    /// <summary>
    /// Represents a vocabulary group
    /// </summary>
    public class VocabularyGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupID { get; set; }

        [Required]
        [StringLength(100)]
        public string GroupName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? CategoryID { get; set; }

        public int? CreatedByUserID { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        [ForeignKey("CreatedByUserID")]
        public virtual User CreatedByUser { get; set; }

        public virtual ICollection<Vocabulary> Vocabularies { get; set; }
    }

    /// <summary>
    /// Represents a vocabulary item
    /// </summary>
    public class Vocabulary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VocabularyID { get; set; }

        [Required]
        [StringLength(100)]
        public string Japanese { get; set; }

        [StringLength(100)]
        public string Kana { get; set; }

        [StringLength(100)]
        public string Romaji { get; set; }

        [StringLength(255)]
        public string Vietnamese { get; set; }

        [StringLength(255)]
        public string English { get; set; }

        public string Example { get; set; }

        public string Notes { get; set; }

        public int? GroupID { get; set; }

        [StringLength(20)]
        public string Level { get; set; }

        [StringLength(50)]
        public string PartOfSpeech { get; set; }

        [StringLength(255)]
        public string AudioFile { get; set; }

        public int? CreatedByUserID { get; set; }

        public int? UpdatedByUserID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        public int? LastModifiedBy { get; set; }

        // Navigation properties
        [ForeignKey("GroupID")]
        public virtual VocabularyGroup Group { get; set; }

        [ForeignKey("CreatedByUserID")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("UpdatedByUserID")]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey("LastModifiedBy")]
        public virtual User LastModifiedByUser { get; set; }

        public virtual ICollection<KanjiVocabulary> KanjiVocabularies { get; set; }
        public virtual ICollection<VocabularyRelation> VocabularyRelations1 { get; set; }
        public virtual ICollection<VocabularyRelation> VocabularyRelations2 { get; set; }
        public virtual ICollection<DepartmentVocabulary> DepartmentVocabularies { get; set; }
        public virtual ICollection<LearningProgress> LearningProgresses { get; set; }
        public virtual ICollection<PersonalWordListItem> PersonalWordListItems { get; set; }
        public virtual ICollection<TestDetail> TestDetails { get; set; }
    }

    /// <summary>
    /// Represents a vocabulary category
    /// </summary>
    public class VocabularyCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        public int? ParentCategoryID { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("ParentCategoryID")]
        public virtual VocabularyCategory ParentCategory { get; set; }

        public virtual ICollection<VocabularyCategory> ChildCategories { get; set; }
        public virtual ICollection<UserVocabularyDetail> UserVocabularyDetails { get; set; }
    }

    /// <summary>
    /// Represents a kanji character
    /// </summary>
    public class Kanji
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KanjiID { get; set; }

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

        public int? CreatedByUserID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("CreatedByUserID")]
        public virtual User CreatedByUser { get; set; }

        public virtual ICollection<KanjiVocabulary> KanjiVocabularies { get; set; }
        public virtual ICollection<KanjiComponentMapping> KanjiComponentMappings { get; set; }
        public virtual ICollection<UserKanjiProgress> UserKanjiProgresses { get; set; }
    }

    /// <summary>
    /// Represents a many-to-many relationship between Kanji and Vocabulary
    /// </summary>
    public class KanjiVocabulary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KanjiVocabularyID { get; set; }

        [Required]
        public int KanjiID { get; set; }

        [Required]
        public int VocabularyID { get; set; }

        public int? Position { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("KanjiID")]
        public virtual Kanji Kanji { get; set; }

        [ForeignKey("VocabularyID")]
        public virtual Vocabulary Vocabulary { get; set; }
    }

    /// <summary>
    /// Represents a grammar point
    /// </summary>
    public class Grammar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GrammarID { get; set; }

        [Required]
        [StringLength(100)]
        public string GrammarPoint { get; set; }

        [StringLength(10)]
        public string JLPTLevel { get; set; }

        [StringLength(255)]
        public string Pattern { get; set; }

        public string Meaning { get; set; }

        public string Usage { get; set; }

        public string Examples { get; set; }

        public string Notes { get; set; }

        public string Conjugation { get; set; }

        [StringLength(255)]
        public string RelatedGrammar { get; set; }

        public int? CategoryID { get; set; }

        public int? CreatedByUserID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        [ForeignKey("CreatedByUserID")]
        public virtual User CreatedByUser { get; set; }

        public virtual ICollection<GrammarExample> GrammarExamples { get; set; }
        public virtual ICollection<UserGrammarProgress> UserGrammarProgresses { get; set; }
    }

    /// <summary>
    /// Represents a grammar example
    /// </summary>
    public class GrammarExample
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExampleID { get; set; }

        [Required]
        public int GrammarID { get; set; }

        public string JapaneseSentence { get; set; }

        public string Romaji { get; set; }

        public string VietnameseTranslation { get; set; }

        public string EnglishTranslation { get; set; }

        [StringLength(255)]
        public string Context { get; set; }

        [StringLength(255)]
        public string AudioFile { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("GrammarID")]
        public virtual Grammar Grammar { get; set; }
    }

    /// <summary>
    /// Represents a technical term
    /// </summary>
    public class TechnicalTerm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TermID { get; set; }

        [Required]
        [StringLength(100)]
        public string Japanese { get; set; }

        [StringLength(100)]
        public string Kana { get; set; }

        [StringLength(100)]
        public string Romaji { get; set; }

        [StringLength(255)]
        public string Vietnamese { get; set; }

        [StringLength(255)]
        public string English { get; set; }

        [StringLength(100)]
        public string Field { get; set; }

        [StringLength(100)]
        public string SubField { get; set; }

        public string Definition { get; set; }

        public string Context { get; set; }

        public string Examples { get; set; }

        [StringLength(50)]
        public string Abbreviation { get; set; }

        [StringLength(255)]
        public string RelatedTerms { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        public int? CreatedByUserID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("CreatedByUserID")]
        public virtual User CreatedByUser { get; set; }

        public virtual ICollection<UserTechnicalTerm> UserTechnicalTerms { get; set; }
        public virtual ICollection<DepartmentVocabulary> DepartmentVocabularies { get; set; }
    }

    /// <summary>
    /// Represents a user's personal vocabulary
    /// </summary>
    public class UserPersonalVocabulary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonalVocabID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string Japanese { get; set; }

        [StringLength(100)]
        public string Kana { get; set; }

        [StringLength(100)]
        public string Romaji { get; set; }

        [StringLength(255)]
        public string Vietnamese { get; set; }

        [StringLength(255)]
        public string English { get; set; }

        public string PersonalNote { get; set; }

        public string Context { get; set; }

        [StringLength(255)]
        public string Source { get; set; }

        public int Importance { get; set; } = 3;

        [StringLength(255)]
        public string Tags { get; set; }

        [StringLength(255)]
        public string ImagePath { get; set; }

        [StringLength(255)]
        public string AudioPath { get; set; }

        public bool IsPublic { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }

    /// <summary>
    /// Represents a user's kanji learning progress
    /// </summary>
    public class UserKanjiProgress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgressID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int KanjiID { get; set; }

        public int RecognitionLevel { get; set; } = 0;

        public int WritingLevel { get; set; } = 0;

        public DateTime? LastPracticed { get; set; }

        public int PracticeCount { get; set; } = 0;

        public int CorrectCount { get; set; } = 0;

        public DateTime? NextReviewDate { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("KanjiID")]
        public virtual Kanji Kanji { get; set; }
    }

    /// <summary>
    /// Represents a user's grammar learning progress
    /// </summary>
    public class UserGrammarProgress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgressID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int GrammarID { get; set; }

        public int UnderstandingLevel { get; set; } = 0;

        public int UsageLevel { get; set; } = 0;

        public DateTime? LastStudied { get; set; }

        public int StudyCount { get; set; } = 0;

        public float? TestScore { get; set; }

        public DateTime? NextReviewDate { get; set; }

        public string PersonalNotes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("GrammarID")]
        public virtual Grammar Grammar { get; set; }
    }

    /// <summary>
    /// Represents a user's technical terms
    /// </summary>
    public class UserTechnicalTerm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserTermID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int TermID { get; set; }

        public bool IsBookmarked { get; set; } = false;

        public int UsageFrequency { get; set; } = 0;

        public DateTime? LastUsed { get; set; }

        public string PersonalExample { get; set; }

        public string WorkContext { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("TermID")]
        public virtual TechnicalTerm Term { get; set; }
    }

    /// <summary>
    /// Represents a relationship between vocabulary items
    /// </summary>
    public class VocabularyRelation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RelationID { get; set; }

        [Required]
        public int VocabularyID1 { get; set; }

        [Required]
        public int VocabularyID2 { get; set; }

        [StringLength(50)]
        public string RelationType { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("VocabularyID1")]
        public virtual Vocabulary Vocabulary1 { get; set; }

        [ForeignKey("VocabularyID2")]
        public virtual Vocabulary Vocabulary2 { get; set; }
    }

    /// <summary>
    /// Represents a kanji component
    /// </summary>
    public class KanjiComponent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ComponentID { get; set; }

        [Required]
        [StringLength(50)]
        public string ComponentName { get; set; }

        [StringLength(10)]
        public string Character { get; set; }

        [StringLength(255)]
        public string Meaning { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public int? StrokeCount { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<KanjiComponentMapping> KanjiComponentMappings { get; set; }
    }

    /// <summary>
    /// Represents a mapping between kanji and its components
    /// </summary>
    public class KanjiComponentMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MappingID { get; set; }

        [Required]
        public int KanjiID { get; set; }

        [Required]
        public int ComponentID { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("KanjiID")]
        public virtual Kanji Kanji { get; set; }

        [ForeignKey("ComponentID")]
        public virtual KanjiComponent Component { get; set; }
    }

    /// <summary>
    /// Represents a vocabulary item for a department
    /// </summary>
    public class DepartmentVocabulary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeptVocabID { get; set; }

        [Required]
        [StringLength(100)]
        public string Department { get; set; }

        public int? VocabularyID { get; set; }

        public int? TechnicalTermID { get; set; }

        public int Priority { get; set; } = 3;

        public bool IsRequired { get; set; } = false;

        public int? CreatedByUserID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("VocabularyID")]
        public virtual Vocabulary Vocabulary { get; set; }

        [ForeignKey("TechnicalTermID")]
        public virtual TechnicalTerm TechnicalTerm { get; set; }

        [ForeignKey("CreatedByUserID")]
        public virtual User CreatedByUser { get; set; }
    }

    #endregion
}
