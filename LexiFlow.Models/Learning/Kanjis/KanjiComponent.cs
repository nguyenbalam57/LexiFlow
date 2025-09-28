using LexiFlow.Models.Cores;
using LexiFlow.Models.Learning.Commons;
using LexiFlow.Models.Medias;
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
    /// Thành phần cấu tạo của Kanji
    /// </summary>
    [Index(nameof(Character), IsUnique = true, Name = "IX_KanjiComponent_Character")]
    public class KanjiComponent : BaseLearning
    {

        /// <summary>
        /// Khóa chính của thành phần Kanji (tự tăng).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KanjiComponentId { get; set; }

        /// <summary>
        /// Ký tự thành phần (bắt buộc, tối đa 10 ký tự, duy nhất).
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Character { get; set; }  // Ký tự thành phần

        /// <summary>
        /// Tên thành phần (bắt buộc, tối đa 50 ký tự).
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }  // Tên thành phần

        /// <summary>
        /// Nghĩa của thành phần (tối đa 100 ký tự).
        /// </summary>
        [StringLength(100)] 
        public string Meaning { get; set; }  // Nghĩa của thành phần

        /// <summary>
        /// Loại thành phần (bắt buộc, tối đa 20 ký tự).
        /// Có thể là Radical, Element, etc.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Type { get; set; }  // Loại: Radical, Element, etc.

        /// <summary>
        /// Số nét của thành phần (bắt buộc).
        /// </summary>
        public int StrokeCount { get; set; }  // Số nét

        /// <summary>
        /// Thông tin nâng cao
        /// </summary>
        [StringLength(50)]
        public string UnicodeHex { get; set; } // Mã Unicode hex

        /// <summary>
        /// Thông tin nâng cao
        /// Mô tả chi tiết về thành phần
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; } // Mô tả chi tiết

        /// <summary>
        /// Thông tin nâng cao
        /// Gợi nhớ để học thuộc thành phần
        /// </summary> 
        [StringLength(100)]
        public string Mnemonics { get; set; } // Gợi nhớ

        /// <summary>
        /// Thứ tự viết nét (nếu có)
        /// </summary>
        [StringLength(255)]
        public string StrokeOrder { get; set; } // Thứ tự viết nét

        /// <summary>
        /// Biến thể của component khác (nếu có)
        /// </summary>
        [StringLength(50)]
        public string VariantOf { get; set; } // Biến thể của component khác

        /// <summary>
        /// Có phải thành phần phổ biến không
        /// </summary>
        public bool IsCommon { get; set; } = true; // Có phải thành phần phổ biến

        /// <summary>
        /// Các biến thể (nếu có)
        /// </summary>
        [StringLength(100)]
        public string Variants { get; set; } // Các biến thể

        /// <summary>
        /// Ngày ngừng sử dụng (nếu có)
        /// </summary>
        public DateTime? DeprecatedAt { get; set; }

        // Navigation properties
        /// <summary>
        /// Các Kanji sử dụng thành phần này
        /// </summary>
        public virtual ICollection<KanjiComponentMapping> ComponentMappings { get; set; }

        /// <summary>
        /// Các ví dụ minh họa sử dụng thành phần này
        /// </summary>
        public virtual ICollection<Example> Examples { get; set; }

        /// <summary>
        /// Các tệp phương tiện liên quan đến thành phần này (nếu có)
        /// </summary>
        public virtual ICollection<MediaFile> Medias { get; set; }
    }
}
