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

namespace LexiFlow.Models.Scheduling
{
    /// <summary>
    /// Lịch trình
    /// </summary>
    [Index(nameof(ScheduleName), Name = "IX_Schedule_Name")]
    public class Schedule : AuditableEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleId { get; set; }

        [Required]
        [StringLength(100)]
        public string ScheduleName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        // Cải tiến: Thông tin chi tiết
        [StringLength(50)]
        public string ScheduleType { get; set; } // Personal, Team, Course

        [StringLength(255)]
        public string ThemeColor { get; set; }

        public bool EnableReminders { get; set; } = true;

        // Cải tiến: Cấu hình đồng bộ
        public bool EnableSync { get; set; } = false;
        public string SyncSource { get; set; }
        public string SyncSettings { get; set; }

        // Cải tiến: Cấu hình chia sẻ
        public string SharedWith { get; set; }
        public string ViewPermissions { get; set; }
        public string EditPermissions { get; set; }

        public int? CreatedByUserId { get; set; }

        public bool IsPublic { get; set; } = false;
        public bool IsActive { get; set; } = true;

        // Cải tiến: Thời gian áp dụng
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        // Cải tiến: Lịch biểu tiết học
        public int? TotalSessions { get; set; }
        public string SessionPattern { get; set; }

        // Navigation properties
        [ForeignKey("CreatedByUserId")]
        public virtual User.User CreatedByUser { get; set; }

        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
    }
}
