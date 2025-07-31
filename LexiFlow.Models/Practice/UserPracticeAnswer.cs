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

namespace LexiFlow.Models.Practice
{
    /// <summary>
    /// Câu trả lời trong luyện tập
    /// </summary>
    [Index(nameof(UserPracticeId), nameof(QuestionId), Name = "IX_UserPracticeAnswer_Practice_Question")]
    public class UserPracticeAnswer : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PracticeAnswerId { get; set; }

        [Required]
        public int UserPracticeId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        public bool IsCorrect { get; set; } = false;

        public int? Attempt { get; set; }

        public DateTime? AnsweredAt { get; set; }

        public int? TimeTaken { get; set; }

        // Cải tiến: Spaced repetition
        public int? MemoryStrength { get; set; }
        public DateTime? NextReviewDate { get; set; }

        // Cải tiến: Chi tiết trả lời
        public string UserAnswer { get; set; }
        public int? SelectedOptionId { get; set; }

        // Cải tiến: Đánh giá mức độ khó
        [StringLength(20)]
        public string Difficulty { get; set; } // Easy, Good, Hard

        // Cải tiến: Ghi chú người dùng
        public string UserNotes { get; set; }

        // Navigation properties
        [ForeignKey("UserPracticeId")]
        public virtual UserPracticeSet UserPracticeSet { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Exam.Question Question { get; set; }

        [ForeignKey("SelectedOptionId")]
        public virtual Exam.QuestionOption SelectedOption { get; set; }
    }
}
