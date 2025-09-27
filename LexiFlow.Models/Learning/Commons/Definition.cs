using LexiFlow.Models.Cores;
using LexiFlow.Models.Learning.Grammars;
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DefinitionId { get; set; }

        public int? VocabularyId { get; set; }

        public int? GrammarId { get; set; }

        [Required]
        public string Text { get; set; }

        public string Usage { get; set; }  // Cách sử dụng

        [StringLength(50)]
        public string PartOfSpeech { get; set; }

        [StringLength(255)]
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

        [StringLength(50)]
        public string DefinitionType { get; set; } // Main, Alternative, Historical

        public bool IsPrimary { get; set; } = false; // Là định nghĩa chính

        [StringLength(255)]
        public string Limitations { get; set; } // Giới hạn sử dụng

        [StringLength(255)]
        public string Caution { get; set; } // Lưu ý khi sử dụng

        // Cải tiến: Nguồn gốc
        [StringLength(255)]
        public string Source { get; set; } // Nguồn tham khảo

        // Navigation properties
        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }

        [ForeignKey("GrammarId")]
        public virtual Grammar Grammar { get; set; }

        public virtual ICollection<MediaFile> MediaFiles { get; set; }

        public virtual ICollection<Example> Examples { get; set; }

    }
}
