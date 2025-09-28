using LexiFlow.Models.Cores;
using LexiFlow.Models.Learning.Commons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Learning.Kanjis
{
    /// <summary>
    /// Ánh xạ giữa Kanji và các thành phần
    /// Quan hệ nhiều-nhiều với thuộc tính bổ sung
    /// </summary>
    [Index(nameof(KanjiId), nameof(KanjiComponentId), IsUnique = true, Name = "IX_KanjiComponentMapping_Kanji_Component")]
    public class KanjiComponentMapping : BaseLearning
    {
        /// <summary>
        /// Khóa chính của ánh xạ (tự tăng).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KanjiComponentMappingId { get; set; }

        /// <summary>
        /// ID của Kanji.
        /// </summary>
        [Required]
        public int KanjiId { get; set; }

        /// <summary>
        /// ID của thành phần Kanji.
        /// </summary>
        [Required]
        public int KanjiComponentId { get; set; }

        /// <summary>
        /// Vị trí của thành phần trong Kanji (nếu có).
        /// </summary>
        [StringLength(50)]
        public string Position { get; set; }  // Vị trí thành phần trong Kanji

        /// <summary>
        /// Thông tin chi tiết
        /// </summary>
        
        /// <summary>
        /// Vai trò của thành phần trong Kanji (nếu có).
        /// </summary>
        [StringLength(50)]
        public string Role { get; set; } // Primary, Secondary, Phonetic, Semantic

        // Tọa độ vị trí

        /// <summary>
        /// Tọa độ và kích thước thành phần trong Kanji (nếu có).
        /// </summary>
        public float? X { get; set; } // Tọa độ X (0-1)
        public float? Y { get; set; } // Tọa độ Y (0-1)
        public float? Width { get; set; } // Độ rộng (0-1)
        public float? Height { get; set; } // Độ cao (0-1)

        // Cải tiến: Ý nghĩa trong cấu trúc
        /// <summary>
        /// Ý nghĩa đóng góp của thành phần trong Kanji (nếu có).
        /// </summary>
        [StringLength(255)]
        public string MeaningContribution { get; set; } // Đóng góp về nghĩa

        /// <summary>
        /// Đóng góp của thành phần vào phát âm của Kanji (nếu có).
        /// </summary>
        [StringLength(255)]
        public string PronunciationContribution { get; set; } // Đóng góp về phát âm

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Notes { get; set; } // Ghi chú về quan hệ

        // Navigation properties
        [ForeignKey("KanjiId")]
        public virtual Kanji Kanji { get; set; }

        [ForeignKey("KanjiComponentId")]
        public virtual KanjiComponent Component { get; set; }
    }
}
