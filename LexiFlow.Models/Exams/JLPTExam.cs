using LexiFlow.Models.Cores;
using LexiFlow.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Exams
{
    /// <summary>
    /// Kỳ thi JLPT
    /// </summary>
    [Index(nameof(ExamName), Name = "IX_Exam_Name")]
    [Index( nameof(Year), nameof(Month), IsUnique = true, Name = "IX_Exam_Level_Date")]
    public class JLPTExam : AuditableEntity
    {
        /// <summary>
        /// Id kỳ thi (Tự tăng)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JLPTExamId { get; set; }

        /// <summary>
        /// Tên kỳ thi
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ExamName { get; set; }

        /// <summary>
        /// Liên kết đến bảng cấp độ JLPT
        /// </summary>
        public int? LevelId { get; set; }

        /// <summary>
        /// Năm tổ chức kỳ thi
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Tháng tổ chức kỳ thi (7 hoặc 12)
        /// </summary>
        [StringLength(20)]
        public string Month { get; set; }

        /// <summary>
        /// Tổng thời gian làm bài (phút)
        /// </summary>
        public int? TotalTime { get; set; }

        /// <summary>
        /// Tổng điểm số
        /// </summary>
        public int? TotalScore { get; set; }

        /// <summary>
        /// Tổng số câu hỏi
        /// </summary>
        public int? TotalQuestions { get; set; }

        /// <summary>
        /// Điểm đỗ
        /// </summary>
        public int? PassingScore { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        [StringLength(255)]
        public string Notes { get; set; }

        /// <summary>
        /// Kỳ thi chính thức do JLPT tổ chức
        /// </summary>
        public bool IsOfficial { get; set; } = false;

        [ForeignKey("LevelId")]
        public virtual JLPTLevel JLPTLevel { get; set; }

        public virtual ICollection<Section> Sections { get; set; }
        public virtual ICollection<UserExam> UserExams { get; set; }
    }
}
