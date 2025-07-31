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

namespace LexiFlow.Models.Analytics
{
    /// <summary>
    /// Phân tích kết quả luyện tập
    /// </summary>
    [Index(nameof(UserPracticeId), IsUnique = true, Name = "IX_PracticeAnalytic_UserPractice")]
    public class PracticeAnalytic : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnalyticsId { get; set; }

        [Required]
        public int UserPracticeId { get; set; }

        public string SkillsAnalysis { get; set; }

        public string LearningCurve { get; set; }

        public string ProblemAreas { get; set; }

        public int? MasteryPercentage { get; set; }

        public string Recommendations { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        // Cải tiến: Phân tích chuyên sâu
        public int TotalItems { get; set; } = 0;
        public int CompletedItems { get; set; } = 0;
        public int CorrectItems { get; set; } = 0;

        public float CompletionRate { get; set; } = 0;
        public float AccuracyRate { get; set; } = 0;

        public int? TotalTimeSpentMinutes { get; set; }
        public int? AverageTimePerItemSeconds { get; set; }

        // Cải tiến: Phân tích tiến triển
        public int? ImprovementRate { get; set; }
        public string LearningStyle { get; set; }
        public string EffectiveStudyPattern { get; set; }

        // Cải tiến: Đề xuất nâng cao
        public string NextLevelRecommendations { get; set; }
        public string ResourceRecommendations { get; set; }
        public string StudyScheduleRecommendations { get; set; }

        // Navigation properties
        [ForeignKey("UserPracticeId")]
        public virtual Practice.UserPracticeSet UserPracticeSet { get; set; }
    }
}
