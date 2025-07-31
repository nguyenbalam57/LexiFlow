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
    /// Phân tích kết quả thi
    /// </summary>
    [Index(nameof(UserExamId), IsUnique = true, Name = "IX_ExamAnalytic_UserExam")]
    public class ExamAnalytic : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnalyticsId { get; set; }

        [Required]
        public int UserExamId { get; set; }

        public float? VocabularyScore { get; set; }

        public float? GrammarScore { get; set; }

        public float? ReadingScore { get; set; }

        public float? ListeningScore { get; set; }

        public string WeakAreas { get; set; }

        public string StrongAreas { get; set; }

        public string Recommendations { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        // Cải tiến: Phân tích chuyên sâu
        public int? TotalCorrect { get; set; }
        public int? TotalIncorrect { get; set; }
        public int? UnansweredCount { get; set; }

        public float? CorrectRate { get; set; }
        public float? SkippedRate { get; set; }

        public int? AverageResponseTimeMs { get; set; }
        public int? FastestResponseTimeMs { get; set; }
        public int? SlowestResponseTimeMs { get; set; }

        // Cải tiến: So sánh với lần thi trước
        public int? PreviousExamId { get; set; }
        public float? ScoreImprovement { get; set; }
        public string ImprovedAreas { get; set; }
        public string DeclinedAreas { get; set; }

        // Cải tiến: So sánh với điểm trung bình
        public float? AverageScoreComparison { get; set; }
        public int? PercentileRank { get; set; }

        // Navigation properties
        [ForeignKey("UserExamId")]
        public virtual Exam.UserExam UserExam { get; set; }

        [ForeignKey("PreviousExamId")]
        public virtual Exam.UserExam PreviousExam { get; set; }
    }
}
