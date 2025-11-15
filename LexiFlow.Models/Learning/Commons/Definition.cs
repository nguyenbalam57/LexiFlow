using LexiFlow.Models.Cores;
using LexiFlow.Models.Learning.Grammars;
using LexiFlow.Models.Learning.Kanjis;
using LexiFlow.Models.Learning.Vocabularys;
using LexiFlow.Models.Medias;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Learning.Commons
{
    /// <summary>
    /// Định nghĩa, ý nghĩa
    /// </summary>
    public class Definition : BaseLearning
    {
        /// <summary>
        /// Liên kết đến từ vựng (nếu có)
        /// </summary>
        public int? VocabularyId { get; set; }

        /// <summary>
        /// Liên kết đến điểm ngữ pháp (nếu có)
        /// </summary>
        public int? GrammarId { get; set; }

        /// <summary>
        /// Liên kết đến Kanji (nếu có)
        /// </summary>
        public int? KanjiId { get; set; }

        /// <summary>
        /// Nội dung định nghĩa
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Cách sử dụng (nếu có)
        /// </summary>
        public string Usage { get; set; }  // Cách sử dụng

        /// <summary>
        /// Loại từ (nếu có)
        /// </summary>
        [StringLength(50)]
        public string PartOfSpeech { get; set; }

        /// <summary>
        /// Ngữ cảnh sử dụng (nếu có)
        /// </summary>
        public string Context { get; set; } // Ngữ cảnh sử dụng

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

        /// <summary>
        /// Loại định nghĩa (nếu có)
        /// </summary>
        [StringLength(50)]
        public string DefinitionType { get; set; } // Main, Alternative, Historical

        /// <summary>
        /// Là định nghĩa chính
        /// </summary>
        public bool IsPrimary { get; set; } = false; // Là định nghĩa chính

        /// <summary>
        /// Giới hạn sử dụng (nếu có)
        /// </summary>
        [StringLength(255)]
        public string Limitations { get; set; } // Giới hạn sử dụng

        /// <summary>
        /// Lưu ý khi sử dụng (nếu có)
        /// </summary>
        [StringLength(255)]
        public string Caution { get; set; } // Lưu ý khi sử dụng

        /// <summary>
        /// Nguồn gốc
        /// </summary>
        [StringLength(255)]
        public string Source { get; set; } // Nguồn tham khảo

        // Navigation properties
        /// <summary>
        /// Liên kết đến từ vựng (nếu có)
        /// </summary>
        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }

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
        /// Các tệp phương tiện liên quan đến định nghĩa
        /// </summary>
        public virtual ICollection<MediaFile> MediaFiles { get; set; }

        /// <summary>
        /// Các ví dụ minh họa cho định nghĩa
        /// </summary>
        public virtual ICollection<Example> Examples { get; set; }

    }
}
