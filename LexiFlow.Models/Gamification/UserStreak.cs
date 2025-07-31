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
    /// Chuỗi hoạt động liên tục của người dùng
    /// </summary>
    [Index(nameof(UserId), nameof(StreakType), IsUnique = true, Name = "IX_UserStreak_User_Type")]
    public class UserStreak : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StreakId { get; set; }

        [Required]
        public int UserId { get; set; }

        [StringLength(50)]
        public string StreakType { get; set; }

        public int CurrentCount { get; set; } = 0;

        public int LongestCount { get; set; } = 0;

        public DateTime? LastActivityDate { get; set; }

        // Cải tiến: Thống kê chi tiết
        public string StreakHistory { get; set; }
        public int TotalBreaks { get; set; } = 0;
        public float AverageStreakLength { get; set; } = 0;

        // Cải tiến: Trạng thái hiện tại
        public int DaysSinceLastActivity { get; set; } = 0;
        public bool IsAtRisk { get; set; } = false;
        public int? RemainingGracePeriodHours { get; set; }

        // Cải tiến: Phần thưởng
        public string StreakMilestones { get; set; }
        public int? NextMilestone { get; set; }
        public string NextReward { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        public int TotalStreakDays { get; set; } = 0;

        public int StreakFreezes { get; set; } = 0;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }
    }
}
