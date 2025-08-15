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
    /// Kỳ thi JLPT
    /// </summary>
    [Index(nameof(ExamName), Name = "IX_Exam_Name")]
    [Index(nameof(Level), nameof(Year), nameof(Month), IsUnique = true, Name = "IX_Exam_Level_Date")]
    [Index(nameof(CreatedByUserId), Name = "IX_JLPTExam_CreatedBy")]
    public class JLPTExam : AuditableEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamId { get; set; }

        [Required]
        [StringLength(100)]
        public string ExamName { get; set; }

        [Required]
        [StringLength(10)]
        public string Level { get; set; }

        public int? LevelId { get; set; }

        public int? Year { get; set; }

        [StringLength(20)]
        public string Month { get; set; }

        public int? TotalTime { get; set; }

        public int? TotalScore { get; set; }

        public int? TotalQuestions { get; set; }

        // Cải tiến: Điểm đỗ
        public int? PassingScore { get; set; }

        // Cải tiến: Chi tiết cấu trúc đề thi
        public bool HasListeningSection { get; set; } = true;
        public bool HasReadingSection { get; set; } = true;
        public bool HasVocabularySection { get; set; } = true;
        public bool HasGrammarSection { get; set; } = true;

        [StringLength(100)]
        public string ExamVersion { get; set; }

        // Cải tiến: Độ khó
        [Range(1, 5)]
        public int Difficulty { get; set; } = 3;

        // Cải tiến: Mô tả và ghi chú
        public string Description { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        public bool IsOfficial { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public int? CreatedByUserId { get; set; }

        // Navigation properties
        [ForeignKey("CreatedByUserId")]
        public virtual User.User CreatedByUser { get; set; }

        [ForeignKey("LevelId")]
        public virtual JLPTLevel JLPTLevel { get; set; }

        public virtual ICollection<JLPTSection> Sections { get; set; }
        public virtual ICollection<UserExam> UserExams { get; set; }
    }
}
