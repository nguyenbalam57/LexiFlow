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
    /// Phần trong kỳ thi JLPT
    /// Có thể hỗ trợ tạo danh sách câu hỏi riêng cho phần bài tập.
    /// sau mỗi bài học thì sẽ có bài kiểm tra nhỏ.
    /// </summary>
    [Index(nameof(JLPTExamId), nameof(SectionName), IsUnique = true, Name = "IX_Section_Exam_Name")]
    public class Section : AuditableEntity
    {
        /// <summary>
        /// Id phần (Tự tăng)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SectionId { get; set; }

        /// <summary>
        /// Liên kết đến kỳ thi
        /// </summary>
        public int? JLPTExamId { get; set; }

        /// <summary>
        /// Tên phần (VD: Nghe, Đọc, Từ vựng, Ngữ pháp)
        /// </summary>
        [StringLength(100)]
        public string SectionName { get; set; }

        /// <summary>
        /// Loại phần (VD: Nghe, Đọc, Từ vựng, Ngữ pháp)
        /// </summary>
        public string SectionType { get; set; } = SectionTypeExtensions.Listening;

        /// <summary>
        /// Thời gian làm bài (phút)
        /// </summary>
        public int? TimeAllocation { get; set; }

        /// <summary>
        /// Phân bổ điểm số
        /// </summary>
        public int? ScoreAllocation { get; set; }

        /// <summary>
        /// Số lượng câu hỏi
        /// </summary>
        public int? QuestionCount { get; set; }

        /// <summary>
        /// Hướng dẫn chi tiết
        /// </summary>
        public string Instructions { get; set; }

        /// <summary>
        /// Tài liệu đính kèm
        /// </summary>
        [StringLength(255)]
        public string AttachmentUrl { get; set; }

        /// <summary>
        /// Trọng số và điểm
        /// </summary>
        public double? WeightPercentage { get; set; }

        /// <summary>
        /// Điểm đỗ cho phần này
        /// </summary>
        public int? PassingScore { get; set; }

        /// <summary>
        /// Phần cha (nếu có)
        /// </summary>
        public int? ParentSectionId { get; set; }

        // Navigation properties

        /// <summary>
        /// Liên kết đến kỳ thi
        /// </summary>
        [ForeignKey("JLPTExamId")]
        public virtual JLPTExam Exam { get; set; }

        /// <summary>
        /// Phần cha (nếu có)
        /// </summary>
        [ForeignKey("ParentSectionId")]
        public virtual Section ParentSection { get; set; }

        /// <summary>
        /// Các câu hỏi trong phần này
        /// Liên kết many-to-many qua bảng trung gian SectionQuestion
        /// </summary>
        public virtual ICollection<SectionQuestion> SectionQuestions { get; set; }
    }


    /// <summary>
    /// Các loại phần trong loại SectionType
    /// </summary>
    public static class SectionTypeExtensions
    { 
        public const string Listening = "Nghe";
        public const string Reading = "Đọc";
        public const string Vocabulary = "Từ vựng";
        public const string Grammar = "Ngữ pháp";
        public const string Writing = "Viết";
        public const string Speaking = "Nói";
    }
}
