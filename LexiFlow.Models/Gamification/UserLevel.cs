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
    /// Cấp độ của người dùng
    /// </summary>
    [Index(nameof(UserId), IsUnique = true, Name = "IX_UserLevel_User")]
    public class UserLevel : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserLevelId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int LevelId { get; set; }

        public int CurrentExperience { get; set; } = 0;

        public int? ExperienceToNextLevel { get; set; }

        public DateTime? LevelUpDate { get; set; }

        public int TotalExperienceEarned { get; set; } = 0;

        public int DaysAtCurrentLevel { get; set; } = 0;

        // Cải tiến: Thống kê tiến triển
        public int LifetimeLevelUps { get; set; } = 0;
        public int HighestLevelAchieved { get; set; } = 1;

        // Cải tiến: Tốc độ tiến triển
        public float? DailyExperienceAverage { get; set; }
        public int? DaysToNextLevel { get; set; }

        // Cải tiến: Lịch sử cấp độ
        public string LevelHistory { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("LevelId")]
        public virtual Level Level { get; set; }
    }
}
