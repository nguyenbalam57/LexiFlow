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
    /// Huy hiệu thành tích
    /// </summary>
    [Index(nameof(BadgeName), IsUnique = true, Name = "IX_Badge_Name")]
    [Index(nameof(BadgeCategory), Name = "IX_Badge_Category")]
    public class Badge : AuditableEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BadgeId { get; set; }

        [Required]
        [StringLength(100)]
        public string BadgeName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string IconPath { get; set; }

        // Cải tiến: Hiệu ứng và trực quan
        [StringLength(255)]
        public string AnimatedIconPath { get; set; }

        [StringLength(255)]
        public string LargeIconPath { get; set; }

        public bool HasAnimation { get; set; } = false;

        public string UnlockMessage { get; set; }

        // Cải tiến: Phần thưởng khi đạt được
        public int? ExperienceReward { get; set; }
        public int? PointsReward { get; set; }
        public int? CoinReward { get; set; }

        public string UnlockCriteria { get; set; }

        public int? RequiredPoints { get; set; }

        [StringLength(50)]
        public string BadgeCategory { get; set; }

        public int? Rarity { get; set; }

        // Cải tiến: Sự kiện và giới hạn
        public bool IsExclusive { get; set; } = false;
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableUntil { get; set; }
        public string RelatedAchievement { get; set; }

        // Cải tiến: Xếp hạng và hiển thị
        public int DisplayOrder { get; set; } = 0;
        public int TierLevel { get; set; } = 1;

        public bool IsActive { get; set; } = true;
        public bool IsHidden { get; set; } = false;

        // Navigation properties
        public virtual ICollection<UserBadge> UserBadges { get; set; }
        public virtual ICollection<Challenge> Challenges { get; set; }
        public virtual ICollection<Achievement> Achievements { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Level> Levels { get; set; }
    }
}
