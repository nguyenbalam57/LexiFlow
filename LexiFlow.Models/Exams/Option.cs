using LexiFlow.Models.Cores;
using LexiFlow.Models.Medias;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Models.Exam
{
    /// <summary>
    /// Đáp án có thể tái sử dụng cho nhiều câu hỏi.
    /// Liên kết tuỳ chọn đến JLPTLevel, JLPTSection, MediaFile, và chứa metadata như điểm/ghi chú.
    /// Có thể dùng cho cả câu hỏi trắc nghiệm và tự luận.
    /// Đáp án có nhiều loại khác nhau text, số, đúng/sai, âm thanh, hình ảnh...
    /// </summary>
    [Index(nameof(AnswerText), Name = "IX_Answer_Text")]
    public class Option : AuditableEntity
    {
        /// <summary>
        /// Id đáp án (Tự tăng)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OptionId { get; set; }

        /// <summary>
        /// Nội dung đáp án
        /// </summary>
        [StringLength(1000)]
        public string AnswerText { get; set; }

        /// <summary>
        /// Giải thích khi cần hiển thị sau khi chấm hoặc để giáo viên tham khảo
        /// Chỉ giải thích về đáp án này, không phải giải thích cho đáp án đúng
        /// </summary>
        public string Explanation { get; set; }

        /// <summary>
        /// Loại câu hỏi mà đáp án này thuộc về
        /// Mặc định là MULTIPLE_CHOICE
        /// Các loại khác có thể là: FILL_IN_BLANK, TRUE_FALSE,...
        /// </summary>
        [Required]
        public string QuestionType { get; set; } = QuestionTypeStatics.MULTIPLE_CHOICE;

        /// <summary>
        /// Loại đáp án mà đối tượng này đại diện
        /// Mặc định là TEXT
        /// Các loại khác có thể là: IMAGE, AUDIO, VIDEO, NUMBER, TRUE_FALSE
        /// </summary>
        public string AnswerType { get; set; } = AnswerTypeStatics.TEXT;

        /// <summary>
        /// Nếu muốn gán đáp án theo cấp độ JLPT
        /// (tham chiếu tới bảng JLPTLevel nếu có)
        /// </summary>
        public int? JLPTLevelId { get; set; }

        [ForeignKey("JLPTLevelId")]
        public virtual JLPTLevel JLPTLevel { get; set; }

        /// <summary>
        /// Nếu đáp án liên quan tới một section cụ thể
        /// </summary>
        public int? JLPTSectionId { get; set; }

        [ForeignKey("JLPTSectionId")]
        public virtual JLPTSection JLPTSection { get; set; }

        /// <summary>
        /// Ghi chú thêm về đáp án
        /// </summary>
        [StringLength(200)]
        public string Tags { get; set; }

        /// <summary>
        /// Tỷ lệ chọn đáp án này
        /// </summary>
        public double? SelectionRate { get; set; }

        /// <summary>
        /// Mức độ nhiễu
        /// </summary>
        public int? DistractorLevel { get; set; }

        /// <summary>
        /// Các file media liên quan đến đáp án (nếu có)
        /// </summary>
        public virtual ICollection<MediaFile> MediaFiles { get; set; }

        /// <summary>
        /// Các liên kết tới Question qua bảng trung gian QuestionOption
        /// </summary>
        public virtual ICollection<QuestionOption> QuestionOptions { get; set; }
    }

    /// <summary>
    /// Các loại đáp án hỗ trợ cho câu hỏi
    /// </summary>
    public static class AnswerTypeStatics
    {
        /// <summary>
        /// Loại text (mặc định)
        /// </summary>
        public const string TEXT = "TEXT";

        /// <summary>
        /// Loại hình ảnh
        /// </summary>
        public const string IMAGE = "IMAGE";

        /// <summary>
        /// Loại âm thanh
        /// </summary>
        public const string AUDIO = "AUDIO";

        /// <summary>
        /// Loại video
        /// </summary>
        public const string VIDEO = "VIDEO";

        /// <summary>
        /// Loại số
        /// </summary>
        public const string NUMBER = "NUMBER";

        /// <summary>
        /// Loại đúng/sai
        /// </summary>
        public const string TRUE_FALSE = "TRUE_FALSE";
    }
}