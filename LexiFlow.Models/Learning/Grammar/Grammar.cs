using LexiFlow.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Learning.Grammar
{
    /// <summary>
    /// Điểm ngữ pháp
    /// </summary>
    [Index(nameof(Pattern), IsUnique = true, Name = "IX_Grammar_Pattern")]
    [Index(nameof(Level), Name = "IX_Grammar_Level")]
    public class Grammar : AuditableEntity, ISoftDeletable
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

        // Cải tiến: Phân loại và cấu trúc
        [StringLength(50)]
        public string GrammarType { get; set; } // Particle, Conjugation, Sentence Pattern

        [StringLength(50)]
        public string FormType { get; set; } // Casual, Formal, Polite

        public bool IsNegative { get; set; } = false; // Dạng phủ định
        public bool IsPast { get; set; } = false; // Thì quá khứ

        // Cải tiến: Luật cấu tạo và cách dùng
        public string FormationRules { get; set; } // Quy tắc cấu tạo
        public string UsageNotes { get; set; } // Ghi chú cách dùng
        public string CommonMistakes { get; set; } // Lỗi thường gặp

        // Cải tiến: Mối quan hệ ngữ pháp
        public string RelatedPatterns { get; set; } // Mẫu ngữ pháp liên quan
        public int? ParentGrammarId { get; set; } // Ngữ pháp cha

        // Cải tiến: Hiển thị và học tập
        public string Shortcut { get; set; } // Cách nhớ ngắn gọn
        [StringLength(255)]
        public string Mnemonic { get; set; } // Mẹo ghi nhớ

        public string Notes { get; set; }
        public string Tags { get; set; }

        // Soft delete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }

        public string Status { get; set; } = "Active";

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Vocabulary.Category Category { get; set; }

        [ForeignKey("ParentGrammarId")]
        public virtual Grammar ParentGrammar { get; set; }

        [ForeignKey("DeletedBy")]
        public virtual User.User DeletedByUser { get; set; }

        public virtual ICollection<Grammar> ChildGrammars { get; set; }
        public virtual ICollection<GrammarDefinition> Definitions { get; set; }
        public virtual ICollection<GrammarExample> Examples { get; set; }
        public virtual ICollection<GrammarTranslation> Translations { get; set; }
        public virtual ICollection<Media.MediaFile> MediaFiles { get; set; }
        public virtual ICollection<Progress.UserGrammarProgress> UserProgresses { get; set; }
    }
}
