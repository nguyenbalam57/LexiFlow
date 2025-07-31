using LexiFlow.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Scheduling
{
    /// <summary>
    /// Mục lịch trình
    /// </summary>
    [Index(nameof(StartTime), Name = "IX_ScheduleItem_StartTime")]
    public class ScheduleItem : BaseEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        [Required]
        public int ScheduleId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? TypeId { get; set; }

        public int? RecurrenceId { get; set; }

        // Cải tiến: Chi tiết địa điểm
        [StringLength(255)]
        public string Location { get; set; }

        [StringLength(255)]
        public string LocationUrl { get; set; }

        public string LocationDetails { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        // Cải tiến: Thông tin bổ sung
        public string AttachmentUrls { get; set; }
        public string MeetingUrl { get; set; }
        public string JoinCode { get; set; }
        public string AdditionalInfo { get; set; }

        public bool IsAllDay { get; set; } = false;
        public bool IsCancelled { get; set; } = false;
        public bool IsCompleted { get; set; } = false;
        public bool IsActive { get; set; } = true;

        // Cải tiến: Trạng thái và tiến trình
        [StringLength(50)]
        public string Status { get; set; } = "Scheduled"; // Scheduled, InProgress, Completed, Cancelled

        public float? CompletionPercentage { get; set; }

        public DateTime? CompletedAt { get; set; }

        public string CompletionNotes { get; set; }

        // Cải tiến: Bài học và tài liệu liên quan
        public int? LessonId { get; set; }
        public int? MaterialId { get; set; }
        public string RelatedEntityType { get; set; }
        public int? RelatedEntityId { get; set; }

        public int? PriorityLevel { get; set; }

        [StringLength(20)]
        public string ColorCode { get; set; }

        // Cải tiến: Mã nguồn
        public int? SourceId { get; set; }
        [StringLength(50)]
        public string SourceType { get; set; }
        public string ExternalId { get; set; }

        // Navigation properties
        [ForeignKey("ScheduleId")]
        public virtual Schedule Schedule { get; set; }

        [ForeignKey("TypeId")]
        public virtual ScheduleItemType Type { get; set; }

        [ForeignKey("RecurrenceId")]
        public virtual ScheduleRecurrence Recurrence { get; set; }

        public virtual ICollection<ScheduleItemParticipant> Participants { get; set; }
        public virtual ICollection<ScheduleReminder> Reminders { get; set; }
    }
}
