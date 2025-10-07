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
    /// Ví dụ sử dụng cho từ vựng, Kanji, điểm ngữ pháp
    /// Có thể mở rộng theo ngữ cảnh: hội thoại, email, báo cáo, thuyết trình...
    /// </summary>
    [Index(nameof(GrammarId), Name = "IX_GrammarExample_Grammar")]
    public class Example : BaseLearning
    {
        /// <summary>
        /// Id ví dụ (Tự tăng)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExampleId { get; set; }

        /// <summary>
        /// Liên kết đến định nghĩa (nếu có)
        /// </summary>
        public int? DefinitionId { get; set; } // Liên kết đến định nghĩa (nếu có)

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
        /// Liên kết đến bộ thành phần Kanji (nếu có)
        /// </summary>
        public int? KanjiComponentId { get; set; } // Ví dụ cho bộ thành phần Kanji

        /// <summary>
        /// Nội dung ví dụ
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Cách đọc ví dụ (nếu có)
        /// </summary>
        public string Reading { get; set; }  // Cách đọc ví dụ

        /// <summary>
        /// Dịch nghĩa ví dụ (nếu có)
        /// </summary>
        public string Context { get; set; }  // Ngữ cảnh sử dụng

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

        /// <summary>
        /// Phân loại và chi tiết
        /// </summary>
        [StringLength(50)]
        public string ExampleType { get; set; } // Basic, Advanced, Conversation, Word, Phrase, Sentence, Compound, Document, Conversation, Report

        /// <summary>
        /// Ngữ cảnh sử dụng (nếu có)
        /// </summary>
        [StringLength(50)]
        public string Scenario { get; set; } // Meeting, Email, Manual

        /// <summary>
        /// Mức độ khó (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? Difficulty { get; set; } = 3; // Mức độ khó

        /// <summary>
        /// Là bản dịch chính
        /// </summary>
        public bool IsPrimary { get; set; } = false; // Là bản dịch chính

        /// <summary>
        /// Ghi chú về ví dụ (nếu có)
        /// </summary>
        public string Notes { get; set; } // Ghi chú về ví dụ

        /// <summary>
        /// Phân tích cấu trúc ngữ pháp, giải thích chi tiết
        /// </summary>
        public string Explanation { get; set; }

        /// <summary>
        /// Câu thay thế (nếu có)
        /// </summary>
        public string AlternativeSentence { get; set; } // Câu thay thế

        /// <summary>
        /// Từ cần chú ý
        /// </summary>
        public string WordsToNote { get; set; } // Từ cần chú ý

        // Navigation properties

        /// <summary>
        /// Liên kết đến định nghĩa (nếu có)
        /// </summary>
        [ForeignKey("DefinitionId")]
        public virtual Definition Definition { get; set; }

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
        /// Liên kết đến bộ thành phần Kanji (nếu có)
        /// </summary>
        [ForeignKey("KanjiComponentId")]
        public virtual KanjiComponent KanjiComponent { get; set; }

        /// <summary>
        /// Các tệp phương tiện liên quan đến ví dụ
        /// </summary>
        public virtual ICollection<MediaFile> MediaFiles { get; set; }

        /// <summary>
        /// Các bản dịch của ví dụ
        /// </summary>
        public virtual ICollection<Translation> Translations { get; set; }

        public virtual ICollection<Segment> Segments { get; set; }

    }
}
