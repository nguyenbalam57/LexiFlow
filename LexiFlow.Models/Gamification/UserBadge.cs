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
    /// Huy hiệu của người dùng
    /// </summary>
    [Index(nameof(UserId), nameof(BadgeId), IsUnique = true, Name = "IX_UserBadge_User_Badge")]
    public class UserBadge : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserBadgeId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BadgeId { get; set; }

        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

        public bool IsDisplayed { get; set; } = true;

        public bool IsFavorite { get; set; } = false;

        public int EarnCount { get; set; } = 1;

        // Cải tiến: Tracking tiến trình
        public float? ProgressPercentage { get; set; } = 100;
        public string ProgressDescription { get; set; }

        // Cải tiến: Lưu trữ context
        public string AchievementContext { get; set; }
        public int? RelatedEntityId { get; set; }
        [StringLength(50)]
        public string RelatedEntityType { get; set; }

        // Cải tiến: Tùy chỉnh hiển thị
        [StringLength(50)]
        public string DisplayPosition { get; set; } // Profile, Showcase, Hidden
        public int DisplayOrder { get; set; } = 0;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("BadgeId")]
        public virtual Badge Badge { get; set; }
    }
}
