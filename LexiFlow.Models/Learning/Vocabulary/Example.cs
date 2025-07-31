using LexiFlow.Models.Core;
using LexiFlow.Models.Media;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Learning.Vocabulary
{
    /// <summary>
    /// Ví dụ sử dụng từ vựng
    /// </summary>
    [Index(nameof(VocabularyId), Name = "IX_Example_VocabularyId")]
    public class Example : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int VocabularyId { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        [StringLength(500)]
        public string Translation { get; set; }

        [StringLength(10)]
        public string LanguageCode { get; set; } = "vi";

        [Range(1, 5)]
        public int DifficultyLevel { get; set; } = 3;

        // Cải tiến: Thêm context và người tạo
        [StringLength(255)]
        public string Context { get; set; }

        public bool IsVerified { get; set; } = false;

        public int CreatedBy { get; set; }

        // Navigation properties
        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User.User CreatedByUser { get; set; }

        public virtual ICollection<MediaFile> MediaFiles { get; set; }
    }
}
