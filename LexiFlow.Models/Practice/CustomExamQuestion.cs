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
    /// Câu hỏi trong đề thi tùy chỉnh
    /// </summary>
    [Index(nameof(CustomExamId), nameof(QuestionId), IsUnique = true, Name = "IX_CustomExamQuestion_Exam_Question")]
    public class CustomExamQuestion : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomQuestionId { get; set; }

        [Required]
        public int CustomExamId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        public int? OrderNumber { get; set; }

        public int? ScoreValue { get; set; }

        // Cải tiến: Ghi đè lên các thiết lập của câu hỏi gốc
        public string QuestionTextOverride { get; set; }
        public string ExplanationOverride { get; set; }

        // Cải tiến: Thời gian làm tối đa
        public int? TimeLimit { get; set; }

        // Cải tiến: Thống kê sử dụng
        public int AttemptCount { get; set; } = 0;
        public int CorrectCount { get; set; } = 0;
        public double? SuccessRate { get; set; }

        // Navigation properties
        [ForeignKey("CustomExamId")]
        public virtual CustomExam CustomExam { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Exam.Question Question { get; set; }
    }
}
