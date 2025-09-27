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

namespace LexiFlow.Models.Learning.Vocabularys
{
    /// <summary>
    /// Nhóm các từ vựng liên quan
    /// </summary>
    [Index(nameof(GroupName), Name = "IX_VocabularyGroup_Name")]
    public class VocabularyGroup : BaseLearning
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VocabularyGroupId { get; set; }

        [Required]
        [StringLength(100)]
        public string GroupName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? CategoryId { get; set; }
        public int? MediaFileId { get; set; }

        // Cải tiến: Thuộc tính nhóm
        [StringLength(50)]
        public string GroupType { get; set; } // Topic, Lesson, Theme, etc.

        [StringLength(20)]
        public string ColorCode { get; set; }

        public string SortingMethod { get; set; } // Alphabetical, Difficulty, Custom

        // Cải tiến: Học tập và thực hành
        public bool IncludeInPractice { get; set; } = true;
        public bool IncludeInTests { get; set; } = true;
        public int? SuggestedStudyTimeMinutes { get; set; }

        // Cải tiến: Giới hạn truy cập
        public bool IsPublic { get; set; } = true;
        public string AllowedRoles { get; set; }

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [ForeignKey("MediaFileId")]
        public virtual MediaFile Media { get; set; }

        public virtual ICollection<Vocabulary> Vocabularies { get; set; }

        // Cải tiến: Relationships mới
        public virtual ICollection<GroupVocabularyRelation> VocabularyRelations { get; set; }
        public virtual ICollection<Progress.LearningSession> LearningSessions { get; set; }
    }
}
