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
    /// Kết quả làm bài thi của người dùng
    /// </summary>
    [Index(nameof(UserId), nameof(ExamId), Name = "IX_UserExam_User_Exam")]
    public class UserExam : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserExamId { get; set; }

        [Required]
        public int UserId { get; set; }

        public int? ExamId { get; set; }

        public int? CustomExamId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? Score { get; set; }

        public int? TotalQuestions { get; set; }

        public int? CorrectAnswers { get; set; }

        // Cải tiến: Các số liệu thống kê chi tiết
        public int? IncorrectAnswers { get; set; }
        public int? UnansweredQuestions { get; set; }
        public int? VocabularyScore { get; set; }
        public int? GrammarScore { get; set; }
        public int? ReadingScore { get; set; }
        public int? ListeningScore { get; set; }

        // Cải tiến: Thời gian hoàn thành
        public int? CompletionTimeMinutes { get; set; }

        // Cải tiến: Kết quả đậu/rớt
        public bool? IsPassed { get; set; }

        // Cải tiến: Xếp hạng
        public string Grade { get; set; }
        public int? Percentile { get; set; }

        public bool IsCompleted { get; set; } = false;

        public string ExamFeedback { get; set; }

        public string UserNotes { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("ExamId")]
        public virtual JLPTExam Exam { get; set; }

        [ForeignKey("CustomExamId")]
        public virtual Practice.CustomExam CustomExam { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
        public virtual ICollection<Analytics.ExamAnalytic> ExamAnalytics { get; set; }
    }
}
