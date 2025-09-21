using LexiFlow.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Exam
{
    /// <summary>
    /// Phiên làm bài của user với tracking chi tiết
    /// </summary>
    [Index(nameof(UserId), nameof(StartTime), Name = "IX_UserExam_User_StartTime")]
    [Index(nameof(Status), Name = "IX_UserExam_Status")]
    [Index(nameof(UserId), nameof(Status), Name = "IX_UserExam_User_Status")]
    [Index(nameof(IsDeleted), nameof(Status), Name = "IX_UserExam_SoftDelete_Status")]
    [Index(nameof(EndTime), Name = "IX_UserExam_EndTime")]
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

        public int? Duration { get; set; } // in seconds

        [StringLength(50)]
        public string Status { get; set; } = ExamStatus.InProgress;

        // Scoring
        public int? TotalQuestions { get; set; }
        public int? CorrectAnswers { get; set; }
        public int? IncorrectAnswers { get; set; }
        public int? SkippedAnswers { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? ScorePercentage { get; set; }
        public int? ScorePoints { get; set; }
        public bool? IsPassed { get; set; }

        [StringLength(10)]
        public string Grade { get; set; }

        public string Notes { get; set; }

        // Progress tracking
        public int? CurrentQuestionIndex { get; set; } = 0;
        public string BookmarkedQuestions { get; set; } // JSON array
        public string FlaggedQuestions { get; set; } // JSON array

        // Time management
        public bool IsTimeLimited { get; set; } = true;
        public int? TimeLimit { get; set; } // Total time in seconds
        public int? TimeRemaining { get; set; }

        // Advanced analytics
        public string QuestionTimeSpent { get; set; } // JSON: {questionId: timeInSeconds}
        public int PauseCount { get; set; } = 0;
        public int TotalPauseTime { get; set; } = 0; // in seconds

        // Browser/Device info
        [StringLength(200)]
        public string UserAgent { get; set; }
        [StringLength(50)]
        public string IpAddress { get; set; }
        [StringLength(50)]
        public string DeviceType { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("ExamId")]
        public virtual JLPTExam Exam { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }

        // Business methods
        public bool IsCompleted => Status == ExamStatus.Completed;
        public bool IsInProgress => Status == ExamStatus.InProgress;
        public bool IsAbandoned => Status == ExamStatus.Abandoned;

        public void CompleteExam()
        {
            EndTime = DateTime.UtcNow;
            Duration = (int)(EndTime.Value - StartTime).TotalSeconds;
            Status = ExamStatus.Completed;
            CalculateResults();
        }

        public void AbandonExam()
        {
            EndTime = DateTime.UtcNow;
            Duration = (int)(EndTime.Value - StartTime).TotalSeconds;
            Status = ExamStatus.Abandoned;
        }

        private void CalculateResults()
        {
            if (TotalQuestions > 0)
            {
                ScorePercentage = (decimal)(CorrectAnswers ?? 0) * 100 / TotalQuestions.Value;
                IsPassed = ScorePercentage >= 60; // Assuming 60% pass rate

                // Grade calculation
                Grade = ScorePercentage switch
                {
                    >= 90 => "A",
                    >= 80 => "B",
                    >= 70 => "C",
                    >= 60 => "D",
                    _ => "F"
                };
            }
        }

        // Helper methods for JSON fields
        public List<int> GetBookmarkedQuestions()
        {
            if (string.IsNullOrEmpty(BookmarkedQuestions))
                return new List<int>();
            return JsonSerializer.Deserialize<List<int>>(BookmarkedQuestions);
        }

        public void SetBookmarkedQuestions(List<int> questions)
        {
            BookmarkedQuestions = JsonSerializer.Serialize(questions);
        }

        public List<int> GetFlaggedQuestions()
        {
            if (string.IsNullOrEmpty(FlaggedQuestions))
                return new List<int>();
            return JsonSerializer.Deserialize<List<int>>(FlaggedQuestions);
        }

        public void SetFlaggedQuestions(List<int> questions)
        {
            FlaggedQuestions = JsonSerializer.Serialize(questions);
        }
    }

    /// <summary>
    /// Constants cho exam status
    /// </summary>
    public static class ExamStatus
    {
        public const string InProgress = "InProgress";
        public const string Completed = "Completed";
        public const string Abandoned = "Abandoned";
        public const string Paused = "Paused";
        public const string Reviewing = "Reviewing";
    }
}
