using LexiFlow.Models.Cores;
using LexiFlow.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Schedulings
{
    /// <summary>
    /// Nhắc nhở lịch trình
    /// </summary>
    public class ScheduleReminder : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReminderId { get; set; }

        [Required]
        public int ItemId { get; set; }

        public int? UserId { get; set; }

        public int? ReminderTime { get; set; }

        [StringLength(20)]
        public string ReminderUnit { get; set; }

        // Cải tiến: Kênh gửi nhắc nhở
        public bool IsEmailReminder { get; set; } = false;
        public bool IsPopupReminder { get; set; } = true;
        public bool IsPushReminder { get; set; } = false;

        // Cải tiến: Chi tiết gửi
        [StringLength(255)]
        public string ReminderMessage { get; set; }
        public string CustomSound { get; set; }
        public bool IsRepeating { get; set; } = false;
        public int? RepeatIntervalMinutes { get; set; }

        public bool IsSent { get; set; } = false;

        public DateTime? SentAt { get; set; }

        // Cải tiến: Theo dõi tương tác
        public bool IsAcknowledged { get; set; } = false;
        public DateTime? AcknowledgedAt { get; set; }
        public string AcknowledgementAction { get; set; }

        // Navigation properties
        [ForeignKey("ItemId")]
        public virtual ScheduleItem Item { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
