using LexiFlow.Models.Cores;
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
    [Index(nameof(UserExamId), nameof(QuestionAnswerId), IsUnique = true, Name = "IX_UserAnswer_Exam_Question")]
    public class UserAnswer : AuditableEntity
    {
        /// <summary>
        /// Id câu trả lời (Tự tăng)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserAnswerId { get; set; }

        /// <summary>
        /// Liên kết đến bài thi của người dùng
        /// </summary>
        [Required]
        public int UserExamId { get; set; }

        /// <summary>
        /// Liên kết đến câu hỏi
        /// Trong đáp án đã được liên kết với câu hỏi
        /// </summary>
        [Required]
        public int QuestionAnswerId { get; set; }

        /// <summary>
        /// Đáp án người dùng nhập (cho câu hỏi tự luận)
        /// </summary>
        public string UserInput { get; set; }

        /// <summary>
        /// Câu trả lời đúng hay sai
        /// Dành cho câu hỏi trắc nghiệm
        /// Khi người dùng chọn đáp án, hệ thống sẽ kiểm tra và đánh dấu đúng sai
        /// </summary>
        public bool IsCorrectUserAnswer { get; set; } = false;

        /// <summary>
        /// Ghi chú thêm (nếu có)
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Lần thử (nếu cho phép làm lại)
        /// </summary>
        public int? Attempt { get; set; }

        /// <summary>
        /// Cờ đánh dấu để xem lại
        /// </summary>
        public bool IsFlagged { get; set; } = false;

        /// <summary>
        /// Thời gian trả lời câu hỏi
        /// </summary>
        public DateTime? AnsweredAt { get; set; }

        // Navigation properties

        /// <summary>
        /// Bài thi của người dùng liên kết
        /// </summary>
        [ForeignKey("UserExamId")]
        public virtual UserExam UserExam { get; set; }

        /// <summary>
        /// Câu hỏi liên kết
        /// </summary>
        [ForeignKey("QuestionAnswerId")]
        public virtual QuestionOption QuestionOption { get; set; }

    }
}
