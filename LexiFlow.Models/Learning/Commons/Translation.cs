using LexiFlow.Models.Cores;
using LexiFlow.Models.Learning.Grammars;
using LexiFlow.Models.Learning.Kanjis;
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
        /// <summary>
        /// Id bản dịch (Tự tăng)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TranslationId { get; set; }

        /// <summary>
        /// Liên kết đến điểm ngữ pháp (nếu có)
        /// </summary>
        public int? GrammarId { get; set; }

        /// <summary>
        /// Liên kết đến Kanji (nếu có)
        /// </summary>
        public int? KanjiId { get; set; }

        /// <summary>
        /// Liên kết đến từ vựng (nếu có)
        /// </summary>
        public int? VocabularyId { get; set; }

        /// <summary>
        /// Liên kết đến ví dụ (nếu có)
        /// </summary>
        public int? ExampleId { get; set; } // Liên kết đến ví dụ (nếu có)

        /// <summary>
        /// Nội dung bản dịch
        /// </summary>
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

        /// <summary>
        /// Là bản dịch chính
        /// </summary>
        public bool IsPrimary { get; set; } = false; // Là bản dịch chính

        // Cải tiến: Phạm vi và chi tiết

        /// <summary>
        /// Phân loại bản dịch
        /// </summary>
        [StringLength(50)]
        public string TranslationType { get; set; } // Literal, Functional, Equivalent

        /// <summary>
        /// Ghi chú và ngữ cảnh
        /// </summary>
        [StringLength(255)]
        public string Notes { get; set; } // Ghi chú về bản dịch

        /// <summary>
        /// Ngữ cảnh sử dụng (nếu có)
        /// </summary>
        [StringLength(255)]
        public string Context { get; set; } // Ngữ cảnh sử dụng

        // Cải tiến: Thông tin bổ sung

        /// <summary>
        /// Nguồn tham khảo
        /// </summary>
        [StringLength(255)]
        public string Source { get; set; } // Nguồn tham khảo

        /// <summary>
        /// Độ tin cậy của bản dịch
        /// </summary>
        public int Accuracy { get; set; } = 100; // 0-100

        /// <summary>
        /// Mức độ và phân loại
        /// </summary>
        [Range(1, 5)]
        public int? Frequency { get; set; } // Tần suất sử dụng (1-5)

        /// <summary>
        /// Nghĩa cổ
        /// </summary>
        public bool IsArchaic { get; set; } = false; // Nghĩa cổ

        /// <summary>
        /// Bản dịch do máy tạo ra
        /// </summary>
        public bool IsMachineTranslated { get; set; } = false;

        // Navigation properties

        /// <summary>
        /// Liên kết đến điểm ngữ pháp (nếu có)
        /// </summary>
        [ForeignKey("GrammarId")]
        public virtual Grammar Grammar { get; set; }

        /// <summary>
        /// Liên kết đến Kanji (nếu có)
        /// </summary>
        [ForeignKey("KanjiId")]
        public virtual Kanji Kanji { get; set; }

        /// <summary>
        /// Liên kết đến từ vựng (nếu có)
        /// </summary>
        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }

        /// <summary>
        /// Liên kết đến ví dụ (nếu có) 
        /// </summary>
        [ForeignKey("ExampleId")]
        public virtual Example Example { get; set; }

        /// <summary>
        /// Các tệp phương tiện liên quan đến ví dụ
        /// </summary>
        public virtual ICollection<MediaFile> MediaFiles { get; set; }

    }
}
