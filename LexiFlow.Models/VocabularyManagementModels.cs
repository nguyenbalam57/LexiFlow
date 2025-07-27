using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace LexiFlow.Models
{

    /// <summary>
    /// Nhóm các từ vựng liên quan.
    /// Thuộc tính chính: GroupID, GroupName, Description, CategoryID
    /// Quan hệ: Thuộc về một Category, chứa nhiều Vocabulary.
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

    #region Vocabulary

    /// <summary>
    /// Mục đích: Đại diện cho danh mục từ vựng, tổ chức theo cấu trúc cây.
    /// Thuộc tính chính: CategoryID, CategoryName, Description, Level, ParentCategoryID
    /// Quan hệ: Mỗi Category có thể có nhiều Category con (ChildCategories), nhiều Vocabulary.
    /// Đặc điểm: Hỗ trợ cấu trúc cây thông qua ParentCategoryID và ChildCategories.
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

        public int? ParentCategoryID { get; set; }

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("ParentCategoryID")]
        public virtual Category ParentCategory { get; set; }

        public virtual ICollection<Category> ChildCategories { get; set; }

        public virtual ICollection<Vocabulary> Vocabularies { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual User UpdatedByUser { get; set; }
    }

    /// <summary>
    /// Mục đích: Lưu trữ thông tin từ vựng.
    /// Thuộc tính chính: Id, Term, LanguageCode, Reading, CategoryId, DifficultyLevel, Notes, Tags
    /// Quan hệ: Thuộc về một Category, có nhiều Definition, Example, Translation.
    /// Đặc điểm: Cấu trúc hiện đại với hỗ trợ đa ngôn ngữ thông qua Translation.
    /// 
    /// </summary>
    public class Vocabulary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Term { get; set; }

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "ja";

        [StringLength(200)]
        public string Reading { get; set; }

        public int? CategoryId { get; set; }

        [Range(1, 5)]
        public int DifficultyLevel { get; set; } = 3;

        public string Notes { get; set; }

        public string Tags { get; set; }

        [StringLength(255)]
        public string AudioFile { get; set; }

        public string Status { get; set; } = "Active";

        public int CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User ModifiedByUser { get; set; }

        public virtual ICollection<Definition> Definitions { get; set; }

        public virtual ICollection<Example> Examples { get; set; }

        public virtual ICollection<Translation> Translations { get; set; }
    }

    /// <summary>
    /// Mục đích: Lưu trữ định nghĩa cho từ vựng.
    /// Thuộc tính chính: Id, VocabularyItemId, Text, PartOfSpeech, SortOrder
    /// Quan hệ: Thuộc về một Vocabulary.
    /// </summary>
    public class Definition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int VocabularyItemId { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        [StringLength(50)]
        public string PartOfSpeech { get; set; }

        public int SortOrder { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("VocabularyItemId")]
        public virtual Vocabulary VocabularyItem { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }
    }

    /// <summary>
    /// Mục đích: Lưu trữ ví dụ sử dụng từ vựng.
    /// Thuộc tính chính: Id, VocabularyItemId, Text, Translation, DifficultyLevel
    /// Quan hệ: Thuộc về một Vocabulary.
    /// </summary>
    public class Example
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int VocabularyItemId { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        [StringLength(500)]
        public string Translation { get; set; }

        [Range(1, 5)]
        public int DifficultyLevel { get; set; } = 3;

        [StringLength(255)]
        public string AudioFile { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("VocabularyItemId")]
        public virtual Vocabulary VocabularyItem { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }
    }

    /// <summary>
    /// Mục đích: Lưu trữ bản dịch của từ vựng sang các ngôn ngữ khác.
    /// Thuộc tính chính: Id, VocabularyItemId, Text, LanguageCode
    /// Quan hệ: Thuộc về một Vocabulary.
    /// </summary>
    public class Translation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int VocabularyItemId { get; set; }

        [Required]
        [StringLength(255)]
        public string Text { get; set; }

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("VocabularyItemId")]
        public virtual Vocabulary VocabularyItem { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }
    }

    #endregion



    #region KANJI

    /// <summary>
    /// Mục đích: Lưu trữ thông tin về ký tự Kanji.
    /// Thuộc tính chính: Id, Character, OnYomi, KunYomi, Strokes, JLPT, Grade, RadicalName
    /// Quan hệ: Có nhiều KanjiMeaning, KanjiExample, KanjiComponent.
    /// Đặc điểm: Cấu trúc đa ngôn ngữ thông qua KanjiMeaning.
    /// </summary>
    /// <summary>
    /// Represents a Kanji character
    /// </summary>
    public class Kanji
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KanjiID { get; set; }

        [Required]
        [StringLength(10)]
        public string Character { get; set; }  // Ký tự Kanji

        [StringLength(100)]
        public string OnYomi { get; set; }  // Âm Hán (On-yomi)

        [StringLength(100)]
        public string KunYomi { get; set; }  // Âm Nhật (Kun-yomi)

        public int StrokeCount { get; set; }  // Số nét

        [StringLength(20)]
        public string JLPTLevel { get; set; }  // Cấp độ JLPT (N5-N1)

        [StringLength(20)]
        public string Grade { get; set; }  // Lớp học (Tiểu học 1-6, v.v.)

        [StringLength(50)]
        public string RadicalName { get; set; }  // Tên bộ thủ

        public string StrokeOrder { get; set; }  // Thứ tự viết nét

        [StringLength(20)]
        public string Status { get; set; } = "Active";

        public int CreatedByUserID { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<KanjiMeaning> Meanings { get; set; } = new List<KanjiMeaning>();
        public virtual ICollection<KanjiExample> Examples { get; set; } = new List<KanjiExample>();
        public virtual ICollection<KanjiComponentMapping> ComponentMappings { get; set; } = new List<KanjiComponentMapping>();
        public virtual ICollection<KanjiVocabulary> KanjiVocabularies { get; set; } = new List<KanjiVocabulary>();

        [ForeignKey("CreatedByUserID")]
        public virtual User CreatedBy { get; set; }

        [ForeignKey("LastModifiedBy")]
        public virtual User ModifiedBy { get; set; }
    }
    /// <summary>
    /// Mục đích: Lưu trữ nghĩa của Kanji trong các ngôn ngữ khác nhau.
    /// Thuộc tính chính: Id, KanjiId, Text, LanguageCode, SortOrder
    /// Quan hệ: Thuộc về một Kanji.
    /// </summary>
    public class KanjiMeaning
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MeaningID { get; set; }

        [Required]
        public int KanjiID { get; set; }

        [Required]
        [StringLength(255)]
        public string Meaning { get; set; }

        [Required]
        [StringLength(10)]
        public string Language { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("KanjiID")]
        public virtual Kanji Kanji { get; set; }
    }
    /// <summary>
    /// Mục đích: Lưu trữ ví dụ về từ chứa Kanji.
    /// Thuộc tính chính: Id, KanjiId, Word, Reading, Meaning, LanguageCode
    /// Quan hệ: Thuộc về một Kanji.
    /// </summary>
    /// <summary>
    /// Represents an example of Kanji usage
    /// </summary>
    public class KanjiExample
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExampleID { get; set; }

        [Required]
        public int KanjiID { get; set; }

        [Required]
        [StringLength(100)]
        public string Japanese { get; set; }  // Ví dụ bằng tiếng Nhật

        [StringLength(100)]
        public string Kana { get; set; }  // Phiên âm Hiragana/Katakana

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("KanjiID")]
        public virtual Kanji Kanji { get; set; }

        // Collection of meanings in different languages
        public virtual ICollection<KanjiExampleMeaning> Meanings { get; set; } = new List<KanjiExampleMeaning>();
    }

    /// <summary>
    /// Represents a meaning of a Kanji example in a specific language
    /// </summary>
    public class KanjiExampleMeaning
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MeaningID { get; set; }

        [Required]
        public int ExampleID { get; set; }

        [Required]
        [StringLength(255)]
        public string Meaning { get; set; }  // Nghĩa của ví dụ

        [Required]
        [StringLength(10)]
        public string Language { get; set; }  // Mã ngôn ngữ (vi, en, ...)

        public int SortOrder { get; set; }  // Thứ tự hiển thị

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation property
        [ForeignKey("ExampleID")]
        public virtual KanjiExample Example { get; set; }
    }
    /// <summary>
    /// Mục đích: Lưu trữ thông tin về các thành phần cấu tạo nên Kanji.
    /// Thuộc tính chính: Id, KanjiId, ComponentId, Type, Position
    /// Quan hệ: Liên kết Kanji với các Kanji khác làm thành phần.
    /// </summary>
    public class KanjiComponent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ComponentID { get; set; }

        [Required]
        [StringLength(10)]
        public string Character { get; set; }  // Ký tự thành phần

        [Required]
        [StringLength(50)]
        public string Name { get; set; }  // Tên thành phần

        [StringLength(100)]
        public string Meaning { get; set; }  // Nghĩa của thành phần

        [Required]
        [StringLength(20)]
        public string Type { get; set; }  // Loại: Radical, Element, etc.

        public int StrokeCount { get; set; }  // Số nét

        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<KanjiComponentMapping> ComponentMappings { get; set; } = new List<KanjiComponentMapping>();
    }

    /// <summary>
    /// Represents a mapping between Kanji and its components
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
        public string Position { get; set; }  // Vị trí thành phần trong Kanji

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("KanjiID")]
        public virtual Kanji Kanji { get; set; }

        [ForeignKey("ComponentID")]
        public virtual KanjiComponent Component { get; set; }
    }

    /// <summary>
    /// Mục đích: Liên kết giữa Kanji và Vocabulary.
    /// Thuộc tính chính: KanjiVocabularyID, KanjiID, VocabularyID, Position
    /// Quan hệ: Tạo quan hệ nhiều-nhiều giữa Kanji và Vocabulary.
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

        public int? Position { get; set; }  // Vị trí Kanji trong từ vựng

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("KanjiID")]
        public virtual Kanji Kanji { get; set; }

        [ForeignKey("VocabularyID")]
        public virtual Vocabulary Vocabulary { get; set; }
    }

    /// <summary>
    /// Represents a user's progress in learning a Kanji
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

        public int RecognitionLevel { get; set; } = 0;  // Mức độ nhận biết (0-10)

        public int WritingLevel { get; set; } = 0;  // Mức độ viết (0-10)

        public DateTime? LastPracticed { get; set; }  // Thời điểm luyện tập gần nhất

        public int PracticeCount { get; set; } = 0;  // Số lần luyện tập

        public int CorrectCount { get; set; } = 0;  // Số lần trả lời đúng

        public DateTime? NextReviewDate { get; set; }  // Thời điểm ôn tập tiếp theo

        public string Notes { get; set; }  // Ghi chú cá nhân

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("KanjiID")]
        public virtual Kanji Kanji { get; set; }
    }

    #endregion


    #region GRAMMAR
    /// <summary>
    /// Mục đích: Lưu trữ thông tin về điểm ngữ pháp.
    /// Thuộc tính chính: Id, Pattern, LanguageCode, Reading, Level, CategoryId
    /// Quan hệ: Thuộc về một Category, có nhiều GrammarDefinition, GrammarExample, GrammarTranslation.
    /// Đặc điểm: Cấu trúc hiện đại với hỗ trợ đa ngôn ngữ.
    /// </summary>
    public class Grammar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Pattern { get; set; }  // Mẫu câu ngữ pháp

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "ja";

        [StringLength(200)]
        public string Reading { get; set; }  // Cách đọc mẫu ngữ pháp

        [StringLength(100)]
        public string Level { get; set; }  // N5, N4, v.v.

        public int? CategoryId { get; set; }

        [Range(1, 5)]
        public int DifficultyLevel { get; set; } = 3;

        public string Notes { get; set; }

        public string Tags { get; set; }

        public string Status { get; set; } = "Active";

        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public virtual ICollection<GrammarDefinition> Definitions { get; set; }
        public virtual ICollection<GrammarExample> Examples { get; set; }
        public virtual ICollection<GrammarTranslation> Translations { get; set; }
    }
    /// <summary>
    /// Mục đích: Lưu trữ định nghĩa và cách sử dụng ngữ pháp.
    /// Thuộc tính chính: Id, GrammarId, Text, Usage, SortOrder
    /// Quan hệ: Thuộc về một Grammar.
    /// </summary>
    public class GrammarDefinition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int GrammarId { get; set; }
        [Required]
        [StringLength(500)]
        public string Text { get; set; }
        public string Usage { get; set; }  // Cách sử dụng
        public int SortOrder { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey("GrammarId")]
        public virtual Grammar Grammar { get; set; }
    }

    /// <summary>
    /// Mục đích: Lưu trữ ví dụ về cách sử dụng ngữ pháp.
    /// Thuộc tính chính: Id, GrammarId, JapaneseSentence, Reading, TranslationText, LanguageCode
    /// Quan hệ: Thuộc về một Grammar.
    /// </summary>
    public class GrammarExample
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int GrammarId { get; set; }
        [Required]
        [StringLength(500)]
        public string JapaneseSentence { get; set; }
        public string Reading { get; set; }  // Cách đọc ví dụ
        public string TranslationText { get; set; }  // Bản dịch
        public string LanguageCode { get; set; } = "vi";  // Ngôn ngữ của bản dịch
        public string Context { get; set; }  // Ngữ cảnh sử dụng
        public string AudioFile { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey("GrammarId")]
        public virtual Grammar Grammar { get; set; }
    }
    /// <summary>
    /// Mục đích: Lưu trữ bản dịch của mẫu ngữ pháp.
    /// Thuộc tính chính: Id, GrammarId, Text, LanguageCode
    /// Quan hệ: Thuộc về một Grammar.
    /// </summary>
    public class GrammarTranslation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int GrammarId { get; set; }
        [Required]
        [StringLength(255)]
        public string Text { get; set; }
        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey("GrammarId")]
        public virtual Grammar Grammar { get; set; }
    }

    #endregion

    #region TECHNICAL TERMS
    /// <summary>
    /// Mục đích: Lưu trữ thông tin về thuật ngữ kỹ thuật.
    /// Thuộc tính chính: Id, Term, LanguageCode, Reading, Field, SubField, Abbreviation
    /// Quan hệ: Có nhiều TermExample, TermTranslation, TermRelation.
    /// Đặc điểm: Cấu trúc hiện đại với hỗ trợ đa ngôn ngữ.
    /// </summary>
    public class TechnicalTerm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Term { get; set; }  // Thuật ngữ

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "ja";

        [StringLength(200)]
        public string Reading { get; set; }  // Cách đọc

        [StringLength(100)]
        public string Field { get; set; }  // Lĩnh vực: IT, Medical, v.v.

        [StringLength(100)]
        public string SubField { get; set; }  // Lĩnh vực con

        [StringLength(50)]
        public string Abbreviation { get; set; }  // Viết tắt

        [StringLength(100)]
        public string Department { get; set; }  // Phòng ban liên quan

        public string Definition { get; set; }  // Định nghĩa thuật ngữ

        public string Context { get; set; }  // Ngữ cảnh sử dụng

        public string RelatedTerms { get; set; }  // Các thuật ngữ liên quan

        public string Status { get; set; } = "Active";

        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<TermExample> Examples { get; set; }
        public virtual ICollection<TermTranslation> Translations { get; set; }
        public virtual ICollection<TermRelation> Relations { get; set; }
    }
    /// <summary>
    /// Mục đích: Lưu trữ ví dụ sử dụng thuật ngữ.
    /// Thuộc tính chính: Id, TermId, Text, Translation, LanguageCode, Context
    /// Quan hệ: Thuộc về một TechnicalTerm.
    /// </summary>
    public class TermExample
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TermId { get; set; }
        [Required]
        [StringLength(500)]
        public string Text { get; set; }
        public string Translation { get; set; }
        public string LanguageCode { get; set; } = "vi";
        public string Context { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey("TermId")]
        public virtual TechnicalTerm Term { get; set; }
    }
    /// <summary>
    /// Mục đích: Lưu trữ bản dịch của thuật ngữ.
    /// Thuộc tính chính: Id, TermId, Text, LanguageCode
    /// Quan hệ: Thuộc về một TechnicalTerm.
    /// </summary>
    public class TermTranslation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TermId { get; set; }
        [Required]
        [StringLength(255)]
        public string Text { get; set; }
        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey("TermId")]
        public virtual TechnicalTerm Term { get; set; }
    }
    /// <summary>
    /// Mục đích: Lưu trữ mối quan hệ giữa các thuật ngữ.
    /// Thuộc tính chính: Id, TermId1, TermId2, RelationType, Notes
    /// Quan hệ: Liên kết hai TechnicalTerm với nhau.
    /// </summary>
    public class TermRelation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TermId1 { get; set; }
        public int TermId2 { get; set; }
        [StringLength(50)]
        public string RelationType { get; set; }  // Synonym, Antonym, Related, v.v.
        public string Notes { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey("TermId1")]
        public virtual TechnicalTerm Term1 { get; set; }
        [ForeignKey("TermId2")]
        public virtual TechnicalTerm Term2 { get; set; }
    }

    #endregion

    #region USER-RELATED MODELS

    public class UserVocabularyProgress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgressID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int VocabularyID { get; set; }

        public int RecognitionLevel { get; set; } = 0;  // Mức độ nhận biết từ

        public int MemorizationLevel { get; set; } = 0;  // Mức độ ghi nhớ

        public DateTime? LastPracticed { get; set; }  // Lần thực hành cuối

        public int PracticeCount { get; set; } = 0;  // Số lần thực hành

        public int CorrectCount { get; set; } = 0;  // Số lần đúng

        public DateTime? NextReviewDate { get; set; }  // Ngày ôn tập tiếp theo

        public string Notes { get; set; }  // Ghi chú cá nhân

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("VocabularyID")]
        public virtual Vocabulary Vocabulary { get; set; }
    }

    /// <summary>
    /// Mục đích: Lưu trữ từ vựng cá nhân của người dùng.
    /// Thuộc tính chính: PersonalVocabID, UserID, Japanese, Kana, Vietnamese, English, PersonalNote
    /// 
    /// </summary>
    public class UserPersonalVocabulary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string Term { get; set; }  // Thay thế Japanese

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "ja";  // Thêm mới

        [StringLength(200)]
        public string Reading { get; set; }  // Thay thế Kana/Romaji

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

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        // Collections mới
        public virtual ICollection<UserPersonalDefinition> Definitions { get; set; }
        public virtual ICollection<UserPersonalExample> Examples { get; set; }
        public virtual ICollection<UserPersonalTranslation> Translations { get; set; }
    }

    public class UserPersonalDefinition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PersonalVocabId { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        [StringLength(50)]
        public string PartOfSpeech { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("PersonalVocabId")]
        public virtual UserPersonalVocabulary PersonalVocabulary { get; set; }
    }

    public class UserPersonalExample
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PersonalVocabId { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        [StringLength(500)]
        public string Translation { get; set; }

        public string Context { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("PersonalVocabId")]
        public virtual UserPersonalVocabulary PersonalVocabulary { get; set; }
    }

    public class UserPersonalTranslation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PersonalVocabId { get; set; }

        [Required]
        [StringLength(255)]
        public string Text { get; set; }

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("PersonalVocabId")]
        public virtual UserPersonalVocabulary PersonalVocabulary { get; set; }
    }

    public class UserLearningProgress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserID { get; set; }

        public int VocabularyCount { get; set; } = 0;  // Số từ vựng đã học

        public int KanjiCount { get; set; } = 0;  // Số Kanji đã học

        public int GrammarCount { get; set; } = 0;  // Số điểm ngữ pháp đã học

        public int TechnicalTermCount { get; set; } = 0;  // Số thuật ngữ đã học

        public int TotalStudyTimeMinutes { get; set; } = 0;  // Tổng thời gian học (phút)

        public DateTime LastActiveDate { get; set; }  // Ngày hoạt động gần nhất

        public int CurrentStreak { get; set; } = 0;  // Số ngày học liên tục hiện tại

        public int MaxStreak { get; set; } = 0;  // Số ngày học liên tục tối đa

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
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
