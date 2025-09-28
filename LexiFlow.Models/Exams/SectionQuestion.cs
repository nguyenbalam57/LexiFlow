using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Models.Exam
{
    /// <summary>
    /// Bảng trung gian many-to-many giữa Section và Question.
    /// Khóa chính composite (SectionId, QuestionId) nên được cấu hình trong DbContext.OnModelCreating.
    /// Cho phép lưu các thuộc tính riêng cho mỗi liên kết (ví dụ thứ tự, có hiển thị hay không, điểm phụ).
    /// </summary>
    public class SectionQuestion
    {
        /// <summary>
        /// ID duy nhất của liên kết (Primary Key)
        /// Được tự động tạo bởi database
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SectionQuestionId { get; set; }

        /// <summary>
        /// ID phần thi (Section)
        /// </summary>
        public int SectionId { get; set; }
        [ForeignKey("SectionId")]
        public virtual Section Section { get; set; }

        /// <summary>
        /// ID câu hỏi (Question)
        /// </summary>
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        /// <summary>
        /// Thứ tự hiển thị câu hỏi trong section
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Ghi chú riêng cho liên kết này
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Nếu cần, có thể override điểm cho câu hỏi khi xuất hiện trong section này
        /// </summary>
        public double? ScoreOverride { get; set; }

        /// <summary>
        /// Nếu true, câu hỏi này chỉ là tham khảo/không tính điểm trong section
        /// </summary>
        public bool IsOptional { get; set; } = false;
    }
}