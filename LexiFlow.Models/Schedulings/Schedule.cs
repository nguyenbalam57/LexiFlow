using LexiFlow.Models.Cores;
using LexiFlow.Models.Users;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Models.Schedulings
{
    /// <summary>
    /// Lịch trình
    /// </summary>
    [Index(nameof(ScheduleName), Name = "IX_Schedule_Name")]
    public class Schedule : AuditableEntity
    {
        /// <summary>
        /// Id lịch trình (tự tăng).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleId { get; set; }

        /// <summary>
        /// Tên lịch trình (bắt buộc, tối đa 100 ký tự).
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ScheduleName { get; set; }

        /// <summary>
        /// Mô tả chi tiết về lịch trình.
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Thông tin chi tiết
        /// </summary>
        [StringLength(50)]
        public string ScheduleType { get; set; } // Personal, Team, Course

        /// <summary>
        /// Màu chủ đề để hiển thị lịch trình (tối đa 255 ký tự).
        /// </summary>
        [StringLength(255)]
        public string ThemeColor { get; set; }
        /// <summary>
        /// Bật tính năng nhắc nhở cho lịch trình
        /// </summary>
        public bool EnableReminders { get; set; } = true;

        /// <summary>
        /// Cấu hình đồng bộ
        /// </summary>
        public bool EnableSync { get; set; } = false;
        public string SyncSource { get; set; }
        public string SyncSettings { get; set; }

        /// <summary>
        /// Cấu hình chia sẻ
        /// </summary>
        public string SharedWith { get; set; }
        public string ViewPermissions { get; set; }
        public string EditPermissions { get; set; }

        public int? CreatedByUserId { get; set; }
        public int? StudyPlanId { get; set; }

        public bool IsPublic { get; set; } = false;

        /// <summary>
        /// Thời gian áp dụng
        /// </summary>
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// Lịch biểu tiết học
        /// </summary>
        public int? TotalSessions { get; set; }
        public string SessionPattern { get; set; }

        // Navigation properties
        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("StudyPlanId")]
        public virtual StudyPlan StudyPlan { get; set; }

        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
    }
}
