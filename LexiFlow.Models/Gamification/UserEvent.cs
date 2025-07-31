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
    /// Sự kiện của người dùng
    /// </summary>
    [Index(nameof(EventId), nameof(UserId), IsUnique = true, Name = "IX_UserEvent_Event_User")]
    public class UserEvent : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserEventId { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public int Score { get; set; } = 0;

        public int? Rank { get; set; }

        // Cải tiến: Tiến trình tham gia
        public string ParticipationHistory { get; set; }
        public string CompletedChallenges { get; set; }
        public int? ActivityCount { get; set; } = 0;

        // Cải tiến: Thống kê thành tích
        public int HighestScore { get; set; } = 0;
        public int? HighestRank { get; set; }
        public float CompletionPercentage { get; set; } = 0;

        // Cải tiến: Phần thưởng
        public string EarnedRewards { get; set; }
        public int TotalPointsEarned { get; set; } = 0;
        public int TotalExperienceEarned { get; set; } = 0;

        public bool HasCompleted { get; set; } = false;
        public bool IsRewarded { get; set; } = false;

        public DateTime? LastActivityAt { get; set; }

        // Cải tiến: Tùy chỉnh hiển thị
        public string DisplayTitle { get; set; }
        public string AchievementBadge { get; set; }

        // Navigation properties
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }
    }
}
