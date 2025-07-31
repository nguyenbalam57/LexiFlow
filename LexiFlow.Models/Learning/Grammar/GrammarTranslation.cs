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
    /// Bản dịch của điểm ngữ pháp
    /// </summary>
    [Index(nameof(GrammarId), nameof(LanguageCode), IsUnique = true, Name = "IX_GrammarTranslation_Grammar_Lang")]
    public class GrammarTranslation : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int GrammarId { get; set; }

        [Required]
        [StringLength(255)]
        public string Text { get; set; }

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; }

        // Cải tiến: Phân loại và hiển thị
        public bool IsPrimary { get; set; } = false; // Là bản dịch chính

        public int DisplayOrder { get; set; } = 0; // Thứ tự hiển thị

        // Cải tiến: Phạm vi và chi tiết
        [StringLength(50)]
        public string TranslationType { get; set; } // Literal, Functional, Equivalent

        [StringLength(255)]
        public string Notes { get; set; } // Ghi chú về bản dịch

        [StringLength(255)]
        public string Context { get; set; } // Ngữ cảnh sử dụng

        // Cải tiến: Trạng thái
        public bool IsVerified { get; set; } = false; // Đã được xác minh

        public int? VerifiedBy { get; set; } // Người xác minh

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        // Navigation properties
        [ForeignKey("GrammarId")]
        public virtual Grammar Grammar { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User.User CreatedByUser { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual User.User UpdatedByUser { get; set; }

        [ForeignKey("VerifiedBy")]
        public virtual User.User VerifiedByUser { get; set; }
    }
}
