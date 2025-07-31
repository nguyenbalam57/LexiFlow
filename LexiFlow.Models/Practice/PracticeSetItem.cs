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
    /// Câu hỏi trong bộ luyện tập
    /// </summary>
    [Index(nameof(PracticeSetId), nameof(QuestionId), IsUnique = true, Name = "IX_PracticeSetItem_Set_Question")]
    public class PracticeSetItem : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        [Required]
        public int PracticeSetId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        public int? OrderNumber { get; set; }

        [StringLength(50)]
        public string PracticeMode { get; set; }

        // Cải tiến: Điểm cho câu hỏi này
        public int Points { get; set; } = 1;

        // Cải tiến: Gợi ý
        public string Hint { get; set; }

        // Cải tiến: Thời gian làm tối đa
        public int? TimeLimit { get; set; }

        // Cải tiến: Thống kê sử dụng
        public int AttemptCount { get; set; } = 0;
        public int CorrectCount { get; set; } = 0;
        public double? SuccessRate { get; set; }

        // Navigation properties
        [ForeignKey("PracticeSetId")]
        public virtual PracticeSet PracticeSet { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Exam.Question Question { get; set; }
    }
}
