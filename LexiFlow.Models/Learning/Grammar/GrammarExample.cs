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
    /// Ví dụ sử dụng điểm ngữ pháp
    /// </summary>
    [Index(nameof(GrammarId), Name = "IX_GrammarExample_Grammar")]
    public class GrammarExample : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GrammarExampleId { get; set; }

        [Required]
        public int GrammarId { get; set; }

        [Required]
        [StringLength(500)]
        public string JapaneseSentence { get; set; }

        public string Reading { get; set; }  // Cách đọc ví dụ

        public string TranslationText { get; set; }  // Bản dịch

        [StringLength(10)]
        public string LanguageCode { get; set; } = "vi";  // Ngôn ngữ của bản dịch

        // Cải tiến: Phân loại và chi tiết
        [StringLength(50)]
        public string ExampleType { get; set; } // Basic, Advanced, Conversation

        [StringLength(255)]
        public string Context { get; set; }  // Ngữ cảnh sử dụng

        [Range(1, 5)]
        public int? Difficulty { get; set; } = 3; // Mức độ khó

        // Cải tiến: Phân tích ngữ pháp
        public string GrammarBreakdown { get; set; } // Phân tích cấu trúc ngữ pháp

        public string AlternativeSentence { get; set; } // Câu thay thế

        public string WordsToNote { get; set; } // Từ cần chú ý

        // Cải tiến: Media và hiển thị
        [StringLength(255)]
        public string AudioFile { get; set; } // File âm thanh

        public int DisplayOrder { get; set; } = 0; // Thứ tự hiển thị

        public bool IsFeatured { get; set; } = false; // Ví dụ nổi bật

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        // Navigation properties
        [ForeignKey("GrammarId")]
        public virtual Grammar Grammar { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User.User CreatedByUser { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual User.User UpdatedByUser { get; set; }

        public virtual ICollection<Media.MediaFile> MediaFiles { get; set; }
    }
}
