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

namespace LexiFlow.Models.Learning.Vocabulary
{
    /// <summary>
    /// Bản dịch của từ vựng
    /// </summary>
    [Index(nameof(VocabularyId), nameof(LanguageCode), Name = "IX_Translation_VocabLang")]
    public class Translation : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int VocabularyId { get; set; }

        [Required]
        [StringLength(255)]
        public string Text { get; set; }

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; }

        public int CreatedBy { get; set; }

        // Cải tiến: Thứ tự hiển thị
        public int SortOrder { get; set; } = 0;

        // Cải tiến: Độ tin cậy của bản dịch
        public int Accuracy { get; set; } = 100; // 0-100

        public bool IsMachineTranslated { get; set; } = false;

        // Navigation properties
        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User.User CreatedByUser { get; set; }
    }
}
