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
    /// Ánh xạ giữa Kanji và các thành phần
    /// </summary>
    [Index(nameof(KanjiId), nameof(ComponentId), IsUnique = true, Name = "IX_KanjiComponentMapping_Kanji_Component")]
    public class KanjiComponentMapping : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MappingId { get; set; }

        [Required]
        public int KanjiId { get; set; }

        [Required]
        public int ComponentId { get; set; }

        [StringLength(50)]
        public string Position { get; set; }  // Vị trí thành phần trong Kanji

        // Cải tiến: Thông tin chi tiết
        [StringLength(50)]
        public string Role { get; set; } // Primary, Secondary, Phonetic, Semantic

        public int DisplayOrder { get; set; } = 0; // Thứ tự hiển thị

        // Cải tiến: Tọa độ vị trí
        public float? X { get; set; } // Tọa độ X (0-1)
        public float? Y { get; set; } // Tọa độ Y (0-1)
        public float? Width { get; set; } // Độ rộng (0-1)
        public float? Height { get; set; } // Độ cao (0-1)

        // Cải tiến: Ý nghĩa trong cấu trúc
        [StringLength(255)]
        public string MeaningContribution { get; set; } // Đóng góp về nghĩa

        [StringLength(255)]
        public string PronunciationContribution { get; set; } // Đóng góp về phát âm

        // Cải tiến: Ghi chú
        public string Notes { get; set; } // Ghi chú về quan hệ

        // Navigation properties
        [ForeignKey("KanjiId")]
        public virtual Kanji Kanji { get; set; }

        [ForeignKey("ComponentId")]
        public virtual KanjiComponent Component { get; set; }
    }
}
