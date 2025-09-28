using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Models.Exam
{
    /// <summary>
    /// Bảng trung gian giữa Question và Option.
    /// Cho phép đánh dấu đáp án đúng cho từng câu hỏi, và ghi đè điểm/thuộc tính khi cần.
    /// Khóa chính composite (QuestionId, OptionId) cấu hình trong DbContext Fluent API.
    /// </summary>
    public class QuestionOption
    {
        /// <summary>
        /// Id bảng trung gian (Tự tăng)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionAnswerId { get; set; }

        /// <summary>
        /// Liên kết đến câu hỏi
        /// </summary>
        public int? QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        /// <summary>
        /// Liên kết đến đáp án
        /// </summary>
        public int? OptionId { get; set; }

        [ForeignKey("OptionId")]
        public virtual Option Option { get; set; }

        /// <summary>
        /// Giải thích đáp án cụ thể cho câu hỏi này (nếu có)
        /// </summary>
        public string Explanation { get; set; }

        /// <summary>
        /// Đánh dấu đáp án này là đúng cho câu hỏi liên kết
        /// </summary>
        public bool IsCorrect { get; set; } = false;

        /// <summary>
        /// Thứ tự hiển thị đáp án trong câu hỏi (nếu có)
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Ghi đè điểm (nếu muốn đáp án này có điểm khác so với Answer.Score)
        /// </summary>
        public double? ScoreOverride { get; set; }

        /// <summary>
        /// Ghi chú thêm về liên kết này (nếu cần)
        /// </summary>
        public string Note { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}