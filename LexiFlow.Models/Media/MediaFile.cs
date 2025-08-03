using LexiFlow.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Media
{
    /// <summary>
    /// Mô hình lưu trữ thông tin media
    /// </summary>
    [Index(nameof(VocabularyId), nameof(MediaType), nameof(IsPrimary), Name = "IX_MediaFiles_Vocabulary")]
    [Index(nameof(UserId), nameof(MediaType), nameof(IsPrimary), Name = "IX_MediaFiles_User")]
    [Index(nameof(MediaType), nameof(IsPrimary), Name = "IX_MediaFiles_Primary")]
    public class MediaFile : AuditableEntity, ISoftDeletable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MediaId { get; set; }

        [Required]
        [StringLength(50)]
        public string MediaType { get; set; } // "Image", "Audio", "Video", "Document"

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        [StringLength(255)]
        public string StoragePath { get; set; }

        [StringLength(255)]
        public string OriginalFileName { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; } // MIME type

        public long FileSize { get; set; } // Bytes

        [StringLength(255)]
        public string CdnUrl { get; set; } // URL nếu lưu trên CDN

        // Metadata
        public string MetadataJson { get; set; }

        // Media có thể thuộc về nhiều loại entity
        public int? VocabularyId { get; set; }
        public int? UserId { get; set; }
        public int? KanjiId { get; set; }
        public int? ExampleId { get; set; }
        public int? GrammarId { get; set; }
        public int? QuestionId { get; set; }
        public int? QuestionOptionId { get; set; }
        public int? GrammarExampleId { get; set; }
        public int? KanjiExampleId { get; set; }
        public int? TechnicalTermId { get; set; }
        public int? TermExampleId { get; set; }
        public int? UserVocabularyDetaillId { get; set; }

        // Thứ tự hiển thị nếu có nhiều media
        public int DisplayOrder { get; set; } = 0;

        // Đánh dấu media chính (thumbnail hoặc audio chính)
        public bool IsPrimary { get; set; } = false;

        // Thông tin xử lý
        public bool IsProcessed { get; set; } = false;
        public DateTime? ProcessedAt { get; set; }

        // Các phiên bản khác nhau (thumbnails, audio bitrates, etc.)
        public string Variants { get; set; }

        // Cài đặt hiển thị
        public bool IsPublic { get; set; } = false;

        // Soft delete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }

        // Cải tiến: Nhóm/phân loại media
        [StringLength(100)]
        public string Category { get; set; }

        // Cải tiến: Cấp phép sử dụng
        [StringLength(100)]
        public string License { get; set; }

        [StringLength(255)]
        public string AttributionText { get; set; }

        // Navigation properties
        [ForeignKey("VocabularyId")]
        public virtual Learning.Vocabulary.Vocabulary Vocabulary { get; set; }

        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("KanjiId")]
        public virtual Learning.Kanji.Kanji Kanji { get; set; }

        [ForeignKey("ExampleId")]
        public virtual Learning.Vocabulary.Example Example { get; set; }

        [ForeignKey("GrammarId")]
        public virtual Learning.Grammar.Grammar Grammar { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Exam.Question Question { get; set; }

        [ForeignKey("QuestionOptionId")]
        public virtual Exam.QuestionOption QuestionOption { get; set; }

        [ForeignKey("GrammarExampleId")]
        public virtual Learning.Grammar.GrammarExample GrammarExample { get; set; }

        [ForeignKey("KanjiExampleId")]
        public virtual Learning.Kanji.KanjiExample KanjiExample { get; set; }

        [ForeignKey("TechnicalTermId")]
        public virtual Learning.TechnicalTerms.TechnicalTerm TechnicalTerm { get; set; }

        [ForeignKey("TermExampleId")]
        public virtual Learning.TechnicalTerms.TermExample TermExample { get; set; }

        [ForeignKey("UserVocabularyDetaillId")]
        public virtual Submission.UserVocabularyDetail UserVocabularyDetail { get; set; }

        [ForeignKey("DeletedBy")]
        public virtual User.User DeletedByUser { get; set; }

        // Properties không lưu DB
        [NotMapped]
        public Dictionary<string, object> Metadata
        {
            get => string.IsNullOrEmpty(MetadataJson)
                ? new Dictionary<string, object>()
                : JsonSerializer.Deserialize<Dictionary<string, object>>(MetadataJson);
            set => MetadataJson = JsonSerializer.Serialize(value);
        }

        [NotMapped]
        public Dictionary<string, string> VariantUrls
        {
            get => string.IsNullOrEmpty(Variants)
                ? new Dictionary<string, string>()
                : JsonSerializer.Deserialize<Dictionary<string, string>>(Variants);
            set => Variants = JsonSerializer.Serialize(value);
        }
    }
}
