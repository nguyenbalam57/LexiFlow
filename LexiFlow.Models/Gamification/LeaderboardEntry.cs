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
    /// Mục trong bảng xếp hạng
    /// </summary>
    [Index(nameof(LeaderboardId), nameof(UserId), IsUnique = true, Name = "IX_LeaderboardEntry_Leaderboard_User")]
    public class LeaderboardEntry : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntryId { get; set; }

        [Required]
        public int LeaderboardId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int Score { get; set; }

        public int? Rank { get; set; }

        public int? PreviousRank { get; set; }

        public int? RankChange { get; set; }

        // Cải tiến: Thông tin chi tiết
        public string ScoreBreakdown { get; set; }
        public int? ScoreLastUpdated { get; set; }
        public int? HighestRank { get; set; }
        public DateTime? HighestRankDate { get; set; }

        // Cải tiến: Phần thưởng
        public string EarnedRewards { get; set; }
        public bool IsRewardClaimed { get; set; } = false;
        public DateTime? RewardClaimedAt { get; set; }

        // Cải tiến: Hiển thị
        public string CustomTitle { get; set; }
        public string BadgeToDisplay { get; set; }

        // Navigation properties
        [ForeignKey("LeaderboardId")]
        public virtual Leaderboard Leaderboard { get; set; }

        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }
    }
}
