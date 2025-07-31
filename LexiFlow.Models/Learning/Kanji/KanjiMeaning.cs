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
    /// Ý nghĩa của Kanji trong các ngôn ngữ
    /// </summary>
    [Index(nameof(KanjiId), nameof(Language), Name = "IX_KanjiMeaning_Kanji_Lang")]
    public class KanjiMeaning : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MeaningId { get; set; }

        [Required]
        public int KanjiId { get; set; }

        [Required]
        [StringLength(255)]
        public string Meaning { get; set; }

        [Required]
        [StringLength(10)]
        public string Language { get; set; }

        public int SortOrder { get; set; } = 0;

        // Cải tiến: Phân loại nghĩa
        [StringLength(50)]
        public string MeaningType { get; set; } // Primary, Secondary, Historical, Modern

        public bool IsPrimary { get; set; } = false; // Nghĩa chính

        // Cải tiến: Nguồn gốc và chi tiết
        [StringLength(255)]
        public string Source { get; set; } // Nguồn tham khảo

        [StringLength(255)]
        public string Context { get; set; } // Ngữ cảnh sử dụng

        [StringLength(255)]
        public string Example { get; set; } // Ví dụ minh họa

        // Cải tiến: Mức độ và phân loại
        [Range(1, 5)]
        public int? Frequency { get; set; } // Tần suất sử dụng (1-5)

        public bool IsArchaic { get; set; } = false; // Nghĩa cổ

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        // Navigation properties
        [ForeignKey("KanjiId")]
        public virtual Kanji Kanji { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User.User CreatedByUser { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual User.User UpdatedByUser { get; set; }
    }
}
