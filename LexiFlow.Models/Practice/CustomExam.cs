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
    /// Đề thi tùy chỉnh
    /// </summary>
    [Index(nameof(UserId), nameof(ExamName), Name = "IX_CustomExam_User_Name")]
    public class CustomExam : AuditableEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomExamId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string ExamName { get; set; }

        [StringLength(10)]
        public string Level { get; set; }

        public string Description { get; set; }

        public int? TimeLimit { get; set; }

        // Cải tiến: Cấu trúc đề thi
        public int QuestionCount { get; set; } = 0;
        public int TotalPoints { get; set; } = 0;
        public int? PassingScore { get; set; }

        // Cải tiến: Loại đề thi
        [StringLength(50)]
        public string ExamType { get; set; } // Practice, Mock, Assessment

        // Cải tiến: Thống kê sử dụng
        public int AttemptCount { get; set; } = 0;
        public double? AverageScore { get; set; }
        public double? PassRate { get; set; }

        public bool IsPublic { get; set; } = false;
        public bool IsFeatured { get; set; } = false;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        public virtual ICollection<CustomExamQuestion> CustomExamQuestions { get; set; }
        public virtual ICollection<Exam.UserExam> UserExams { get; set; }
    }
}
