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
    /// Nhiệm vụ học tập
    /// </summary>
    [Index(nameof(GoalId), nameof(TaskName), Name = "IX_StudyTask_Goal_Name")]
    public class StudyTask : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Required]
        public int GoalId { get; set; }

        [Required]
        [StringLength(100)]
        public string TaskName { get; set; }

        public string Description { get; set; }

        public int? EstimatedDuration { get; set; }

        [StringLength(20)]
        public string DurationUnit { get; set; } // Minutes, Hours

        // Cải tiến: Liên kết với kế hoạch
        public int? ItemId { get; set; } // Liên kết với StudyPlanItem

        // Cải tiến: Phân loại và mức độ
        [Range(1, 5)]
        public int? Priority { get; set; } = 3; // Mức độ ưu tiên (1-5)

        [StringLength(50)]
        public string TaskType { get; set; } // Study, Practice, Review, Test

        [StringLength(50)]
        public string TaskCategory { get; set; } // Reading, Writing, Listening, etc.

        // Cải tiến: Lịch trình và thời gian
        public DateTime? ScheduledDate { get; set; } // Ngày dự kiến

        public DateTime? DueDate { get; set; } // Hạn cuối

        public bool HasTimeConstraint { get; set; } = false; // Có ràng buộc thời gian

        // Cải tiến: Nguồn lực và tài liệu
        public string RequiredResources { get; set; } // Nguồn lực cần thiết

        public string AttachmentUrls { get; set; } // Tài liệu đính kèm

        // Cải tiến: Tiến trình và trạng thái
        public bool IsRequired { get; set; } = true; // Bắt buộc

        public bool IsCompleted { get; set; } = false; // Hoàn thành

        public DateTime? CompletedAt { get; set; } // Thời điểm hoàn thành

        [StringLength(50)]
        public string Status { get; set; } = "NotStarted"; // NotStarted, InProgress, OnHold, Completed

        public float CompletionPercentage { get; set; } = 0; // Phần trăm hoàn thành

        // Cải tiến: Lặp lại và thông báo
        public bool IsRecurring { get; set; } = false; // Lặp lại

        [StringLength(50)]
        public string RecurrencePattern { get; set; } // Mẫu lặp lại

        public bool EnableReminders { get; set; } = true; // Bật nhắc nhở

        [StringLength(255)]
        public string ReminderSettings { get; set; } // Cài đặt nhắc nhở

        // Cải tiến: Phụ thuộc và điều kiện
        public string Dependencies { get; set; } // Phụ thuộc vào các task khác

        public string CompletionConditions { get; set; } // Điều kiện hoàn thành

        // Navigation properties
        [ForeignKey("GoalId")]
        public virtual StudyGoal Goal { get; set; }

        [ForeignKey("ItemId")]
        public virtual StudyPlanItem Item { get; set; }

        public virtual ICollection<TaskCompletion> TaskCompletions { get; set; }
        public virtual ICollection<Scheduling.ScheduleItem> ScheduleItems { get; set; }
    }
}
