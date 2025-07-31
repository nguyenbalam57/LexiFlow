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
    /// Phần trong kỳ thi JLPT
    /// </summary>
    [Index(nameof(ExamId), nameof(SectionName), IsUnique = true, Name = "IX_Section_Exam_Name")]
    public class JLPTSection : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SectionId { get; set; }

        [Required]
        public int ExamId { get; set; }

        [StringLength(100)]
        public string SectionName { get; set; }

        [StringLength(50)]
        public string SectionType { get; set; }

        public int? OrderNumber { get; set; }

        public int? TimeAllocation { get; set; }

        public int? ScoreAllocation { get; set; }

        public int? QuestionCount { get; set; }

        // Cải tiến: Hướng dẫn chi tiết
        public string Instructions { get; set; }

        // Cải tiến: Tài liệu đính kèm
        [StringLength(255)]
        public string AttachmentUrl { get; set; }

        // Cải tiến: Trọng số và điểm
        public double? WeightPercentage { get; set; }
        public int? PassingScore { get; set; }

        // Navigation properties
        [ForeignKey("ExamId")]
        public virtual JLPTExam Exam { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
