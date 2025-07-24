using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models
{
    #region Scheduling System Models

    /// <summary>
    /// Represents a schedule
    /// </summary>
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleID { get; set; }

        [Required]
        [StringLength(100)]
        public string ScheduleName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? CreatedByUserID { get; set; }

        public bool IsPublic { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("CreatedByUserID")]
        public virtual User CreatedByUser { get; set; }

        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
    }

    /// <summary>
    /// Represents a schedule item type
    /// </summary>
    public class ScheduleItemType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string IconPath { get; set; }

        [StringLength(20)]
        public string DefaultColor { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
    }

    /// <summary>
    /// Represents a schedule recurrence
    /// </summary>
    public class ScheduleRecurrence
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecurrenceID { get; set; }

        [StringLength(50)]
        public string RecurrenceType { get; set; }

        public int? Interval { get; set; }

        [StringLength(20)]
        public string DaysOfWeek { get; set; }

        public int? DayOfMonth { get; set; }

        public int? MonthOfYear { get; set; }

        public DateTime? RecurrenceEnd { get; set; }

        public int? MaxOccurrences { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
    }

    /// <summary>
    /// Represents a schedule item
    /// </summary>
    public class ScheduleItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }

        [Required]
        public int ScheduleID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? TypeID { get; set; }

        public int? RecurrenceID { get; set; }

        [StringLength(255)]
        public string Location { get; set; }

        public bool IsAllDay { get; set; } = false;

        public bool IsCancelled { get; set; } = false;

        public bool IsCompleted { get; set; } = false;

        public int? PriorityLevel { get; set; }

        [StringLength(20)]
        public string ColorCode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("ScheduleID")]
        public virtual Schedule Schedule { get; set; }

        [ForeignKey("TypeID")]
        public virtual ScheduleItemType Type { get; set; }

        [ForeignKey("RecurrenceID")]
        public virtual ScheduleRecurrence Recurrence { get; set; }

        public virtual ICollection<ScheduleItemParticipant> Participants { get; set; }
        public virtual ICollection<ScheduleReminder> Reminders { get; set; }
    }

    /// <summary>
    /// Represents a schedule item participant
    /// </summary>
    public class ScheduleItemParticipant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParticipantID { get; set; }

        [Required]
        public int ItemID { get; set; }

        public int? UserID { get; set; }

        public int? GroupID { get; set; }

        [StringLength(50)]
        public string ParticipantRole { get; set; }

        public int? ResponseStatus { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("ItemID")]
        public virtual ScheduleItem Item { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("GroupID")]
        public virtual Group Group { get; set; }
    }

    /// <summary>
    /// Represents a schedule reminder
    /// </summary>
    public class ScheduleReminder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReminderID { get; set; }

        [Required]
        public int ItemID { get; set; }

        public int? UserID { get; set; }

        public int? ReminderTime { get; set; }

        [StringLength(20)]
        public string ReminderUnit { get; set; }

        public bool IsEmailReminder { get; set; } = false;

        public bool IsPopupReminder { get; set; } = true;

        public bool IsSent { get; set; } = false;

        public DateTime? SentAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        [ForeignKey("ItemID")]
        public virtual ScheduleItem Item { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }

    #endregion
}
