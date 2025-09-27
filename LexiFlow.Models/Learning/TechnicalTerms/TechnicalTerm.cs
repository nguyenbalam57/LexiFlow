using LexiFlow.Models.Cores;
using LexiFlow.Models.Learning.Commons;
using LexiFlow.Models.Medias;
using LexiFlow.Models.Users;
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
    public class TechnicalTerm : BaseLearning
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TechnicalTermId { get; set; }

        [Required]
        [StringLength(100)]
        public string Term { get; set; }  // Thuật ngữ

        [StringLength(200)]
        public string Reading { get; set; }  // Cách đọc

        [StringLength(100)]
        public string Field { get; set; }  // Lĩnh vực: IT, Medical, v.v.

        [StringLength(100)]
        public string SubField { get; set; }  // Lĩnh vực con

        /// <summary>
        /// Mã ngôn ngữ của pattern ngữ pháp
        /// </summary>
        /// <value>
        /// Code chuẩn ISO 639-1:
        /// - "ja": Tiếng Nhật (mặc định)
        /// - "en": Tiếng Anh
        /// - "vi": Tiếng Việt
        /// Mặc định: "ja"
        /// </value>
        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "ja"; // Ngôn ngữ định nghĩa

        [StringLength(50)]
        public string Abbreviation { get; set; }  // Viết tắt

        public int? DepartmentId { get; set; }  // Phòng ban liên quan

        // Cải tiến: Thông tin chi tiết
        public string Definition { get; set; }  // Định nghĩa thuật ngữ

        public string Context { get; set; }  // Ngữ cảnh sử dụng

        public string RelatedTerms { get; set; }  // Các thuật ngữ liên quan

        // Cải tiến: Phân loại và cấu trúc
        [StringLength(50)]
        public string TermType { get; set; } // Single, Compound, Phrase, Acronym

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

        public string Status { get; set; } = "Active";

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual ICollection<Example> Examples { get; set; }
        public virtual ICollection<Translation> Translations { get; set; }
        
        [NotMapped]
        public virtual ICollection<TermRelation> Relations { get; set; }
        
        public virtual ICollection<MediaFile> MediaFiles { get; set; }
        public virtual ICollection<UserTechnicalTerm> UserTechnicalTerms { get; set; }
    }
}
