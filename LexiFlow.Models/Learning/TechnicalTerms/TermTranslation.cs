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
    /// Bản dịch của thuật ngữ kỹ thuật
    /// </summary>
    [Index(nameof(TermId), nameof(LanguageCode), IsUnique = true, Name = "IX_TermTranslation_Term_Lang")]
    public class TermTranslation : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TermId { get; set; }

        [Required]
        [StringLength(255)]
        public string Text { get; set; }

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; }

        // Cải tiến: Phân loại và trạng thái
        public bool IsPrimary { get; set; } = false; // Là bản dịch chính

        [StringLength(50)]
        public string TranslationType { get; set; } // Official, Alternative, Informal

        public bool IsApproved { get; set; } = false; // Đã được phê duyệt

        // Cải tiến: Thông tin bổ sung
        [StringLength(255)]
        public string Source { get; set; } // Nguồn tham khảo

        [StringLength(255)]
        public string Notes { get; set; } // Ghi chú về bản dịch

        [StringLength(255)]
        public string Context { get; set; } // Ngữ cảnh sử dụng

        // Cải tiến: Xác thực
        public int? ApprovedBy { get; set; } // Người phê duyệt

        public DateTime? ApprovedAt { get; set; } // Thời gian phê duyệt

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        // Navigation properties
        [ForeignKey("TermId")]
        public virtual TechnicalTerm Term { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User.User CreatedByUser { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual User.User UpdatedByUser { get; set; }

        [ForeignKey("ApprovedBy")]
        public virtual User.User ApprovedByUser { get; set; }
    }
}
