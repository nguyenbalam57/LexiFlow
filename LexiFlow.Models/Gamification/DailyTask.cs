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
    /// Nhiệm vụ hàng ngày
    /// </summary>
    [Index(nameof(TaskName), Name = "IX_DailyTask_Name")]
    public class DailyTask : BaseEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Required]
        [StringLength(100)]
        public string TaskName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? PointsReward { get; set; }

        public int? ExperienceReward { get; set; }

        // Cải tiến: Phần thưởng bổ sung
        public int? CoinReward { get; set; }
        public string ItemRewards { get; set; }
        public int? StreakBonus { get; set; }

        [StringLength(50)]
        public string TaskCategory { get; set; }

        [StringLength(50)]
        public string TaskType { get; set; }

        public int? Difficulty { get; set; }

        // Cải tiến: Chi tiết nhiệm vụ
        public string CompletionCriteria { get; set; }
        public int? EstimatedTimeMinutes { get; set; }

        // Cải tiến: Hạn chế truy cập
        public int? MinimumLevel { get; set; }
        public bool RequiresPreviousCompletion { get; set; } = false;

        // Cải tiến: Thuộc tính sự kiện
        public int? EventId { get; set; }
        public bool IsSpecialTask { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public int? RecurrenceDays { get; set; }

        // Navigation properties
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        public virtual ICollection<DailyTaskRequirement> DailyTaskRequirements { get; set; }
        public virtual ICollection<UserDailyTask> UserDailyTasks { get; set; }
    }
}
