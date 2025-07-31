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

namespace LexiFlow.Models.Gamification
{
    /// <summary>
    /// Nhiệm vụ hàng ngày của người dùng
    /// </summary>
    [Index(nameof(UserId), nameof(TaskId), nameof(TaskDate), IsUnique = true, Name = "IX_UserDailyTask_User_Task_Date")]
    public class UserDailyTask : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserTaskId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int TaskId { get; set; }

        [Required]
        public DateTime TaskDate { get; set; }

        public DateTime? CompletedAt { get; set; }

        public int CurrentProgress { get; set; } = 0;

        public int? MaxProgress { get; set; }

        // Cải tiến: Tracking chi tiết
        public string ProgressDetails { get; set; }
        public float CompletionPercentage { get; set; } = 0;

        // Cải tiến: Thời gian
        public int? TimeSpentMinutes { get; set; }

        // Cải tiến: Điều chỉnh phần thưởng
        public float RewardMultiplier { get; set; } = 1.0f;
        public int? BonusPoints { get; set; }

        // Cải tiến: Ghi chú người dùng
        public string UserNotes { get; set; }

        public bool IsCompleted { get; set; } = false;
        public bool IsRewarded { get; set; } = false;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("TaskId")]
        public virtual DailyTask Task { get; set; }
    }
}
