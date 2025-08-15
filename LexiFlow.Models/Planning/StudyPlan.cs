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
    /// Kế hoạch học tập
    /// </summary>
    [Index(nameof(UserId), nameof(PlanName), IsUnique = true, Name = "IX_StudyPlan_User_Name")]
    public class StudyPlan : BaseEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudyPlanId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string PlanName { get; set; }

        [StringLength(10)]
        public int TargetLevel { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? TargetDate { get; set; }

        public string Description { get; set; }

        public int? MinutesPerDay { get; set; }

        // Cải tiến: Loại và phân loại
        [StringLength(50)]
        public string PlanType { get; set; } // General, JLPT, Business, Travel

        [StringLength(50)]
        public string Intensity { get; set; } // Light, Moderate, Intense

        public int? DaysPerWeek { get; set; } // Số ngày học mỗi tuần

        // Cải tiến: Trạng thái và theo dõi
        public bool IsActive { get; set; } = true;

        [StringLength(50)]
        public string CurrentStatus { get; set; } // NotStarted, InProgress, OnHold, Completed

        public float CompletionPercentage { get; set; } = 0;

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Cải tiến: Tùy chỉnh và lịch trình
        public string SchedulePattern { get; set; } // Mẫu lịch trình

        public string ExcludedDates { get; set; } // Ngày loại trừ

        public bool AutoAdjust { get; set; } = true; // Tự động điều chỉnh

        // Cải tiến: Thông báo và nhắc nhở
        public bool EnableReminders { get; set; } = true; // Bật nhắc nhở

        public string ReminderSettings { get; set; } // Cài đặt nhắc nhở

        // Cải tiến: Tích hợp và chia sẻ
        public bool SyncWithCalendar { get; set; } = false; // Đồng bộ với lịch

        public bool IsShared { get; set; } = false; // Chia sẻ với người khác

        public string SharedWith { get; set; } // Danh sách người dùng được chia sẻ

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("TargetLevel")]
        public virtual Exam.JLPTLevel JLPTLevel { get; set; }

        public virtual ICollection<StudyGoal> StudyGoals { get; set; }
        public virtual ICollection<StudyPlanItem> StudyPlanItems { get; set; }
        public virtual ICollection<Scheduling.Schedule> Schedules { get; set; }
    }
}
