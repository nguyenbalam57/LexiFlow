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
    /// Thử thách trong hệ thống gamification
    /// </summary>
    [Index(nameof(ChallengeName), Name = "IX_Challenge_Name")]
    public class Challenge : AuditableEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChallengeId { get; set; }

        [Required]
        [StringLength(100)]
        public string ChallengeName { get; set; }

        public string Description { get; set; }

        public int? PointsReward { get; set; }

        public int? ExperienceReward { get; set; }

        public int? BadgeId { get; set; }

        // Cải tiến: Phần thưởng bổ sung
        public int? CoinReward { get; set; }
        public string ItemRewards { get; set; }
        public string FeatureUnlocks { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? DurationDays { get; set; }

        [StringLength(50)]
        public string ChallengeType { get; set; }

        [StringLength(20)]
        public string Difficulty { get; set; }

        // Cải tiến: Chi tiết thử thách
        public int? RequiredCompletions { get; set; } = 1;
        public string CompletionCriteria { get; set; }
        public string FailureCriteria { get; set; }

        // Cải tiến: Thuộc tính sự kiện
        public int? EventId { get; set; }
        public int? SeasonId { get; set; }
        public bool IsSeasonalChallenge { get; set; } = false;

        // Cải tiến: Hạn chế truy cập
        public int? MinimumLevel { get; set; }
        public int? PrerequisiteChallengeId { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsRecurring { get; set; } = false;

        [StringLength(100)]
        public string RecurrencePattern { get; set; }

        // Navigation properties
        [ForeignKey("BadgeId")]
        public virtual Badge Badge { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [ForeignKey("PrerequisiteChallengeId")]
        public virtual Challenge PrerequisiteChallenge { get; set; }

        public virtual ICollection<ChallengeRequirement> ChallengeRequirements { get; set; }
        public virtual ICollection<UserChallenge> UserChallenges { get; set; }
        public virtual ICollection<Challenge> DependentChallenges { get; set; }
    }
}
