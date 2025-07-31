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
    /// Cấp độ trong hệ thống gamification
    /// </summary>
    [Index(nameof(LevelNumber), IsUnique = true, Name = "IX_Level_Number")]
    public class Level : BaseEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LevelId { get; set; }

        [Required]
        [StringLength(50)]
        public string LevelName { get; set; }

        [Required]
        public int LevelNumber { get; set; }

        [Required]
        public int ExperienceRequired { get; set; }

        [StringLength(255)]
        public string IconPath { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        // Cải tiến: Hiệu ứng và hình ảnh
        [StringLength(255)]
        public string AnimatedIconPath { get; set; }

        [StringLength(255)]
        public string BackgroundPath { get; set; }

        public string LevelUpMessage { get; set; }

        // Cải tiến: Phần thưởng khi lên cấp
        public int? CoinReward { get; set; }
        public string ItemRewards { get; set; }
        public int? BadgeId { get; set; }

        // Cải tiến: Mở khóa tính năng
        public string UnlockedFeatures { get; set; }

        public string Benefits { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<UserLevel> UserLevels { get; set; }

        [ForeignKey("BadgeId")]
        public virtual Badge Badge { get; set; }
    }
}
