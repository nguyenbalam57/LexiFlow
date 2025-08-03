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

namespace LexiFlow.Models.Learning.TechnicalTerms
{
    /// <summary>
    /// Ví dụ sử dụng thuật ngữ kỹ thuật
    /// </summary>
    [Index(nameof(TermId), Name = "IX_TermExample_Term")]
    public class TermExample : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TermExampleId { get; set; }

        [Required]
        public int TermId { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        public string Translation { get; set; }

        [StringLength(10)]
        public string LanguageCode { get; set; } = "vi";

        // Cải tiến: Phân loại và ngữ cảnh
        [StringLength(255)]
        public string Context { get; set; } // Ngữ cảnh sử dụng

        [StringLength(50)]
        public string ExampleType { get; set; } // Document, Conversation, Report

        [StringLength(50)]
        public string Scenario { get; set; } // Meeting, Email, Manual

        // Cải tiến: Hiển thị và học tập
        public int DisplayOrder { get; set; } = 0; // Thứ tự hiển thị

        [Range(1, 5)]
        public int? Difficulty { get; set; } = 3; // Mức độ khó

        public bool IsCommon { get; set; } = true; // Ví dụ phổ biến

        // Cải tiến: Media và bổ sung
        [StringLength(255)]
        public string AudioUrl { get; set; } // Đường dẫn âm thanh

        public string Notes { get; set; } // Ghi chú về ví dụ

        public string Explanation { get; set; } // Giải thích chi tiết

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        // Navigation properties
        [ForeignKey("TermId")]
        public virtual TechnicalTerm Term { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User.User CreatedByUser { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual User.User UpdatedByUser { get; set; }

        public virtual ICollection<Media.MediaFile> MediaFiles { get; set; }
    }
}
