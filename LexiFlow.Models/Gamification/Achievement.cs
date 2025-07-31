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
    /// Thành tựu trong hệ thống gamification
    /// </summary>
    [Index(nameof(AchievementName), IsUnique = true, Name = "IX_Achievement_Name")]
    public class Achievement : AuditableEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AchievementId { get; set; }

        [Required]
        [StringLength(100)]
        public string AchievementName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string IconPath { get; set; }

        public int? PointsReward { get; set; }

        public int? ExperienceReward { get; set; }

        public int? BadgeId { get; set; }

        // Cải tiến: Phần thưởng bổ sung
        public int? CoinReward { get; set; }
        public string ItemRewards { get; set; }

        // Cải tiến: Hiệu ứng trực quan
        [StringLength(255)]
        public string AnimatedIconPath { get; set; }
        public string UnlockMessage { get; set; }
        public string CompletionEffect { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        public int? Tier { get; set; }

        // Cải tiến: Tiến trình theo cấp độ
        public int MaxTier { get; set; } = 1;
        public string TierRequirements { get; set; }
        public string TierRewards { get; set; }

        // Cải tiến: Tiêu chí hoàn thành
        public string CompletionCriteria { get; set; }

        // Cải tiến: Liên kết thành tựu
        public int? ParentAchievementId { get; set; }
        public string RelatedAchievements { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsSecret { get; set; } = false;

        // Navigation properties
        [ForeignKey("BadgeId")]
        public virtual Badge Badge { get; set; }

        [ForeignKey("ParentAchievementId")]
        public virtual Achievement ParentAchievement { get; set; }

        public virtual ICollection<AchievementRequirement> AchievementRequirements { get; set; }
        public virtual ICollection<UserAchievement> UserAchievements { get; set; }
        public virtual ICollection<Achievement> ChildAchievements { get; set; }
    }
}
