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
    /// Thử thách của người dùng
    /// </summary>
    [Index(nameof(UserId), nameof(ChallengeId), IsUnique = true, Name = "IX_UserChallenge_User_Challenge")]
    public class UserChallenge : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserChallengeId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ChallengeId { get; set; }

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        public int CurrentProgress { get; set; } = 0;

        public int? MaxProgress { get; set; }

        // Cải tiến: Tracking chi tiết
        public string ProgressDetails { get; set; }
        public string RequirementsStatus { get; set; }

        // Cải tiến: Thống kê tiến độ
        public int CompletedRequirements { get; set; } = 0;
        public int TotalRequirements { get; set; } = 0;
        public float CompletionPercentage { get; set; } = 0;

        // Cải tiến: Thời gian
        public int? ElapsedTimeMinutes { get; set; } = 0;
        public int? RemainingTimeMinutes { get; set; }

        // Cải tiến: Trạng thái chi tiết
        [StringLength(50)]
        public string Status { get; set; } = "InProgress"; // InProgress, Completed, Failed, Abandoned

        public bool IsCompleted { get; set; } = false;
        public bool IsRewarded { get; set; } = false;
        public bool IsAbandoned { get; set; } = false;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("ChallengeId")]
        public virtual Challenge Challenge { get; set; }
    }
}
