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
    /// Nhóm các từ vựng liên quan
    /// </summary>
    [Index(nameof(GroupName), Name = "IX_VocabularyGroup_Name")]
    public class VocabularyGroup : AuditableEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }

        [Required]
        [StringLength(100)]
        public string GroupName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? CategoryId { get; set; }

        public int? CreatedByUserId { get; set; }

        // Cải tiến: Thuộc tính nhóm
        [StringLength(50)]
        public string GroupType { get; set; } // Topic, Lesson, Theme, etc.

        [StringLength(255)]
        public string IconPath { get; set; }

        [StringLength(20)]
        public string ColorCode { get; set; }

        // Cải tiến: Hiển thị và sắp xếp
        public int DisplayOrder { get; set; } = 0;
        public string SortingMethod { get; set; } // Alphabetical, Difficulty, Custom

        // Cải tiến: Học tập và thực hành
        public bool IncludeInPractice { get; set; } = true;
        public bool IncludeInTests { get; set; } = true;
        public int? SuggestedStudyTimeMinutes { get; set; }

        // Cải tiến: Giới hạn truy cập
        public bool IsPublic { get; set; } = true;
        public string AllowedRoles { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User.User CreatedByUser { get; set; }

        public virtual ICollection<Vocabulary> Vocabularies { get; set; }

        // Cải tiến: Relationships mới
        public virtual ICollection<GroupVocabularyRelation> VocabularyRelations { get; set; }
        public virtual ICollection<Progress.LearningSession> LearningSessions { get; set; }
    }
}
