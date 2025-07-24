using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models
{
    #region Study Planning Models

    /// <summary>
    /// Represents a study plan
    /// </summary>
    public class StudyPlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlanID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string PlanName { get; set; }

        [StringLength(10)]
        public string TargetLevel { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? TargetDate { get; set; }

        public string Description { get; set; }

        public int? MinutesPerDay { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(50)]
        public string CurrentStatus { get; set; }

        public float CompletionPercentage { get; set; } = 0;

        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public virtual ICollection<StudyGoal> StudyGoals { get; set; }
        public virtual ICollection<StudyPlanItem> StudyPlanItems { get; set; }
    }

    /// <summary>
    /// Represents a study topic
    /// </summary>
    public class StudyTopic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TopicID { get; set; }

        [Required]
        [StringLength(100)]
        public string TopicName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        public int? ParentTopicID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("ParentTopicID")]
        public virtual StudyTopic ParentTopic { get; set; }

        public virtual ICollection<StudyTopic> ChildTopics { get; set; }
        public virtual ICollection<StudyGoal> StudyGoals { get; set; }
    }

    /// <summary>
    /// Represents a study goal
    /// </summary>
    public class StudyGoal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GoalID { get; set; }

        [Required]
        public int PlanID { get; set; }

        [Required]
        [StringLength(100)]
        public string GoalName { get; set; }

        public string Description { get; set; }

        public int? LevelID { get; set; }

        public int? TopicID { get; set; }

        public DateTime? TargetDate { get; set; }

        public int? Importance { get; set; }

        public int? Difficulty { get; set; }

        public bool IsCompleted { get; set; } = false;

        public float ProgressPercentage { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("PlanID")]
        public virtual StudyPlan Plan { get; set; }

        [ForeignKey("LevelID")]
        public virtual JLPTLevel Level { get; set; }

        [ForeignKey("TopicID")]
        public virtual StudyTopic Topic { get; set; }

        public virtual ICollection<StudyTask> StudyTasks { get; set; }
        public virtual ICollection<StudyReportItem> StudyReportItems { get; set; }
    }

    /// <summary>
    /// Represents an item in a study plan
    /// </summary>
    public class StudyPlanItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }

        [Required]
        public int PlanID { get; set; }

        [StringLength(50)]
        public string ItemType { get; set; }

        public string Content { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public int? Priority { get; set; }

        public bool IsRequired { get; set; } = true;

        public int? EstimatedTime { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("PlanID")]
        public virtual StudyPlan Plan { get; set; }

        public virtual ICollection<StudyPlanProgress> StudyPlanProgresses { get; set; }
        public virtual ICollection<StudyTask> StudyTasks { get; set; }
    }

    /// <summary>
    /// Represents progress on a study plan item
    /// </summary>
    public class StudyPlanProgress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgressID { get; set; }

        [Required]
        public int ItemID { get; set; }

        public int? CompletionStatus { get; set; }

        public DateTime? CompletedDate { get; set; }

        public int? ActualTime { get; set; }

        public string UserNotes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("ItemID")]
        public virtual StudyPlanItem Item { get; set; }
    }

    /// <summary>
    /// Represents a study task
    /// </summary>
    public class StudyTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskID { get; set; }

        [Required]
        public int GoalID { get; set; }

        [Required]
        [StringLength(100)]
        public string TaskName { get; set; }

        public string Description { get; set; }

        public int? EstimatedDuration { get; set; }

        [StringLength(20)]
        public string DurationUnit { get; set; }

        public int? ItemID { get; set; }

        public int? Priority { get; set; }

        public bool IsRequired { get; set; } = true;

        public bool IsCompleted { get; set; } = false;

        public DateTime? CompletedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("GoalID")]
        public virtual StudyGoal Goal { get; set; }

        [ForeignKey("ItemID")]
        public virtual StudyPlanItem Item { get; set; }

        public virtual ICollection<TaskCompletion> TaskCompletions { get; set; }
    }

    /// <summary>
    /// Represents a task completion
    /// </summary>
    public class TaskCompletion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompletionID { get; set; }

        [Required]
        public int TaskID { get; set; }

        public DateTime? CompletionDate { get; set; }

        public int? ActualDuration { get; set; }

        public int? EffortLevel { get; set; }

        public int? SatisfactionLevel { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("TaskID")]
        public virtual StudyTask Task { get; set; }
    }

    #endregion
}
