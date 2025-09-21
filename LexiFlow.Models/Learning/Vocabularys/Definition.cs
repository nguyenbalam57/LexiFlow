using LexiFlow.Models.Cores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Learning.Vocabulary
{
    /// <summary>
    /// Định nghĩa của từ vựng
    /// </summary>
    public class Definition : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int VocabularyId { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        [StringLength(50)]
        public string PartOfSpeech { get; set; }

        [StringLength(10)]
        public string LanguageCode { get; set; } = "vi";

        public int SortOrder { get; set; }

        // Cải tiến: Theo dõi người tạo
        public int CreatedBy { get; set; }

        // Navigation properties
        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User.User CreatedByUser { get; set; }
    }
}
