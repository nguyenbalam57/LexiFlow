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
    /// Thành phần cấu tạo của Kanji
    /// </summary>
    [Index(nameof(Character), IsUnique = true, Name = "IX_KanjiComponent_Character")]
    public class KanjiComponent : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ComponentId { get; set; }

        [Required]
        [StringLength(10)]
        public string Character { get; set; }  // Ký tự thành phần

        [Required]
        [StringLength(50)]
        public string Name { get; set; }  // Tên thành phần

        [StringLength(100)]
        public string Meaning { get; set; }  // Nghĩa của thành phần

        [Required]
        [StringLength(20)]
        public string Type { get; set; }  // Loại: Radical, Element, etc.

        public int StrokeCount { get; set; }  // Số nét

        // Cải tiến: Thông tin nâng cao
        [StringLength(50)]
        public string UnicodeHex { get; set; } // Mã Unicode hex

        [StringLength(255)]
        public string Description { get; set; } // Mô tả chi tiết

        [StringLength(100)]
        public string Mnemonics { get; set; } // Gợi nhớ

        [StringLength(255)]
        public string StrokeOrder { get; set; } // Thứ tự viết nét

        // Cải tiến: Hiển thị và phân loại
        [StringLength(255)]
        public string ImageUrl { get; set; } // Đường dẫn hình ảnh

        [StringLength(50)]
        public string VariantOf { get; set; } // Biến thể của component khác

        public bool IsCommon { get; set; } = true; // Có phải thành phần phổ biến

        // Cải tiến: Thông tin bổ sung
        [StringLength(255)]
        public string Example { get; set; } // Ví dụ Kanji chứa thành phần này

        [StringLength(100)]
        public string Variants { get; set; } // Các biến thể

        public int CreatedBy { get; set; }

        // Cải tiến: Trạng thái
        public bool IsActive { get; set; } = true;
        public DateTime? DeprecatedAt { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy")]
        public virtual User.User CreatedByUser { get; set; }

        public virtual ICollection<KanjiComponentMapping> ComponentMappings { get; set; }
    }
}
