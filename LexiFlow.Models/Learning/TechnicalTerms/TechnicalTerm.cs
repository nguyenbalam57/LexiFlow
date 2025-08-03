using LexiFlow.Models.Core;
using LexiFlow.Models.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Learning.TechnicalTerms
{
    /// <summary>
    /// Thuật ngữ kỹ thuật
    /// </summary>
    [Index(nameof(Term), nameof(LanguageCode), nameof(Field), Name = "IX_TechnicalTerm_Term_Lang_Field")]
    [Index(nameof(Field), Name = "IX_TechnicalTerm_Field")]
    public class TechnicalTerm : AuditableEntity, ISoftDeletable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TechnicalTermId { get; set; }

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

        // Cải tiến: Thông tin chi tiết
        public string Definition { get; set; }  // Định nghĩa thuật ngữ

        public string Context { get; set; }  // Ngữ cảnh sử dụng

        public string RelatedTerms { get; set; }  // Các thuật ngữ liên quan

        // Cải tiến: Phân loại và cấu trúc
        [StringLength(50)]
        public string TermType { get; set; } // Single, Compound, Phrase, Acronym

        [StringLength(50)]
        public string Origin { get; set; } // Japanese, English, Latin, etc.

        [StringLength(255)]
        public string Etymology { get; set; } // Nguồn gốc từ nguyên

        // Cải tiến: Sử dụng và độ phổ biến
        [Range(1, 5)]
        public int Frequency { get; set; } = 3; // Tần suất sử dụng

        [Range(1, 5)]
        public int? Specificity { get; set; } = 3; // Mức độ chuyên ngành

        public bool IsStandardTerm { get; set; } = true; // Thuật ngữ chuẩn

        // Cải tiến: Liên kết và phân loại
        public int? CategoryId { get; set; } // Danh mục

        public string Tags { get; set; } // Tags

        // Soft delete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }

        public string Status { get; set; } = "Active";

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Vocabulary.Category Category { get; set; }

        [ForeignKey("DeletedBy")]
        public virtual User.User DeletedByUser { get; set; }

        public virtual ICollection<TermExample> Examples { get; set; }
        public virtual ICollection<TermTranslation> Translations { get; set; }
        public virtual ICollection<TermRelation> Relations { get; set; }
        public virtual ICollection<Media.MediaFile> MediaFiles { get; set; }
        public virtual ICollection<UserTechnicalTerm> UserTechnicalTerms { get; set; }
    }
}
