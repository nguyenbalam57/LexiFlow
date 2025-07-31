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
    /// Mối quan hệ giữa các thuật ngữ kỹ thuật
    /// </summary>
    [Index(nameof(TermId1), nameof(TermId2), IsUnique = true, Name = "IX_TermRelation_Term1_Term2")]
    public class TermRelation : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TermId1 { get; set; }

        [Required]
        public int TermId2 { get; set; }

        [StringLength(50)]
        public string RelationType { get; set; }  // Synonym, Antonym, Related, v.v.

        // Cải tiến: Chi tiết mối quan hệ
        public string Notes { get; set; } // Ghi chú về mối quan hệ

        [StringLength(255)]
        public string Description { get; set; } // Mô tả mối quan hệ

        // Cải tiến: Mức độ liên quan
        [Range(1, 5)]
        public int? RelationStrength { get; set; } = 3; // Mức độ liên quan

        public bool IsBidirectional { get; set; } = true; // Quan hệ hai chiều

        // Cải tiến: Ví dụ và trạng thái
        [StringLength(255)]
        public string ExampleContext { get; set; } // Ngữ cảnh ví dụ

        public bool IsVerified { get; set; } = false; // Đã được xác minh

        public int? VerifiedBy { get; set; } // Người xác minh

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        // Navigation properties
        [ForeignKey("TermId1")]
        public virtual TechnicalTerm Term1 { get; set; }

        [ForeignKey("TermId2")]
        public virtual TechnicalTerm Term2 { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User.User CreatedByUser { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual User.User UpdatedByUser { get; set; }

        [ForeignKey("VerifiedBy")]
        public virtual User.User VerifiedByUser { get; set; }
    }
}
