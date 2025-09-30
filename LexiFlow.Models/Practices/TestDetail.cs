using LexiFlow.Models.Cores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Practices
{
    /// <summary>
    /// Chi tiết kết quả kiểm tra
    /// </summary>
    public class TestDetail : BaseEntity
    {
        /// <summary>
        /// Loại kỹ năng: Listening, Reading, Speaking, Writing
        /// </summary>
        public string? SkillType { get; set; }

        /// <summary>
        /// Khóa chính của chi tiết kiểm tra Tự tăng
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestDetailId { get; set; }   

        /// <summary>
        /// Khóa ngoại tham chiếu đến bảng TestResult
        /// </summary>
        [Required]
        public int TestResultId { get; set; }

        /// <summary>
        /// Khóa ngoại tham chiếu đến bảng Vocabulary (nếu có)
        /// </summary>
        public int? VocabularyId { get; set; }

        // Cải tiến: Liên kết với câu hỏi

        /// <summary>
        /// Khóa ngoại tham chiếu đến bảng Question (nếu có)
        /// </summary>
        public int? QuestionId { get; set; }

        /// <summary>
        /// Đáp án đúng hay sai
        /// </summary>
        public bool IsCorrect { get; set; } = false;

        /// <summary>
        /// Điểm số cho câu hỏi này
        /// </summary>
        public int? TimeSpent { get; set; }

        /// <summary>
        /// Điểm số cho câu hỏi này
        /// </summary>
        [StringLength(255)]
        public string UserAnswer { get; set; }

        // Cải tiến: Đánh giá khó dễ

        /// <summary>
        /// Đánh giá độ khó của câu hỏi: Easy, Normal, Hard
        /// </summary>
        [StringLength(20)]
        public string Difficulty { get; set; } // Easy, Normal, Hard

        // Cải tiến: Ghi chú

        /// <summary>
        /// Ghi chú thêm về câu hỏi hoặc đáp án
        /// </summary>
        public string Notes { get; set; }

        // Navigation properties
        [ForeignKey("TestResultId")]
        public virtual TestResult TestResult { get; set; }

        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }
    }
}
