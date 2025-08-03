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
    /// Ví dụ sử dụng Kanji
    /// </summary>
    [Index(nameof(KanjiId), Name = "IX_KanjiExample_Kanji")]
    public class KanjiExample : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KanjiExampleId { get; set; }

        [Required]
        public int KanjiId { get; set; }

        [Required]
        [StringLength(100)]
        public string Japanese { get; set; }  // Ví dụ bằng tiếng Nhật

        [StringLength(100)]
        public string Kana { get; set; }  // Phiên âm Hiragana/Katakana

        // Cải tiến: Thông tin chi tiết
        [StringLength(255)]
        public string Translation { get; set; } // Bản dịch mặc định

        [StringLength(50)]
        public string TranslationLanguage { get; set; } = "vi"; // Ngôn ngữ bản dịch

        // Cải tiến: Ngữ cảnh và phân loại
        [StringLength(50)]
        public string ExampleType { get; set; } // Word, Phrase, Sentence, Compound

        [StringLength(100)]
        public string Context { get; set; } // Ngữ cảnh sử dụng

        [Range(1, 5)]
        public int? Difficulty { get; set; } // Mức độ khó (1-5)

        // Cải tiến: Media và học tập
        [StringLength(255)]
        public string AudioUrl { get; set; } // Đường dẫn file âm thanh

        public bool IsCommon { get; set; } = true; // Là ví dụ phổ biến

        public string Notes { get; set; } // Ghi chú về ví dụ

        // Cải tiến: Trạng thái
        public bool IsVerified { get; set; } = false; // Đã được xác minh
        public int? VerifiedBy { get; set; } // Người xác minh

        // Navigation properties
        [ForeignKey("KanjiId")]
        public virtual Kanji Kanji { get; set; }

        [ForeignKey("VerifiedBy")]
        public virtual User.User VerifiedByUser { get; set; }

        // Collection of meanings in different languages
        public virtual ICollection<KanjiExampleMeaning> Meanings { get; set; }

        // Cải tiến: Liên kết media
        public virtual ICollection<Media.MediaFile> MediaFiles { get; set; }
    }
}
