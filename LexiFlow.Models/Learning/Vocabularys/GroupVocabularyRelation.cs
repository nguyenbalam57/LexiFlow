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
    /// Liên kết giữa từ vựng và nhóm với thuộc tính bổ sung
    /// </summary>
    [Index(nameof(GroupId), nameof(VocabularyId), IsUnique = true, Name = "IX_GroupVocabulary_Group_Vocab")]
    public class GroupVocabularyRelation : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RelationId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int VocabularyId { get; set; }

        // Cải tiến: Thuộc tính quan hệ
        public int DisplayOrder { get; set; } = 0;

        [StringLength(50)]
        public string RelationType { get; set; } // Primary, Secondary, Related

        public int? Importance { get; set; } = 3; // 1-5

        public string Notes { get; set; }

        // Navigation properties
        [ForeignKey("GroupId")]
        public virtual VocabularyGroup Group { get; set; }

        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }
    }
}
