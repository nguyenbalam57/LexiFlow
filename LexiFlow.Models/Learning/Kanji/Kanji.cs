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
    /// Ký tự Kanji
    /// </summary>
    [Index(nameof(Character), IsUnique = true, Name = "IX_Kanji_Character")]
    [Index(nameof(JLPTLevel), Name = "IX_Kanji_JLPT")]
    public class Kanji : AuditableEntity, ISoftDeletable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KanjiId { get; set; }

        [Required]
        [StringLength(10)]
        public string Character { get; set; }  // Ký tự Kanji

        [StringLength(100)]
        public string OnYomi { get; set; }  // Âm Hán (On-yomi)

        [StringLength(100)]
        public string KunYomi { get; set; }  // Âm Nhật (Kun-yomi)

        public int StrokeCount { get; set; }  // Số nét

        [StringLength(20)]
        public string JLPTLevel { get; set; }  // Cấp độ JLPT (N5-N1)

        [StringLength(20)]
        public string Grade { get; set; }  // Lớp học (Tiểu học 1-6, v.v.)

        [StringLength(50)]
        public string RadicalName { get; set; }  // Tên bộ thủ

        public string StrokeOrder { get; set; }  // Thứ tự viết nét

        // Cải tiến: Thông tin nâng cao
        [StringLength(50)]
        public string UnicodeHex { get; set; } // Mã Unicode hex

        [StringLength(100)]
        public string Mnemonics { get; set; } // Gợi nhớ

        [StringLength(100)]
        public string Etymology { get; set; } // Nguồn gốc

        // Cải tiến: Phân loại và xếp hạng
        [Range(1, 5)]
        public int? Frequency { get; set; } // Tần suất sử dụng (1-5)

        [Range(1, 5)]
        public int? Difficulty { get; set; } // Mức độ khó (1-5)

        // Cải tiến: Nhóm và phân loại
        [StringLength(50)]
        public string KanjiGroup { get; set; } // Nhóm Kanji

        public int? CategoryId { get; set; } // Danh mục

        // Cải tiến: Thuộc tính hiển thị
        public string StrokeOrderJson { get; set; } // Chi tiết thứ tự viết dạng JSON

        public string SearchKeywords { get; set; } // Từ khóa tìm kiếm

        public string Note { get; set; } // Ghi chú

        // Soft delete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Active";

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Vocabulary.Category Category { get; set; }

        [ForeignKey("DeletedBy")]
        public virtual User.User DeletedByUser { get; set; }

        public virtual ICollection<KanjiMeaning> Meanings { get; set; }
        public virtual ICollection<KanjiExample> Examples { get; set; }
        public virtual ICollection<KanjiComponentMapping> ComponentMappings { get; set; }
        public virtual ICollection<KanjiVocabulary> KanjiVocabularies { get; set; }
        public virtual ICollection<Media.MediaFile> MediaFiles { get; set; }
        public virtual ICollection<Progress.UserKanjiProgress> UserProgresses { get; set; }
    }
}
