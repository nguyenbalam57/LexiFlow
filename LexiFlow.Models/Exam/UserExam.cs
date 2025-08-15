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

namespace LexiFlow.Models.Exam
{
    /// <summary>
    /// Phiên làm bài của user
    /// </summary>
    [Index(nameof(UserId), nameof(StartTime), Name = "IX_UserExam_User_StartTime")]
    [Index(nameof(Status), Name = "IX_UserExam_Status")]
    public class UserExam : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserExamId { get; set; }

        [Required]
        public int UserId { get; set; }

        public int? ExamId { get; set; }

        [Required]
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        public DateTime? EndTime { get; set; }

        public int? Duration { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "InProgress"; // InProgress, Completed, Abandoned

        public int? TotalQuestions { get; set; }

        public int? CorrectAnswers { get; set; }

        public int? IncorrectAnswers { get; set; }

        public int? SkippedAnswers { get; set; }

        public double? ScorePercentage { get; set; }

        public int? ScorePoints { get; set; }

        public bool? IsPassed { get; set; }

        [StringLength(50)]
        public string Grade { get; set; }

        public string Notes { get; set; }

        // Cải tiến: Theo dõi tiến trình
        public int? CurrentQuestionIndex { get; set; }
        public string BookmarkedQuestions { get; set; }
        public string FlaggedQuestions { get; set; }

        // Cải tiến: Cài đặt làm bài
        public bool IsTimeLimited { get; set; } = true;
        public int? TimeLimit { get; set; }
        public int? TimeRemaining { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("ExamId")]
        public virtual JLPTExam Exam { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
