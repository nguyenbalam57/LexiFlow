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

namespace LexiFlow.Models.Practice
{
    /// <summary>
    /// Bộ luyện tập
    /// </summary>
    [Index(nameof(SetName), Name = "IX_PracticeSet_Name")]
    [Index(nameof(CreatedByUserId), Name = "IX_PracticeSet_CreatedBy")]
    public class PracticeSet : AuditableEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PracticeSetId { get; set; }

        [Required]
        [StringLength(100)]
        public string SetName { get; set; }

        [StringLength(50)]
        public string SetType { get; set; }

        [StringLength(10)]
        public string Level { get; set; }

        public string Description { get; set; }

        [StringLength(255)]
        public string Skills { get; set; }

        public int? ItemCount { get; set; }

        // Cải tiến: Thời gian hoàn thành ước tính
        public int? EstimatedMinutes { get; set; }

        // Cải tiến: Mức độ khó
        [Range(1, 5)]
        public int? Difficulty { get; set; } = 3;

        // Cải tiến: Tags và danh mục
        public string Tags { get; set; }

        public int? CategoryId { get; set; }

        // Cải tiến: Thống kê sử dụng
        public int UsageCount { get; set; } = 0;
        public double? AverageRating { get; set; }
        public int? RatingCount { get; set; } = 0;

        public bool IsPublic { get; set; } = false;
        public bool IsFeatured { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public int? CreatedByUserId { get; set; }

        // Navigation properties
        [ForeignKey("CreatedByUserId")]
        public virtual User.User CreatedByUser { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Learning.Vocabulary.Category Category { get; set; }

        public virtual ICollection<PracticeSetItem> PracticeSetItems { get; set; }
        public virtual ICollection<UserPracticeSet> UserPracticeSets { get; set; }
    }
}
