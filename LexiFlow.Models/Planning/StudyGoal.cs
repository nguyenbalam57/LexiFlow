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

namespace LexiFlow.Models.Planning
{
    /// <summary>
    /// Mục tiêu học tập
    /// </summary>
    [Index(nameof(PlanId), nameof(GoalName), IsUnique = true, Name = "IX_StudyGoal_Plan_Name")]
    public class StudyGoal : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GoalId { get; set; }

        [Required]
        public int PlanId { get; set; }

        [Required]
        [StringLength(100)]
        public string GoalName { get; set; }

        public string Description { get; set; }

        public int? LevelId { get; set; }

        public int? TopicId { get; set; }

        public DateTime? TargetDate { get; set; }

        // Cải tiến: Mức độ và phân loại
        [Range(1, 5)]
        public int? Importance { get; set; } = 3; // Mức độ quan trọng (1-5)

        [Range(1, 5)]
        public int? Difficulty { get; set; } = 3; // Mức độ khó (1-5)

        [StringLength(50)]
        public string GoalType { get; set; } // Vocabulary, Grammar, Reading, etc.

        [StringLength(50)]
        public string GoalScope { get; set; } // Specific, General, Milestone

        // Cải tiến: Mục tiêu và đo lường
        public int? TargetCount { get; set; } // Số lượng mục tiêu

        [StringLength(50)]
        public string MeasurementUnit { get; set; } // Words, Hours, Points, etc.

        public string SuccessCriteria { get; set; } // Tiêu chí thành công

        public string VerificationMethod { get; set; } // Phương pháp xác minh

        // Cải tiến: Trạng thái và tiến trình
        public bool IsCompleted { get; set; } = false;

        public DateTime? CompletedDate { get; set; }

        public float ProgressPercentage { get; set; } = 0;

        [StringLength(50)]
        public string Status { get; set; } = "NotStarted"; // NotStarted, InProgress, OnHold, Completed

        // Cải tiến: Thời gian và nguồn lực
        public int? EstimatedHours { get; set; } // Số giờ ước tính

        public string RequiredResources { get; set; } // Nguồn lực cần thiết

        // Cải tiến: Liên kết và phụ thuộc
        public int? ParentGoalId { get; set; } // Mục tiêu cha

        public string DependsOn { get; set; } // Phụ thuộc vào mục tiêu khác

        // Navigation properties
        [ForeignKey("PlanId")]
        public virtual StudyPlan Plan { get; set; }

        [ForeignKey("LevelId")]
        public virtual Exam.JLPTLevel Level { get; set; }

        [ForeignKey("TopicId")]
        public virtual StudyTopic Topic { get; set; }

        [ForeignKey("ParentGoalId")]
        public virtual StudyGoal ParentGoal { get; set; }

        public virtual ICollection<StudyTask> StudyTasks { get; set; }
        public virtual ICollection<StudyGoal> ChildGoals { get; set; }
        public virtual ICollection<Analytics.StudyReportItem> StudyReportItems { get; set; }
        public virtual ICollection<Analytics.StrengthWeakness> StrengthWeaknesses { get; set; }
    }
}
