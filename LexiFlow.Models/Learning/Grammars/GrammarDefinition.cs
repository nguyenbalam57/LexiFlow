using LexiFlow.Models.Cores;
using LexiFlow.Models.Medias;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Learning.Grammars
{
    /// <summary>
    /// Định nghĩa và cách sử dụng điểm ngữ pháp
    /// </summary>
    public class GrammarDefinition : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int GrammarId { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        public string Usage { get; set; }  // Cách sử dụng

        // Cải tiến: Phân loại và trạng thái
        [StringLength(50)]
        public string DefinitionType { get; set; } // Main, Alternative, Historical

        [StringLength(10)]
        public string LanguageCode { get; set; } = "vi"; // Ngôn ngữ định nghĩa

        public bool IsPrimary { get; set; } = false; // Là định nghĩa chính

        // Cải tiến: Ngữ cảnh và giới hạn
        [StringLength(255)]
        public string Context { get; set; } // Ngữ cảnh sử dụng

        [StringLength(255)]
        public string Limitations { get; set; } // Giới hạn sử dụng

        [StringLength(255)]
        public string Caution { get; set; } // Lưu ý khi sử dụng

        // Cải tiến: Nguồn gốc
        [StringLength(255)]
        public string Source { get; set; } // Nguồn tham khảo

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        // Navigation properties
        [ForeignKey("GrammarId")]
        public virtual Grammar Grammar { get; set; }

        public virtual ICollection<MediaFile> MediaFiles { get; set; }

    }
}
