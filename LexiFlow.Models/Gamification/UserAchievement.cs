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
    /// Thành tựu của người dùng
    /// </summary>
    [Index(nameof(UserId), nameof(AchievementId), IsUnique = true, Name = "IX_UserAchievement_User_Achievement")]
    public class UserAchievement : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserAchievementId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int AchievementId { get; set; }

        public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;

        public int CurrentTier { get; set; } = 1;

        public int? MaxTier { get; set; }

        public int ProgressValue { get; set; } = 0;

        public int? TargetValue { get; set; }

        // Cải tiến: Tracking chi tiết
        public string ProgressHistory { get; set; }
        public float CompletionPercentage { get; set; } = 0;

        // Cải tiến: Thời gian
        public DateTime? FirstProgressAt { get; set; }
        public int? DaysToUnlock { get; set; }

        // Cải tiến: Phần thưởng thực tế
        public int? ExperienceGranted { get; set; }
        public int? PointsGranted { get; set; }
        public int? CoinsGranted { get; set; }

        // Cải tiến: Hiển thị và tùy chỉnh
        public string DisplayStyle { get; set; }
        public int DisplayOrder { get; set; } = 0;

        public bool IsDisplayed { get; set; } = true;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("AchievementId")]
        public virtual Achievement Achievement { get; set; }
    }
}
