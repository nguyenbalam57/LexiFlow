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

namespace LexiFlow.Models.Learning.Kanji
{
    /// <summary>
    /// Ý nghĩa của ví dụ Kanji theo ngôn ngữ
    /// </summary>
    [Index(nameof(ExampleId), nameof(Language), IsUnique = true, Name = "IX_KanjiExampleMeaning_Example_Lang")]
    public class KanjiExampleMeaning : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MeaningId { get; set; }

        [Required]
        public int ExampleId { get; set; }

        [Required]
        [StringLength(255)]
        public string Meaning { get; set; }  // Nghĩa của ví dụ

        [Required]
        [StringLength(10)]
        public string Language { get; set; }  // Mã ngôn ngữ (vi, en, ...)

        public int SortOrder { get; set; } = 0;  // Thứ tự hiển thị

        // Cải tiến: Thông tin bổ sung
        public string Notes { get; set; } // Ghi chú về nghĩa

        public int? CreatedBy { get; set; } // Người tạo

        public bool IsPrimary { get; set; } = true; // Là nghĩa chính

        // Navigation property
        [ForeignKey("ExampleId")]
        public virtual KanjiExample Example { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User.User CreatedByUser { get; set; }
    }
}
