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
    /// Ví dụ sử dụng điểm ngữ pháp
    /// </summary>
    [Index(nameof(GrammarId), Name = "IX_GrammarExample_Grammar")]
    public class Example : BaseLearning
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExampleId { get; set; }

        public int? DefinitionId { get; set; } // Liên kết đến định nghĩa (nếu có)

        public int? GrammarId { get; set; }

        public int? KanjiId { get; set; }

        public int? TermId { get; set; }

        public int? VocabularyId { get; set; }

        public int? KanjiComponentId { get; set; } // Ví dụ cho bộ thành phần Kanji

        [Required]
        public string Text { get; set; }

        public string Reading { get; set; }  // Cách đọc ví dụ

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

        // Cải tiến: Phân loại và chi tiết
        [StringLength(50)]
        public string ExampleType { get; set; } // Basic, Advanced, Conversation, Word, Phrase, Sentence, Compound, Document, Conversation, Report

        [StringLength(50)]
        public string Scenario { get; set; } // Meeting, Email, Manual

        [Range(1, 5)]
        public int? Difficulty { get; set; } = 3; // Mức độ khó

        public bool IsPrimary { get; set; } = false; // Là bản dịch chính

        public string Notes { get; set; } // Ghi chú về ví dụ

        public string Explanation { get; set; } // Phân tích cấu trúc ngữ pháp, giải thích chi tiết

        public string AlternativeSentence { get; set; } // Câu thay thế

        public string WordsToNote { get; set; } // Từ cần chú ý

        // Navigation properties
        [ForeignKey("DefinitionId")]
        public virtual Definition Definition { get; set; }

        [ForeignKey("GrammarId")]
        public virtual Grammar Grammar { get; set; }

        [ForeignKey("KanjiId")]
        public virtual Kanji Kanji { get; set; }

        [ForeignKey("TermId")]
        public virtual TechnicalTerm Term { get; set; }

        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }

        [ForeignKey("KanjiComponentId")]
        public virtual KanjiComponent KanjiComponent { get; set; }

        public virtual ICollection<MediaFile> MediaFiles { get; set; }

        public virtual ICollection<Translation> Translations { get; set; }

    }
}
