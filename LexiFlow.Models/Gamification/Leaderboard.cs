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
    /// Bảng xếp hạng
    /// </summary>
    [Index(nameof(LeaderboardName), IsUnique = true, Name = "IX_Leaderboard_Name")]
    public class Leaderboard : BaseEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaderboardId { get; set; }

        [Required]
        [StringLength(100)]
        public string LeaderboardName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        public string LeaderboardType { get; set; }

        [StringLength(20)]
        public string TimeFrame { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ResetPeriodDays { get; set; }

        // Cải tiến: Thông tin hiển thị
        [StringLength(50)]
        public string DisplayMode { get; set; } // Full, Top10, Top100
        public int? MaxEntriesDisplayed { get; set; }
        public bool ShowRelativePosition { get; set; } = true;

        // Cải tiến: Phần thưởng
        public string RewardTiers { get; set; }
        public bool HasPrizes { get; set; } = false;
        public string PrizeDescription { get; set; }

        // Cải tiến: Quy tắc tính điểm
        public string ScoringRules { get; set; }
        public float ScoreMultiplier { get; set; } = 1.0f;

        public bool IsActive { get; set; } = true;

        [StringLength(50)]
        public string Scope { get; set; }

        [StringLength(50)]
        public string EntityType { get; set; }

        public int? EntityId { get; set; }

        // Cải tiến: Liên kết sự kiện
        public int? EventId { get; set; }

        // Navigation properties
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        public virtual ICollection<LeaderboardEntry> LeaderboardEntries { get; set; }
    }
}
