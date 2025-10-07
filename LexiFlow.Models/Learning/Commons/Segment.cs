using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LexiFlow.Models.Exams;
using LexiFlow.Models.Learning.Grammars;
using LexiFlow.Models.Learning.Vocabularys;

namespace LexiFlow.Models.Learning.Commons
{

    /// <summary>
    /// Đại diện cho một phân đoạn trong bài học tiếng Nhật, có thể liên kết với từ vựng, ngữ pháp, ví dụ hoặc câu hỏi.
    /// Sử dụng cho furigana hiển thị trên giao diện người dùng.
    /// </summary>
    public class Segment
    {
        /// <summary>
        /// Khóa chính, tự động tăng.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SegmentId { get; set; }

        /// <summary>
        /// ID của từ vựng liên kết.
        /// </summary>
        public int? VocabularyId { get; set; }

        /// <summary>
        /// Vocabulary liên kết với phân đoạn này.
        /// </summary>
        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }

        /// <summary>
        /// ID của ngữ pháp liên kết.
        /// </summary>
        public int? GrammarId { get; set; }
        [ForeignKey("GrammarId")]
        public virtual Grammar Grammar { get; set; }

        /// <summary>
        /// ID của ví dụ liên kết.
        /// </summary>
        public int? ExampleId { get; set; }
        [ForeignKey("ExampleId")]
        public virtual Example Example { get; set; }

        /// <summary>
        /// ID của câu hỏi liên kết.
        /// </summary>
        public int? QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        /// <summary>
        /// ID của phần (section) chứa phân đoạn này. (Đáp án cho câu hỏi có thể nằm trong một phần riêng biệt)
        /// </summary>
        public int? SectionId { get; set; }
        [ForeignKey("SectionId")]
        public virtual Section Section { get; set; }

        /// <summary>
        /// Văn bản Kanji của phân đoạn. Ký tự hoặc cụm từ Kanji.
        /// </summary>
        public string KanjiText { get; set; }

        /// <summary>
        /// Văn bản Furigana của phân đoạn. Phiên âm Hiragana cho Kanji.
        /// </summary>
        public string FuriganaText { get; set; }

        /// <summary>
        /// Kiểm tra xem phân đoạn có chứa văn bản Kanji hay không.
        /// </summary>
        public bool HasKanji { get; set; }

        /// <summary>
        /// Chỉ số bắt đầu của phân đoạn trong văn bản gốc.
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// Chỉ số kết thúc của phân đoạn trong văn bản gốc.
        /// </summary>
        public int EndIndex { get; set; }
    }
}