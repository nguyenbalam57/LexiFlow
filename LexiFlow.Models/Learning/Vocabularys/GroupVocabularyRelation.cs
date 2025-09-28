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

namespace LexiFlow.Models.Learning.Vocabularys
{
    /// <summary>
    /// Liên kết giữa từ vựng và nhóm với thuộc tính bổ sung
    /// </summary>
    [Index(nameof(GroupId), nameof(VocabularyId), IsUnique = true, Name = "IX_GroupVocabulary_Group_Vocab")]
    public class GroupVocabularyRelation : BaseLearning
    {
        /// <summary>
        /// Khóa chính của liên kết (tự tăng).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RelationId { get; set; }

        /// <summary>
        /// ID của nhóm từ vựng.
        /// </summary>
        [Required]
        public int GroupId { get; set; }

        /// <summary>
        /// ID của từ vựng.
        /// </summary>
        [Required]
        public int VocabularyId { get; set; }

        /// <summary>
        /// Loại quan hệ (Primary, Secondary, Related).
        /// </summary>
        [StringLength(50)]
        public string RelationType { get; set; } // Primary, Secondary, Related

        /// <summary>
        /// Mức độ quan trọng trong nhóm (1-5).
        /// </summary>
        public int? Importance { get; set; } = 3; // 1-5

        /// <summary>
        /// Ghi chú bổ sung về từ vựng trong nhóm này.
        /// </summary>
        public string Notes { get; set; }

        // Navigation properties
        /// <summary>
        /// Nhóm từ vựng liên quan.
        /// </summary>
        [ForeignKey("GroupId")]
        public virtual VocabularyGroup Group { get; set; }

        /// <summary>
        /// Từ vựng liên quan.
        /// </summary>
        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }
    }
}
