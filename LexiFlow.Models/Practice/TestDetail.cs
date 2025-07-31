using LexiFlow.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Practice
{
    /// <summary>
    /// Chi tiết kết quả kiểm tra
    /// </summary>
    public class TestDetail : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestDetailId { get; set; }

        [Required]
        public int TestResultId { get; set; }

        public int? VocabularyId { get; set; }

        // Cải tiến: Liên kết với câu hỏi
        public int? QuestionId { get; set; }

        public bool IsCorrect { get; set; } = false;

        public int? TimeSpent { get; set; }

        [StringLength(255)]
        public string UserAnswer { get; set; }

        // Cải tiến: Đánh giá khó dễ
        [StringLength(20)]
        public string Difficulty { get; set; } // Easy, Normal, Hard

        // Cải tiến: Ghi chú
        public string Notes { get; set; }

        // Navigation properties
        [ForeignKey("TestResultId")]
        public virtual TestResult TestResult { get; set; }

        [ForeignKey("VocabularyId")]
        public virtual Learning.Vocabulary.Vocabulary Vocabulary { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Exam.Question Question { get; set; }
    }
}
