using LexiFlow.Models.Cores;
using LexiFlow.Models.Learning.Grammars;
using LexiFlow.Models.Learning.Kanjis;
using LexiFlow.Models.Learning.TechnicalTerms;
using LexiFlow.Models.Learning.Vocabularys;
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

namespace LexiFlow.Models.Learning.Commons
{
    /// <summary>
    /// Bản dịch 
    /// </summary>
    [Index(nameof(VocabularyId), nameof(LanguageCode), Name = "IX_Translation_VocabLang")]
    public class Translation : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TranslationId { get; set; }

        public int? GrammarId { get; set; }

        public int? KanjiId { get; set; }

        public int? TermId { get; set; }

        public int? VocabularyId { get; set; }

        public int? ExampleId { get; set; } // Liên kết đến ví dụ (nếu có)

        [Required]
        public string Text { get; set; }

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
        public string LanguageCode { get; set; } = "vi"; // Ngôn ngữ định nghĩa

        // Cải tiến: Phân loại và hiển thị
        public bool IsPrimary { get; set; } = false; // Là bản dịch chính

        // Cải tiến: Phạm vi và chi tiết
        [StringLength(50)]
        public string TranslationType { get; set; } // Literal, Functional, Equivalent

        [StringLength(255)]
        public string Notes { get; set; } // Ghi chú về bản dịch

        [StringLength(255)]
        public string Context { get; set; } // Ngữ cảnh sử dụng

        // Cải tiến: Thông tin bổ sung
        [StringLength(255)]
        public string Source { get; set; } // Nguồn tham khảo

        // Cải tiến: Độ tin cậy của bản dịch
        public int Accuracy { get; set; } = 100; // 0-100

        // Cải tiến: Mức độ và phân loại
        [Range(1, 5)]
        public int? Frequency { get; set; } // Tần suất sử dụng (1-5)

        public bool IsArchaic { get; set; } = false; // Nghĩa cổ

        public bool IsMachineTranslated { get; set; } = false;

        // Navigation properties
        [ForeignKey("GrammarId")]
        public virtual Grammar Grammar { get; set; }

        [ForeignKey("KanjiId")]
        public virtual Kanji Kanji { get; set; }

        [ForeignKey("TermId")]
        public virtual TechnicalTerm Term { get; set; }

        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }

        [ForeignKey("ExampleId")]
        public virtual Example Example { get; set; }

        public virtual ICollection<MediaFile> MediaFiles { get; set; }

    }
}
