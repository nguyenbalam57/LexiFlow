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
    /// Câu trả lời của người dùng
    /// </summary>
    [Index(nameof(UserExamId), nameof(QuestionId), IsUnique = true, Name = "IX_UserAnswer_Exam_Question")]
    public class UserAnswer : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }

        [Required]
        public int UserExamId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        public int? SelectedOptionId { get; set; }

        public string UserInput { get; set; }

        public bool IsCorrect { get; set; } = false;

        public int? TimeSpent { get; set; }

        public int? Attempt { get; set; }

        // Cải tiến: Mức độ tự tin
        [Range(1, 5)]
        public int? ConfidenceLevel { get; set; }

        // Cải tiến: Thời gian suy nghĩ
        public int? ThinkingTimeMs { get; set; }

        // Cải tiến: Số lần thay đổi đáp án
        public int? ChangeCount { get; set; } = 0;

        // Cải tiến: Cờ đánh dấu để xem lại
        public bool IsFlagged { get; set; } = false;

        public DateTime? AnsweredAt { get; set; }

        // Navigation properties
        [ForeignKey("UserExamId")]
        public virtual UserExam UserExam { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        [ForeignKey("SelectedOptionId")]
        public virtual QuestionOption SelectedOption { get; set; }
    }
}
